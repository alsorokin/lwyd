﻿using UnityEngine;

public class Game : MonoBehaviour {
    public GameObject grassPrefab;
    public Texture2D levelTiles;

    public Level CurrentLevel { get; private set; }

    // Use this for initialization
    void Start()
    {
        CurrentLevel = new Level(19, 19, levelTiles);
	}

    // Update is called once per frame
    void Update() { }
}
