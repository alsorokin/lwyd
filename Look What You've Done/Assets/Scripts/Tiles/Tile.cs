using UnityEngine;

public class Tile
{
    public bool passable;
    public GameObject gameObject;
    public int id { get; }

    private static readonly Quaternion zeroRotation = new Quaternion();

    public Tile(Sprite sprite, int id, Vector3 position, bool passable)
    {
        this.id = id;
        this.passable = passable;
        gameObject = new GameObject("tile-" + id.ToString());
        gameObject.transform.position = position;
        var renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        
        //gameObject = Object.Instantiate(prefab, position, zeroRotation);
    }
}
