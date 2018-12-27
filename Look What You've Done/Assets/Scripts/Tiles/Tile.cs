using UnityEngine;

public class Tile
{
    public GameObject gameObject;

    public SpriteRenderer spriteRenderer
    {
        get
        {
            return gameObject.GetComponent<SpriteRenderer>();
        }
    }

    public int id { get; }

    public Tile(Sprite sprite, int id, Vector3 position)
    {
        this.id = id;
        gameObject = new GameObject("tile-" + id.ToString());
        gameObject.transform.position = position;
        var renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
    }

    public Tile Clone()
    {
        return new Tile(spriteRenderer.sprite, id, this.gameObject.transform.position);
    }
}
