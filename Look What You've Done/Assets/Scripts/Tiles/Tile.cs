﻿using UnityEngine;

public class Tile
{
    public bool passable;
    public GameObject gameObject;

    private static readonly Quaternion zeroRotation = new Quaternion();

    public Tile(GameObject prefab, Vector3 position, bool passable)
    {
        this.passable = passable;
        gameObject = Object.Instantiate(prefab, position, zeroRotation);
    }
}
