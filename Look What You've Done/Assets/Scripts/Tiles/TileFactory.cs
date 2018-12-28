using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;
using System.Linq;

class TileFactory
{
    private static TileFactory instance;
    private Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    private Dictionary<int, Tile> tiles = new Dictionary<int, Tile>();

    private TileFactory() {
        ReadTiles();
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

    public Tile CreateTile(int id, float x, float y, float scale)
    {
        if (!tiles.ContainsKey(id))
        {
            throw new Exception("Cannot create tile " + id.ToString() + ". I don't know of such an id.");
        }

        var newTile = tiles[id].Clone();
        newTile.gameObject.transform.position = new Vector3(x, y);
        newTile.gameObject.transform.localScale = new Vector3(scale, scale, 1);
        newTile.gameObject.SetActive(true);

        return newTile;
    }

    private void ReadTiles()
    {
        var fileContents = File.ReadAllText("Assets\\Resources\\test.tsx");

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Async = false;

        var doc = new XmlDocument();
        doc.LoadXml(fileContents);
        var docTiles = doc.GetElementsByTagName("tile");
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
                                var cType = bNode.ChildNodes.Count > 0 ?
                                    bNode.ChildNodes.Item(0).Name == "ellipse" ?
                                        ColliderType.Ellipse
                                        : ColliderType.Box
                                    : ColliderType.None;

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
                throw new XmlException("All tiles should have id");
            }

            if (tiles.ContainsKey(id))
            {
                Debug.LogWarning("Duplicated tile? " + id.ToString());
                continue;
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
                var texture = Resources.Load(source) as Texture2D;
                texture.filterMode = FilterMode.Point;
                var pivotPoint = 0.5f;
                sprite = Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(pivotPoint, pivotPoint), width);
                sprites.Add(source, sprite);
            }

            var tile = new Tile(sprite, id, Vector3.zero, 1f);
            tile.gameObject.SetActive(false);
            tiles.Add(id, tile);
        }
    }
}
