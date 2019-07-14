using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using System.Linq;
using UnityEditor;

class TileFactory
{
    private static TileFactory _instance;
    private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();
    private Dictionary<uint, Tile> _tiles = new Dictionary<uint, Tile>();

    private TileFactory() {
        uint nextGid = 1;
        nextGid = ReadTiles("Assets\\Resources\\test.tsx", nextGid);
        nextGid = ReadTiles("Assets\\Resources\\tree-sample.tsx", nextGid);
        nextGid = ReadTiles("Assets\\Resources\\sprite-pack-sample.tsx", nextGid);
    }

    public static TileFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TileFactory();
            }

            return _instance;
        }
    }

    public Tile CreateTile(uint globalId, float x, float y, float z, int rotation = 0)
    {
        var gid = globalId;
        var flippedHorizontally = GetBit(gid, 32);
        gid = ZeroBit(gid, 32);
        var flippedVertically = GetBit(gid, 31);
        gid = ZeroBit(gid, 31);
        var flippedDiagonally = GetBit(gid, 30);
        gid = ZeroBit(gid, 30);

        if (gid == 0)
        {
            return CreateEmptyTile();
        }

        if (!_tiles.ContainsKey(gid))
        {
            Debug.LogWarning("Cannot create tile " + gid.ToString() + ". I don't know of such a gid.");
            return CreateEmptyTile();
        }

        var sprite = _tiles[gid].SpriteRenderer.sprite;
        var normalizedX = x + (sprite.pivot.x / sprite.pixelsPerUnit);
        var normalizedY = y + (sprite.pivot.y / sprite.pixelsPerUnit);
        var newTile = _tiles[gid].CloneTo(normalizedX, normalizedY, z);
        newTile.SetFlipped(flippedHorizontally, flippedVertically, flippedDiagonally, rotation);

        newTile.GameObject.SetActive(true);

        return newTile;
    }

    private Tile CreateEmptyTile()
    {
        var emptyTile = new Tile(Sprite.Create(new Texture2D(0, 0), new Rect(0, 0, 0, 0), Vector2.zero), 0, 0, new Vector3(0f, 0f, 0f), 0f);
        emptyTile.GameObject.transform.position = Vector3.zero;
        emptyTile.GameObject.SetActive(true);

        return emptyTile;
    }

    private uint ReadTiles(string tileMap, uint firstGid)
    {
        var fileContents = File.ReadAllText(tileMap);
        uint gid = firstGid;

        var doc = new XmlDocument();
        doc.LoadXml(fileContents);
        XmlNodeList tileNodes = doc.GetElementsByTagName("tile");
        XmlNodeList imageNodes = doc.GetElementsByTagName("image");

        if (imageNodes.Count > 0 && imageNodes[0].ParentNode.Name == "tile")
        {
            // Individual tile images
            gid = LoadTilesFromTileNodes(tileNodes, gid);
        }
        else
        {
            // Single tileset image
            gid = LoadTilesFromSingleNode(imageNodes[0], gid);
        }

        return gid;
    }

    private uint LoadTilesFromSingleNode(XmlNode imageNode, uint firstGid)
    {
        var gid = firstGid;
        int id = 1;

        string source = imageNode.Attributes["source"].Value;
        source = source.Substring(0, source.LastIndexOf('.'));
        if (source.StartsWith("./"))
        {
            source = source.Substring(2);
        }

        if (string.IsNullOrEmpty(source))
        {
            Debug.LogWarning("Source is empty for tile " + id.ToString());
        }

        if (_sprites.ContainsKey(source + ":0"))
        {
            Debug.LogWarning("Sprites already loaded: " + source);
        }

        var texture = Resources.Load<Texture2D>(source);
        string spriteSheet = AssetDatabase.GetAssetPath(texture);
        Sprite[] loadedSprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToArray();
        IEnumerable<XmlNode> tileNodes = imageNode.OwnerDocument.GetElementsByTagName("tile").Cast<XmlNode>().ToArray();

        for (var i = 0; i < loadedSprites.Length; i++)
        {
            _sprites.Add(source + ":" + i.ToString(), loadedSprites[i]);

            XmlNode tileNode = tileNodes.FirstOrDefault(n => n.Attributes["id"].Value == i.ToString());
            TileCollider collider = null;
            if (tileNode != null)
            {
                // TODO: Parse multiple colliders
                XmlNode objectGroupNode = tileNode.ChildNodes[0];
                XmlNode objectNode = objectGroupNode.ChildNodes[0];
                collider = ParseObjectNode(objectNode);
            }

            var tile = new Tile(loadedSprites[i], -1, gid++, Vector3.zero, 1f, collider);
            tile.GameObject.SetActive(false);
            _tiles.Add(tile.Gid, tile);
        }

        return gid;
    }

    private uint LoadTilesFromTileNodes(XmlNodeList tileNodes, uint firstGid)
    {
        if (tileNodes == null || tileNodes.Cast<XmlNode>().Count() == 0)
        {
            return firstGid;
        }

        foreach (XmlNode tileNode in tileNodes)
        {
            XmlNode docImage = null;
            TileCollider collider = null;
            foreach (XmlNode aNode in tileNode.ChildNodes)
            {
                switch (aNode.Name)
                {
                    case "image":
                        docImage = aNode;
                        break;
                    case "objectgroup":
                        foreach (XmlNode bNode in aNode.ChildNodes)
                        {
                            // TODO: Add support for multiple colliders
                            if (bNode.Name == "object" && bNode.Attributes["id"].Value == "1")
                            {
                                collider = ParseObjectNode(bNode);
                            }
                        }

                        break;
                }
            }

            if (docImage == null)
            {
                throw new XmlException("Tile should have an image element");
            }

            if (!int.TryParse(tileNode.Attributes["id"].Value, out int id))
            {
                throw new XmlException("All single-file tiles should have an id");
            }

            string source = docImage.Attributes["source"].Value;
            source = source.Substring(0, source.LastIndexOf('.'));
            if (source.StartsWith("./"))
            {
                source = source.Substring(2);
            }

            if (string.IsNullOrEmpty(source))
            {
                Debug.LogWarning("Source is empty for tile " + id.ToString());
                continue;
            }

            /*
            int width = -1;
            if (!int.TryParse(docImage.Attributes["width"].Value, out width))
            {
                Debug.LogWarning("Tile image should have width! Id: " + id.ToString());
                continue;
            }

            int height = -1;
            if (!int.TryParse(docImage.Attributes["height"].Value, out height))
            {
                Debug.LogWarning("Tile image should have height! Id: " + id.ToString());
                continue;
            }
            */

            Sprite sprite;
            if (_sprites.ContainsKey(source))
            {
                sprite = _sprites[source];
            }
            else
            {
                sprite = Resources.Load<Sprite>(source);
                _sprites.Add(source, sprite);
            }

            
            var tile = new Tile(sprite, id, (uint)(id + firstGid), Vector3.zero, 1f);
            if (collider != null)
            {
                tile.SetCollider(collider);
            }

            tile.GameObject.SetActive(false);
            _tiles.Add(tile.Gid, tile);
        }

        return _tiles.Last().Value.Gid + 1;
    }

    private TileCollider ParseObjectNode(XmlNode objectNode)
    {
        if (objectNode == null)
        {
            throw new ArgumentNullException("objectNode");
        }

        var children = objectNode.ChildNodes.Cast<XmlNode>();
        float.TryParse(objectNode.Attributes["x"]?.Value, out float cx);
        float.TryParse(objectNode.Attributes["y"]?.Value, out float cy);

        if (children.Count() == 0)
        {
            float.TryParse(objectNode.Attributes["width"].Value, out float cWidth);
            float.TryParse(objectNode.Attributes["height"].Value, out float cHeight);

            var collider = new BoxTileCollider();
            collider.Bounds = new Rect(cx, cy, cWidth, cHeight);

            return collider;
        }
        else if (children.First().Name == "ellipse")
        {
            float.TryParse(objectNode.Attributes["width"].Value, out float cWidth);
            float.TryParse(objectNode.Attributes["height"].Value, out float cHeight);

            var collider = new CircleTileCollider();
            collider.Bounds = new Rect(cx, cy, cWidth, cHeight);

            return collider;
        }
        else if (children.First().Name == "polygon")
        {
            var collider = new PolygonTileCollider();
            IEnumerable<Vector2> points = children.First().Attributes["points"].Value.Split(' ')
                .Select(value => {
                    string[] pair = value.Split(',');
                    float.TryParse(pair[0], out float x);
                    float.TryParse(pair[1], out float y);

                    return new Vector2(x + cx, y + cy);
                });

            collider.Vertices = points.ToList();
            return collider;
        }

        return null;
    }

    private bool GetBit(uint value, int bitPosition)
    {
        return (value & (1 << bitPosition - 1)) != 0;
    }

    private uint ZeroBit(uint num, int bitPosition)
    {
        return num &= ~(1u << bitPosition - 1);
    }
}
