using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Tile
{
    public bool passable;
    public GameObject gameObject;

    private static readonly Quaternion zeroRotation = new Quaternion();

    public Tile(GameObject prefab, Vector3 position, bool passable)
    {
        this.passable = passable;
        gameObject = UnityEngine.Object.Instantiate(prefab, position, zeroRotation);
    }
}
