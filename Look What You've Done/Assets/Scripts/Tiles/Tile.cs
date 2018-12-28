using UnityEngine;

public class Tile
{
    public GameObject gameObject;
    public TileCollider tileCollider = TileCollider.Zero;

    public SpriteRenderer spriteRenderer
    {
        get
        {
            return gameObject.GetComponent<SpriteRenderer>();
        }
    }

    public int id { get; }

    public Tile(Sprite sprite, int id, Vector3 position, float scale, TileCollider tc) : this (sprite, id, position, scale)
    {
        tileCollider = tc;
        switch(tc.type)
        {
            case ColliderType.Box:
                var box = gameObject.AddComponent<BoxCollider2D>();
                // relative?
                box.transform.position = new Vector3(tc.bounds.position.x, tc.bounds.position.y, position.z);
                break;
        }
    }

    public Tile(Sprite sprite, int id, Vector3 position, float scale)
    {
        this.id = id;
        gameObject = new GameObject("tile-" + id.ToString());
        gameObject.transform.localScale = new Vector3(scale, scale, 1f);
        gameObject.transform.position = position;
        var renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }

    public Tile Clone()
    {
        return new Tile(spriteRenderer.sprite, id, this.gameObject.transform.position, this.gameObject.transform.localScale.x);
    }
}
