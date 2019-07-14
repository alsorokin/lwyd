using UnityEngine;
using System.Linq;

public class Tile
{
    public GameObject GameObject;
    // TODO: Add support for multiple colliders
    private TileCollider _tileCollider;
    private readonly float _originalZ;

    public float Offset { get; private set; }

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            return GameObject.GetComponent<SpriteRenderer>();
        }
    }

    public int Id { get; }
    public uint Gid { get; set; }

    public Tile(Sprite sprite, int id, uint gid, Vector3 position, float scale, TileCollider tc) : this (sprite, id, gid, position, scale)
    {
        SetCollider(tc);
    }

    public Tile(Sprite sprite, int id, uint gid, Vector3 position, float scale)
    {
        Id = id;
        Gid = gid;
        GameObject = new GameObject("tile-" + id.ToString());
        GameObject.transform.localScale = new Vector3(scale, scale, 1f);
        _originalZ = position.z;
        Vector3 normalizedPosition = new Vector3(position.x, position.y, 0f);
        GameObject.transform.position = normalizedPosition;
        var renderer = GameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        Offset = -(SpriteRenderer.sprite.pivot.y / SpriteRenderer.sprite.pixelsPerUnit);
        
        UpdateZPosition();
    }

    private int OrderInLayer => (int)((-GameObject.transform.position.y - Offset - _originalZ) * 100f);

    public void SetCollider(TileCollider collider)
    {
        var isBox = collider is BoxTileCollider;
        var isCircle = collider is CircleTileCollider;
        var isPolygon = collider is PolygonTileCollider;

        if (collider != null && !isBox && !isCircle && !isPolygon)
        {
            // unknown collider type
            Debug.LogWarning("Unknown collider type: " + collider.GetType().Name);
            return;
        }

        // removing any existing collider
        // TODO: support more than one collider
        var oldBoxCollider = GameObject.GetComponent<BoxCollider2D>();
        if (oldBoxCollider != null)
        {
            GameObject.Destroy(oldBoxCollider);
        }

        var oldCircleCollider = GameObject.GetComponent<CircleCollider2D>();
        if (oldCircleCollider != null)
        {
            GameObject.Destroy(oldCircleCollider);
        }

        var oldPolygonCollider = GameObject.GetComponent<PolygonCollider2D>();
        if (oldPolygonCollider != null)
        {
            GameObject.Destroy(oldPolygonCollider);
        }

        if (collider == null)
        {
            // at this point we have removed every collider
            return;
        }

        var rigidbody = GameObject.GetComponent<Rigidbody2D>();
        if (rigidbody == null)
        {
            rigidbody = GameObject.AddComponent<Rigidbody2D>();
        }

        rigidbody.bodyType = RigidbodyType2D.Static;
        rigidbody.simulated = true;

        _tileCollider = collider;
        // note that the tile is not always 1 unit, so this is not necessarily equal to ppu
        var tileWidthPixels = SpriteRenderer.sprite.texture.width;
        var tileWidthPixelsHalf = tileWidthPixels / 2;
        var tileHeightPixels = SpriteRenderer.sprite.texture.height;
        var tileHeightPixelsHalf = tileHeightPixels / 2;
        float ppu = SpriteRenderer.sprite.pixelsPerUnit;

        if (collider is BoxTileCollider)
        {
            var boxCollider = collider as BoxTileCollider;
            var newBoxCollider = GameObject.AddComponent<BoxCollider2D>();
            newBoxCollider.size = new Vector2(
                boxCollider.Bounds.width / ppu,
                boxCollider.Bounds.height / ppu);
            newBoxCollider.offset = new Vector2(
                ((boxCollider.Bounds.width / 2) - tileWidthPixelsHalf + boxCollider.Bounds.x) / ppu,
                -((boxCollider.Bounds.height / 2) - tileHeightPixelsHalf + boxCollider.Bounds.y) / ppu);
            Offset = newBoxCollider.offset.y - (SpriteRenderer.sprite.texture.height / 2 / ppu);
        }
        else if (collider is CircleTileCollider)
        {
            var circleCollider = collider as CircleTileCollider;
            var newCircleCollider = GameObject.AddComponent<CircleCollider2D>();
            newCircleCollider.radius = circleCollider.Radius / ppu;
            newCircleCollider.offset = new Vector2(
                ((circleCollider.Bounds.width / 2) - tileWidthPixelsHalf + circleCollider.Bounds.x) / ppu,
                -((circleCollider.Bounds.height / 2) - tileHeightPixelsHalf + circleCollider.Bounds.y) / ppu);
            Offset = newCircleCollider.offset.y - (SpriteRenderer.sprite.texture.height / 2 / ppu);
        }
        else //if (collider is PolygonTileCollider)
        {
            var polygonCollider = collider as PolygonTileCollider;
            var newPolygonCollider = GameObject.AddComponent<PolygonCollider2D>();
            newPolygonCollider.points = polygonCollider.Vertices.Select(v => new Vector2(v.x / ppu, -v.y / ppu)).ToArray();
            newPolygonCollider.offset = new Vector2(-tileWidthPixelsHalf / ppu, tileHeightPixelsHalf / ppu);
            Offset = newPolygonCollider.offset.y - (SpriteRenderer.sprite.texture.height / 2 / ppu);
        }

        UpdateZPosition();
    }

    public void SetFlipped(bool h, bool v, bool d, int rotation = 0)
    {
        // 000 - std - 5
        if (!h && !v && !d)
        {
            GameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        // 001 - fv90 - 536870917
        else if (!h && !v && d)
        {
            GameObject.transform.localRotation = Quaternion.Euler(180, 0, 90);
        }
        // 010 - fv - 1073741829
        else if (!h && v && !d)
        {
            GameObject.transform.localRotation = Quaternion.Euler(180, 0, 0);
        }
        // 100 - fh - 2147483653
        else if (h && !v && !d)
        {
            GameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        // 011 - std90 - 1610612741
        else if (!h && v && d)
        {
            GameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
        }
        // 101 - std270 - 2684354565
        else if (h && !v && d)
        {
            GameObject.transform.localRotation = Quaternion.Euler(0, 0, 270);
        }
        // 110 - std180 - 3221225477
        else if (h && v && !d)
        {
            GameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
        }
        // 111 - fh90 - 3758096389
        else // if (h && v && d)
        {
            GameObject.transform.localRotation = Quaternion.Euler(0, 180, 90);
        }
        Vector3 pivotPoint = GameObject.transform.position - SpriteRenderer.bounds.extents;
        GameObject.transform.RotateAround(pivotPoint, Vector3.back, rotation);
    }

    public Tile CloneTo(float x, float y, float z)
    {
        var newPosition = new Vector3(x, y, z);
        var newTile = new Tile(SpriteRenderer.sprite, Id, Gid, newPosition, GameObject.transform.localScale.x);
        newTile.SetCollider(_tileCollider);
        
        return newTile;
    }

    private void UpdateZPosition()
    {
        SpriteRenderer.sortingOrder = OrderInLayer;
    }
}
