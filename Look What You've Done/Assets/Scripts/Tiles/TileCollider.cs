using System.Collections.Generic;
using UnityEngine;

public abstract class TileCollider
{
    
}

public class PolygonTileCollider : TileCollider
{
    public List<Vector2> Vertices = new List<Vector2>();
}

public class CircleTileCollider : TileCollider
{
    public Rect bounds;
    public float Radius
    {
        get
        {
            return bounds.width / 2;
        }
    }
}

public class BoxTileCollider : TileCollider
{
    public Rect bounds;
}
