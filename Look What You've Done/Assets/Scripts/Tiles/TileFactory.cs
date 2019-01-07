﻿using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using System.Linq;
using UnityEditor;

class TileFactory
{
    private static TileFactory instance;
    private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    private Dictionary<int, Tile> tiles = new Dictionary<int, Tile>();

    private TileFactory() {
        int nextGid = 1;
        nextGid = ReadTiles("Assets\\Resources\\test.tsx", nextGid);
        nextGid = ReadTiles("Assets\\Resources\\tree-sample.tsx", nextGid);
    }

    public static TileFactory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TileFactory();
            }

            return instance;
        }
    }

    public Tile CreateTile(int gid, float x, float y, float scale)
    {
        if (gid == 0)
        {
            return CreateEmptyTile();
        }

        if (!tiles.ContainsKey(gid))
        {
            throw new Exception("Cannot create tile " + gid.ToString() + ". I don't know of such a gid.");
        }

        var newTile = tiles[gid].Clone();
        newTile.gameObject.transform.position = new Vector3(x, y);
        newTile.gameObject.transform.localScale = new Vector3(scale, scale, 1);
        newTile.gameObject.SetActive(true);

        return newTile;
    }

    private Tile CreateEmptyTile()
    {
        var emptyTile = new Tile(Sprite.Create(new Texture2D(0, 0), new Rect(0, 0, 0, 0), Vector2.zero), 0, 0, new Vector3(0f, 0f, 0f), 0f);
        emptyTile.gameObject.transform.position = Vector3.zero;
        emptyTile.gameObject.SetActive(true);

        return emptyTile;
    }

    private int ReadTiles(string tileMap, int firstGid)
    {
        var fileContents = File.ReadAllText(tileMap);
        var gid = firstGid;

        XmlReaderSettings settings = new XmlReaderSettings { Async = false };
        var doc = new XmlDocument();
        doc.LoadXml(fileContents);
        XmlNodeList tileNodes = doc.GetElementsByTagName("tile");
        XmlNodeList imageNodes = doc.GetElementsByTagName("image");

        if (tileNodes.Count == 0 || (imageNodes.Count > 0 && imageNodes[0].ParentNode.Name == "tile"))
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

    private int LoadTilesFromSingleNode(XmlNode imageNode, int firstGid)
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

        if (sprites.ContainsKey(source + ":0"))
        {
            Debug.LogWarning("Sprites already loaded: " + source);
        }

        var texture = Resources.Load<Texture2D>(source);
        string spriteSheet = AssetDatabase.GetAssetPath(texture);
        Sprite[] loadedSprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToArray();
        for (var i = 0; i < loadedSprites.Length; i++)
        {
            sprites.Add(source + ":" + i.ToString(), loadedSprites[i]);
            var tile = new Tile(loadedSprites[i], -1, gid++, Vector3.zero, 1f);
            tiles.Add(tile.Gid, tile);
        }

        return gid;
    }

    private int LoadTilesFromTileNodes(XmlNodeList docTiles, int firstGid)
    {
        if (docTiles == null || docTiles.Cast<XmlNode>().Count() == 0)
        {
            return firstGid;
        }

        var gid = firstGid;
        foreach (XmlNode docTile in docTiles)
        {
            XmlNode docImage = null;
            var collider = TileCollider.Zero;
            foreach (XmlNode aNode in docTile.ChildNodes)
            {
                switch (aNode.Name)
                {
                    case "image":
                        docImage = aNode;
                        break;
                    case "objectgroup":
                        foreach (XmlNode bNode in aNode.ChildNodes)
                        {
                            if (bNode.Name == "object" && bNode.Attributes["id"].Value == "1")
                            {
                                var bChildren = bNode.ChildNodes.Cast<XmlNode>();
                                collider.type = bChildren.Count() > 0 && bChildren.Any(bn => bn.Name == "ellipse") ?
                                    ColliderType.Circle : ColliderType.Box;
                                float.TryParse(bNode.Attributes["x"].Value, out float cx);
                                float.TryParse(bNode.Attributes["y"].Value, out float cy);
                                float.TryParse(bNode.Attributes["width"].Value, out float cWidth);
                                float.TryParse(bNode.Attributes["height"].Value, out float cHeight);
                                collider.bounds = new Rect(cx, cy, cWidth, cHeight);
                            }
                        }

                        break;
                }

            }

            if (docImage == null)
            {
                throw new XmlException("Tile should have an image element");
            }

            int id = -1;
            if (!int.TryParse(docTile.Attributes["id"].Value, out id))
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

            Sprite sprite;
            if (sprites.ContainsKey(source))
            {
                sprite = sprites[source];
            }
            else
            {
                sprite = Resources.Load<Sprite>(source);
                sprites.Add(source, sprite);
            }

            
            var tile = new Tile(sprite, id, id + firstGid, Vector3.zero, 1f);
            if (collider.type != ColliderType.None)
            {
                tile.SetCollider(collider);
            }

            tile.gameObject.SetActive(false);
            tiles.Add(tile.Gid, tile);
        }

        return tiles.Last().Value.Gid + 1;
    }
}
