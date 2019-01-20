using UnityEngine;
using System.Linq;

public class Tile
{
    public GameObject gameObject;
    // TODO: Add support for multiple colliders
    private TileCollider tileCollider;
    private float offset;
    private readonly float originalZ;

    private float OrderInLayer
    {
        get
        {
            return (this.gameObject.transform.position.y + offset) * 0.001f;
        }
    }

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return gameObject.GetComponent<SpriteRenderer>();
        }
    }

    public int Id { get; }
    public uint Gid { get; set; }

    public bool IsFlippedHorizontally { get; private set; }

    public bool IsFlippedVertically { get; private set; }

    public bool IsFlippedDiagonally { get; private set; }

    public Tile(Sprite sprite, int id, uint gid, Vector3 position, float scale, TileCollider tc) : this (sprite, id, gid, position, scale)
    {
        SetCollider(tc);
    }

    public Tile(Sprite sprite, int id, uint gid, Vector3 position, float scale)
    {
        this.Id = id;
        this.Gid = gid;
        this.gameObject = new GameObject("tile-" + id.ToString());
        this.gameObject.transform.localScale = new Vector3(scale, scale, 1f);
        this.gameObject.transform.position = position;
        var renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        this.offset = -(this.SpriteRenderer.bounds.size.y / 2);
        this.originalZ = position.z;
        UpdateZPosition();
    }

    public void SetCollider(TileCollider collider)
    {
        if (collider == null)
        {
            return;
        }
        else if (collider is BoxTileCollider)
        {
            var boxCollider = gameObject.GetComponent<BoxCollider2D>();
            GameObject.Destroy(boxCollider);
        }
        else if (collider is CircleTileCollider)
        {
            var circleCollider = gameObject.GetComponent<CircleCollider2D>();
            GameObject.Destroy(circleCollider);
        }
        else if (collider is PolygonTileCollider)
        {
            var polygonCollider = gameObject.GetComponent<PolygonCollider2D>();
            GameObject.Destroy(polygonCollider);
        }
        else
        {
            // unknown collider type
            Debug.LogWarning("Unknown collider type: " + collider.GetType().Name);
            return;
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
        float ppu = this.SpriteRenderer.sprite.pixelsPerUnit;

        if (collider.GetType() == typeof(BoxTileCollider))
        {
            var boxCollider = collider as BoxTileCollider;
            var newBoxCollider = gameObject.AddComponent<BoxCollider2D>();
            newBoxCollider.size = new Vector2(
                boxCollider.bounds.width / ppu,
                boxCollider.bounds.height / ppu);
            newBoxCollider.offset = new Vector2(
                ((boxCollider.bounds.width / 2) - tileWidthPixelsHalf + boxCollider.bounds.x) / ppu,
                -((boxCollider.bounds.height / 2) - tileWidthPixelsHalf + boxCollider.bounds.y) / ppu);
            this.offset = newBoxCollider.offset.y - (this.SpriteRenderer.sprite.texture.height / 2 / ppu);
        }
        else if (collider.GetType() == typeof(CircleTileCollider))
        {
            var circleCollider = collider as CircleTileCollider;
            var newCircleCollider = gameObject.AddComponent<CircleCollider2D>();
            newCircleCollider.radius = circleCollider.Radius / ppu;
            newCircleCollider.offset = new Vector2(
                ((circleCollider.bounds.width / 2) - tileWidthPixelsHalf + circleCollider.bounds.x) / ppu,
                -((circleCollider.bounds.height / 2) - tileWidthPixelsHalf + circleCollider.bounds.y) / ppu);
            this.offset = newCircleCollider.offset.y - (this.SpriteRenderer.sprite.texture.height / 2 / ppu);
        }
        else if (collider.GetType() == typeof(PolygonTileCollider))
        {
            var polygonCollider = collider as PolygonTileCollider;
            var newPolygonCollider = gameObject.AddComponent<PolygonCollider2D>();
            newPolygonCollider.points = polygonCollider.Vertices.ToArray();
        }


        UpdateZPosition();
    }

    public void SetFlipped(bool h, bool v, bool d)
    {
        IsFlippedHorizontally = h;
        IsFlippedVertically = v;
        IsFlippedDiagonally = d;

        // 000 - std - 5
        if (!h && !v && !d)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        // 001 - fv90 - 536870917
        else if (!h && !v && d)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(180, 0, 90);
        }
        // 010 - fv - 1073741829
        else if (!h && v && !d)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(180, 0, 0);
        }
        // 100 - fh - 2147483653
        else if (h && !v && !d)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        // 011 - std90 - 1610612741
        else if (!h && v && d)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        // 101 - std270 - 2684354565
        else if (h && !v && d)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
        // 110 - std180 - 3221225477
        else if (h && v && !d)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        // 111 - fh90 - 3758096389
        else if (h && v && d)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(0, 180, 90);
        }
        // wtf?
        else
        {
            throw new System.Exception("Looks like I didn't handle every possible variant of tile flipping. Please contact me and tell me I'm dumb.");
        }
    }

    public Tile CloneTo(float x, float y, float z)
    {
        var newPosition = new Vector3(x, y, z);
        var newTile = new Tile(SpriteRenderer.sprite, Id, Gid, newPosition, this.gameObject.transform.localScale.x);
        newTile.SetCollider(this.tileCollider);
        
        return newTile;
    }

    private void UpdateZPosition()
    {
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
                                                         this.gameObject.transform.position.y,
                                                         originalZ + OrderInLayer);
    }
}
