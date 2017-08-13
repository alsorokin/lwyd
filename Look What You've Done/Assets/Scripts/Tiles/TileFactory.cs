using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class TileFactory
{
    private static TileFactory instance;
    private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    private TileFactory() { }
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

    public Tile CreateTileFromResourse(string resourcePath, float x, float y, bool passable)
    {
        GameObject prefab;
        if (prefabs.ContainsKey(resourcePath))
        {
            prefab = prefabs[resourcePath];
        } else
        {
            prefab = Resources.Load<GameObject>(resourcePath);
            prefabs.Add(resourcePath, prefab);
        }
        return new Tile(prefab, new Vector3(x, y, 2), passable);
    }
}
