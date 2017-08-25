using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Tile
{
    public bool passable;

    private static readonly Quaternion zeroRotation = new Quaternion();

    public Tile(GameObject prefab, Vector3 position, bool passable)
    {
        this.passable = passable;
        UnityEngine.Object.Instantiate(prefab, position, zeroRotation);
    }
}
