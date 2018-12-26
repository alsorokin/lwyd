using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System;

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
        throw new NotImplementedException();
    }

    private void ReadTiles()
    {
        var fileStream = new FileStream("Assets\\Resources\\Tiles\\test.tsx", FileMode.Open);
        var smth = XmlReader.Create(fileStream);

        XmlReaderSettings settings = new XmlReaderSettings();
        settings.Async = false;

        using (XmlReader reader = XmlReader.Create(fileStream, settings))
        {
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        Console.WriteLine("Start Element {0}", reader.Name);
                        if (reader.Name == "tile")
                        {
                            Console.WriteLine("Its id is {0}", reader.GetAttribute("id"));
                        }
                        break;
                    case XmlNodeType.Text:
                        Console.WriteLine("Text Node: {0}", reader.Value);
                        break;
                    case XmlNodeType.EndElement:
                        Console.WriteLine("End Element {0}", reader.Name);
                        break;
                    default:
                        Console.WriteLine("Other node {0} with value {1}",
                                        reader.NodeType, reader.Value);
                        break;
                }
            }
        }
    }
}
