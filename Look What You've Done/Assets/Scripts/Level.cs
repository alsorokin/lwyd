using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityObject = UnityEngine.Object;

public class Level
{
    private const float DefaultScale = 0.3f;
    private const string DefaultLevelName = "test";

    private int _levelWidth;
    private int _levelHeight;
    private List<Actor> _actors = new List<Actor>();
    private Tile[,,] _levelTiles;
    private readonly List<Tile> _objects = new List<Tile>();
    private float _cameraSize = 3f;
    private Vector2 _playerSpawn = Vector2.zero;

    private const float DefaultPpu = 16f;
    private const float DefaultObjectLayerIndex = 100f;

    private GameObject _player;
    private Camera _camera;

    public float Scale
    {
        get => 10f / _cameraSize;
        set
        {
            _cameraSize = 10f / value;
            if (_camera != null)
            {
                _camera.orthographicSize = _cameraSize;
            }
        }
    }

    public Tile LeftmostTile => _levelTiles[0, 0, 0];

    public Tile RightmostTile => _levelTiles[_levelWidth - 1, _levelHeight - 1, 0];

    public Tile TopmostTile => RightmostTile;

    public Tile BottommostTile => LeftmostTile;

    public Level(float scale) : this()
    {
        Scale = scale;
    }

    public Level(float scale, string filePath)
    {
        LoadFromFile(filePath);
        Scale = scale;
        AddPlayer();
    }

    public Level() : this(DefaultScale, DefaultLevelName) { }

