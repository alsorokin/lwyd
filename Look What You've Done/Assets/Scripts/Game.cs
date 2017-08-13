using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public GameObject grassPrefab;
    public Texture2D levelTiles;

	// Use this for initialization
	void Start () {
        Level mainLevel = new Level(7, 7, levelTiles);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
