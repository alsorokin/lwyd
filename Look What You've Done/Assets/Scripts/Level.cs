using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using Direction = MovementController.Direction;

public class Level
{
    private int levelWidth;
    private int levelHeight;
    private List<Actor> actors = new List<Actor>();
    private Tile[,,] levelTiles;
    public readonly float TileScale = 1f;

    public Tile GetLeftmostTile()
    {
        return levelTiles[0, 0, 0];
    }

    public Tile GetRightmostTile()
    {
        return levelTiles[levelWidth - 1, levelHeight - 1, 0];
    }

    public Tile GetTopmostTile()
    {
        return GetRightmostTile();
    }

    public Tile GetBottommostTile()
    {
        return GetLeftmostTile();
    }

    public Level()
    {
        LoadFromFile("Assets\\Levels\\test.tmx");

        // Adding player
        GameObject player = GameObject.Instantiate(Resources.Load<GameObject>("Tiles/Player"));
        player.transform.position = new Vector3(TranslateGridToX(levelWidth / 2), TranslateGridToY(levelHeight / 2), -10f);
        var playerLocalScale = player.transform.localScale;
        player.transform.localScale = new Vector3(playerLocalScale.x * TileScale, playerLocalScale.y * TileScale, playerLocalScale.z);
        Actor playerActor = player.GetComponent<Hero>();
        playerActor.SetLevel(this);
        playerActor.Cloneable = false;
        AddActor(playerActor);

        // Adding camera and its controller
        var cameraObj = new GameObject();
        var playerCamera = cameraObj.AddComponent<Camera>();
        cameraObj.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -100f);
        playerCamera.orthographic = true;
        cameraObj.AddComponent<CameraMovementController>();
        var cameraController = cameraObj.GetComponent<CameraMovementController>();
        cameraController.player = player;
        cameraController.level = this;
    }

    private void LoadFromFile(string fileName)
    {
        // parsing xml
        string content = File.ReadAllText(fileName);
        XmlReaderSettings settings = new XmlReaderSettings { Async = false };
        var doc = new XmlDocument();
        doc.LoadXml(content);

        // getting the main node and its attributes
        XmlNodeList mapNodes = doc.GetElementsByTagName("map");
        if (mapNodes.Count == 0)
        {
            throw new Exception("Can\'t see the map node in level file " + fileName);
        }

        XmlNode mapNode = mapNodes[0];
        if (!int.TryParse(mapNode.Attributes["width"].Value, out levelWidth))
        {
            levelWidth = 100;
        }

        if (!int.TryParse(mapNode.Attributes["height"].Value, out levelHeight))
        {
            levelHeight = 100;
        }

        // getting layers
        IEnumerable<XmlNode> layerNodes = doc.GetElementsByTagName("layer").Cast<XmlNode>();
        int layerCount = layerNodes.Count();
        if (layerCount == 0)
        {
            throw new Exception("Can\'t see any layers in level file " + fileName);
        }

        levelTiles = new Tile[levelWidth, levelHeight, layerCount];

        // getting data nodes
        XmlNodeList dataNodes = doc.GetElementsByTagName("data");
        if (dataNodes.Count == 0)
        {
            throw new Exception("Can\'t see the data node in level file " + fileName);
        }

        // reading layers from data nodes
        for (var z = 0; z < dataNodes.Count; z++)
        {
            XmlNode dataNode = dataNodes[z];
            string encoding = dataNode.Attributes["encoding"].Value;
            if (encoding != "csv")
            {
                throw new Exception("Unknown level data encoding: " + encoding);
            }

            var data = dataNode.InnerText.Trim();
            ParseCsvData(data, levelWidth, levelHeight, out uint[,] dataArray);
            for (int i = 0; i < levelWidth; i++)
            {
                for (int j = 0; j < levelHeight; j++)
                {
                    CreateTile(dataArray[i, j], i, levelHeight - j - 1, z);
                }
            }
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

    public GameObject SpawnGenericEnemyAt(int gridX, int gridY, bool isFree)
    {
        var genericEnemy = GameObject.Instantiate(Resources.Load<GameObject>(isFree ? "Tiles/GenericFreeEnemyTile" : "Tiles/GenericGridEnemyTile"));
        genericEnemy.transform.position = new Vector3(TranslateGridToX(gridX), TranslateGridToY(gridY), 1f);
        var localScale = genericEnemy.transform.localScale;
        genericEnemy.transform.localScale = new Vector3(localScale.x * TileScale, localScale.y * TileScale, localScale.z);
        var ge = genericEnemy.GetComponent<GenericEnemy>();
        ge.SetLevel(this);
        ge.Cloneable = true;
        ge.Health = ge.MaxHealth;
        AddActor(ge);

        return genericEnemy;
    }

    public int TranslateXToGrid(float x)
    {
        return (int)Math.Round(x / TileScale);
    }

    public int TranslateYToGrid(float y)
    {
        return (int)Math.Round(y / TileScale);
    }

    public float TranslateGridToX(int gridX)
    {
        return gridX * TileScale;
    }

    public float TranslateGridToY(int gridY)
    {
        return gridY * TileScale;
    }

    public void AddActor(Actor actor)
    {
        if (!actors.Contains(actor))
        {
            actors.Add(actor);
        }
    }

    private void CreateTile(uint id, int w, int h, int z)
    {
        levelTiles[w, h, z] = TileFactory.Instance.CreateTile(id, TranslateGridToX(w), TranslateGridToY(h), TileScale);
    }
}
