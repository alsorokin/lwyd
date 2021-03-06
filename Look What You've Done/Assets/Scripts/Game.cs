﻿using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject GrassPrefab;
    public Texture2D LevelTiles;

    public Level CurrentLevel { get; private set; }

    // Use this for initialization
    void Start() => CurrentLevel = new Level();

    // Update is called once per frame
    void Update() { }

    void FixedUpdate()
    {
        if (CurrentLevel.Scale < 2.5f)
        {
            CurrentLevel.Scale *= 1.01f;
        }
    }
}
