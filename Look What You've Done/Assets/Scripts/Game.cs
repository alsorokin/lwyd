using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    static int levelWidth = 10;
    static int levelHeight = 10;

    public GameObject grassPrefab;

    Tile[,] levelTiles = new Tile[levelWidth, levelHeight];

	// Use this for initialization
	void Start () {
        float offsetX = -0.5f;
        float offsetY = -0.5f;
        for (int w = 0; w < levelWidth; w++)
        {
            for (int h = 0; h < levelHeight; h++)
            {
                float x = (w - offsetX);
                float y = (h - offsetY);
                levelTiles[h, w] = new Tile(grassPrefab, new Vector3(x, y, 2), true);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
