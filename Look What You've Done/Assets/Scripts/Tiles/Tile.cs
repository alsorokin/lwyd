using UnityEngine;

public class Tile
{
    public GameObject gameObject;
    private TileCollider tileCollider = TileCollider.Zero;

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return gameObject.GetComponent<SpriteRenderer>();
        }
    }

    public int id { get; }

    public Tile(Sprite sprite, int id, Vector3 position, float scale, TileCollider tc) : this (sprite, id, position, scale)
    {
        SetCollider(tc);
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

    public void SetCollider(TileCollider collider)
    {
        if (this.tileCollider.type == ColliderType.None)
        {
            switch (this.tileCollider.type)
            {
                case ColliderType.Box:
                    var boxCollider = gameObject.GetComponent<BoxCollider2D>();
                    GameObject.Destroy(boxCollider);
                    break;
                case ColliderType.Circle:
                    var circleCollider = gameObject.GetComponent<CircleCollider2D>();
                    GameObject.Destroy(circleCollider);
                    break;
            }
        }

        var rigidbody = gameObject.GetComponent<Rigidbody2D>();
        if (rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody2D>();
        }

        rigidbody.bodyType = RigidbodyType2D.Static;
        rigidbody.simulated = true;

        this.tileCollider = collider;
        var tileWidthPixels = this.SpriteRenderer.sprite.pixelsPerUnit;
        var tileWidthPixelsHalf = tileWidthPixels / 2;
        var tileHeightPixels = this.SpriteRenderer.sprite.pixelsPerUnit;
        switch (collider.type)
        {
            case ColliderType.Box:
                var newBoxCollider = gameObject.AddComponent<BoxCollider2D>();
                newBoxCollider.size = new Vector2(
                    collider.bounds.width / this.SpriteRenderer.sprite.pixelsPerUnit,
                    collider.bounds.height / this.SpriteRenderer.sprite.pixelsPerUnit);
                newBoxCollider.offset = new Vector2(
                    ((collider.bounds.width / 2) - tileWidthPixelsHalf + collider.bounds.x) / this.SpriteRenderer.sprite.pixelsPerUnit,
                    -((collider.bounds.height / 2) - tileWidthPixelsHalf + collider.bounds.y) / this.SpriteRenderer.sprite.pixelsPerUnit);
                break;
            case ColliderType.Circle:
                var newCircleCollider = gameObject.AddComponent<CircleCollider2D>();
                newCircleCollider.radius = collider.bounds.width / this.SpriteRenderer.sprite.pixelsPerUnit;
                newCircleCollider.offset = new Vector2(
                    ((collider.bounds.width / 2) - tileWidthPixelsHalf + collider.bounds.x) / this.SpriteRenderer.sprite.pixelsPerUnit,
                    -((collider.bounds.height / 2) - tileWidthPixelsHalf + collider.bounds.y) / this.SpriteRenderer.sprite.pixelsPerUnit);
                break;
        }
    }

    public Tile Clone()
    {
        var newTile = new Tile(SpriteRenderer.sprite, id, this.gameObject.transform.position, this.gameObject.transform.localScale.x);
        newTile.SetCollider(this.tileCollider);

        return newTile;
    }
}
