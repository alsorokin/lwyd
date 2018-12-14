using System.Collections.Generic;
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

    public Tile CreateTileFromResourse(string resourcePath, float x, float y, float scale, bool passable)
    {
        GameObject prefab;
        if (prefabs.ContainsKey(resourcePath))
        {
            prefab = prefabs[resourcePath];
        }
        else
        {
            prefab = Resources.Load<GameObject>(resourcePath);
            prefabs.Add(resourcePath, prefab);
        }

        var result = new Tile(prefab, new Vector3(x, y, 2), passable);
        var localScale = result.gameObject.transform.localScale;
        result.gameObject.transform.localScale = new Vector3(localScale.x * scale, localScale.y * scale, localScale.z);
        return result;
    }
}
