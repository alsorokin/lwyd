using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    static int levelWidth = 1000;
    static int levelHeight = 1000;

    public GameObject grassPrefab;

    GameObject[,] levelTiles = new GameObject[levelWidth, levelHeight];

	// Use this for initialization
	void Start () {
        Quaternion zeroRotation = new Quaternion();
        float tileWidth = grassPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float tileHeight = grassPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        float offsetX = levelWidth / 2;
        float offsetY = levelHeight / 2;
        for (int w = 0; w < levelWidth; w++)
        {
            for (int h = 0; h < levelHeight; h++)
            {
                float x = (w - offsetX);
                float y = (h - offsetY);
                levelTiles[h, w] = Instantiate(grassPrefab, new Vector3(x, y, 2), zeroRotation);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
