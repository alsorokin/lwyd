using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public GameObject grassPrefab;
    public Texture2D levelTiles;
    public Level currentLevel
    {
        get
        {
            return _currentLevel;
        }
    }

    private Level _currentLevel;

    // Use this for initialization
    void Start()
    {
        _currentLevel = new Level(23, 11, levelTiles);
	}

    // Update is called once per frame
    void Update() { }
}