    private void AddPlayer()
    {
        // Adding player
        _player = UnityObject.Instantiate(Resources.Load<GameObject>("Tiles/Player"));
        float spawnX = _playerSpawn == Vector2.zero ? TranslateGridToUnits(_levelWidth / 2) : _playerSpawn.x;
        float spawnY = _playerSpawn == Vector2.zero ? TranslateGridToUnits(_levelHeight / 2) : _playerSpawn.y;
        _player.transform.position = new Vector3(spawnX, spawnY, 0f);
        Vector3 playerLocalScale = _player.transform.localScale;
        _player.transform.localScale = new Vector3(playerLocalScale.x, playerLocalScale.y, playerLocalScale.z);
        Actor playerActor = _player.GetComponent<Hero>();
        playerActor.SetLevel(this);
        playerActor.Cloneable = false;
        AddActor(playerActor);

        // Adding camera and its controller
        var cameraObj = new GameObject();
        _camera = cameraObj.AddComponent<Camera>();
        cameraObj.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, -5f);
        _camera.orthographic = true;
        _camera.orthographicSize = _cameraSize;
        var cameraController = cameraObj.AddComponent<CameraMovementController>();
        cameraController.Player = _player;
        cameraController.Level = this;
    }

    private void LoadFromFile(string fileName)
    {
        // parsing xml
        var filePath = $"Assets\\Levels\\{fileName}.tmx";
        string content = File.ReadAllText(filePath);
        var doc = new XmlDocument();
        doc.LoadXml(content);

        // getting the main node and its attributes
        XmlNodeList mapNodes = doc.GetElementsByTagName("map");
        if (mapNodes.Count == 0)
        {
            throw new Exception("Can\'t see the map node in level file " + fileName);
        }
        else if (mapNodes.Count > 1)
        {
            Debug.LogWarning("Multiple map nodes found! Will use only the first one.");
        }

        XmlNode mapNode = mapNodes[0];
        if (!int.TryParse(mapNode.Attributes["width"]?.Value, out _levelWidth))
        {
            _levelWidth = 100;
        }

        if (!int.TryParse(mapNode.Attributes["height"]?.Value, out _levelHeight))
        {
            _levelHeight = 100;
        }

        // getting layers
        IEnumerable<XmlNode> layerNodes = doc.GetElementsByTagName("layer").Cast<XmlNode>();
        int layerCount = layerNodes.Count();
        if (layerCount == 0)
        {
            throw new Exception("Can\'t see any layers in level file " + fileName);
        }

        _levelTiles = new Tile[_levelWidth, _levelHeight, layerCount];

        // getting data nodes
        XmlNode[] dataNodes = doc.GetElementsByTagName("data").Cast<XmlNode>().ToArray();
        if (dataNodes.Length == 0)
        {
            throw new Exception("Can\'t see the data node in level file " + fileName);
        }

        // reading layers from data nodes
        for (var z = 0; z < dataNodes.Length; z++)
        {
            XmlNode dataNode = dataNodes[z];
            string encoding = dataNode.Attributes["encoding"].Value;
            if (encoding != "csv")
            {
                throw new Exception("Unknown level data encoding: " + encoding);
            }

            var data = dataNode.InnerText.Trim();
            ParseCsvData(data, _levelWidth, _levelHeight, out uint[,] dataArray);
            for (int i = 0; i < _levelWidth; i++)
            {
                for (int j = 0; j < _levelHeight; j++)
                {
                    CreateTile(dataArray[i, j], i, _levelHeight - j - 1, z);
                }
            }
        }

        // getting object groups
        IEnumerable<XmlNode> objectGroups = doc.GetElementsByTagName("objectgroup").Cast<XmlNode>().Where(n => n.ParentNode.Name == "map");
        foreach (XmlNode objectGroup in objectGroups)
        {
            foreach (XmlNode obj in objectGroup.ChildNodes)
            {
                string gidValue = obj.Attributes["gid"]?.Value;
                if (string.IsNullOrEmpty(gidValue))
                {
                    // Gid is empty. This is either player spawn or spriteless collider. 
                    // Or some other object that we don't support yet.
                    if (obj.FirstChild != null && obj.FirstChild.Name == "point" && obj.Attributes["type"].Value == "player_spawn")
                    {
                        if (float.TryParse(obj.Attributes["x"].Value, out float spawnX)
                            && float.TryParse(obj.Attributes["y"].Value, out float spawnY))
                        {
                            spawnX = TranslatePixelsToUnits(spawnX);
                            spawnY = TranslatePixelsToUnits(TranslateYFromTmx(spawnY));
                            _playerSpawn = new Vector2(spawnX, spawnY);
                        }
                    }
                    // TODO: Parse other objects, like spriteless colliders
                    continue;
                }

                uint.TryParse(gidValue, out uint gid);
                float.TryParse(obj.Attributes["x"].Value, out float x);
                float.TryParse(obj.Attributes["y"].Value, out float y);
                y = TranslateYFromTmx(y);
                int.TryParse(obj.Attributes["rotation"]?.Value, out int rotation);

                // TODO: use width and height attributes?

                CreateObject(gid, x, y, DefaultObjectLayerIndex, rotation);
            }
        }
    }

    public GameObject SpawnGenericEnemyAt(int gridX, int gridY, bool isFree)
    {
        var genericEnemy = UnityObject.Instantiate(Resources.Load<GameObject>(isFree ? "Tiles/GenericFreeEnemyTile" : "Tiles/GenericGridEnemyTile"));
        genericEnemy.transform.position = new Vector3(TranslateGridToUnits(gridX), TranslateGridToUnits(gridY), 1f);
        var localScale = genericEnemy.transform.localScale;
        genericEnemy.transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
        var ge = genericEnemy.GetComponent<GenericEnemy>();
        ge.SetLevel(this);
        ge.Cloneable = true;
        ge.Health = ge.MaxHealth;
        AddActor(ge);

        return genericEnemy;
    }

    public int TranslateXToGrid(float x)
    {
        return (int)Math.Round(x);
    }

    public int TranslateYToGrid(float y)
    {
        return (int)Math.Round(y);
    }

    public float TranslateGridToUnits(int grid)
    {
        return grid;
    }

    public float TranslatePixelsToUnits(float pixels)
    {
        // if any tile is present, use first tile's ppu
        //float ppu = levelTiles.Length > 0 && levelTiles[0, 0, 0] != null ? levelTiles[0, 0, 0].SpriteRenderer.sprite.pixelsPerUnit : default_ppu;

        return pixels / DefaultPpu;
    }

    private float TranslateYFromTmx(float tmxY)
    {
        float levelHeightInPixels = _levelHeight * DefaultPpu;
        return levelHeightInPixels - tmxY;
    }

    public void AddActor(Actor actor)
    {
        if (!_actors.Contains(actor))
        {
            _actors.Add(actor);
        }
    }

    private void ParseCsvData(string data, int width, int height, out uint[,] dataArray)
    {
        var processedData = data.Replace("\r", "");
        dataArray = new uint[width, height];
        int j = 0;
        foreach (var line in processedData.Split('\n'))
        {
            int i = 0;
            var processedLine = line.Trim(',');
            foreach (string pos in processedLine.Split(','))
            {
                if (uint.TryParse(pos, out uint parsed))
                {
                    dataArray[i, j] = parsed;
                }
                else
                {
                    dataArray[i, j] = 0;
                }

                i++;
            }

            j++;
        }
    }

    private void CreateTile(uint gid, int w, int h, int z)
    {
        _levelTiles[w, h, z] = TileFactory.Instance.CreateTile(
            gid,
            TranslateGridToUnits(w),
            TranslateGridToUnits(h),
            0f - (z * 10));
    }

    private void CreateObject(uint gid, float pixelX, float pixelY, float z, int rotation = 0)
    {
        _objects.Add(TileFactory.Instance.CreateTile(
            gid,
            TranslatePixelsToUnits(pixelX),
            TranslatePixelsToUnits(pixelY),
            0f - z,
            rotation));
    }
}
