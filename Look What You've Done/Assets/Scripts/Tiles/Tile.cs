using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Tile
{
    public bool passable;

    private GameObject instance;
    private GameObject prefab;
    private Vector3 position;

    private static readonly Quaternion zeroRotation = new Quaternion();

    public Tile(GameObject prefab, Vector3 position, bool passable)
    {
        this.position = position;
        this.prefab = prefab;
        this.passable = passable;
        instance = UnityEngine.Object.Instantiate(prefab, position, zeroRotation);
    }
}
