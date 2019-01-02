using UnityEngine;

public enum ColliderType
{
    None, Box, Circle
}

public struct TileCollider
{
    public ColliderType type;
    public Rect bounds;

    public static TileCollider Zero
    {
        get
        {
            return new TileCollider()
            {
                type = ColliderType.None,
                bounds = new Rect(0f, 0f, 0f, 0f)
            };
        }
    }
}
