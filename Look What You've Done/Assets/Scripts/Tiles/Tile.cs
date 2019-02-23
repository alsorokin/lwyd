﻿using UnityEngine;
using System.Linq;

public class Tile
{
    public GameObject gameObject;
    // TODO: Add support for multiple colliders
    private TileCollider tileCollider;
    private readonly float originalZ;

    public float Offset { get; private set; }

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
        this.originalZ = position.z;
        Vector3 normalizedPosition = new Vector3(position.x, position.y, 0f);
        this.gameObject.transform.position = normalizedPosition;
        var renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        this.Offset = -(this.SpriteRenderer.sprite.pivot.y / this.SpriteRenderer.sprite.pixelsPerUnit);
        
        UpdateZPosition();
    }

    private int OrderInLayer
    {
        get
        {
            return (int)((-this.gameObject.transform.position.y - Offset - originalZ) * 100f);
        }
    }

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
        var oldBoxCollider = gameObject.GetComponent<BoxCollider2D>();
        if (oldBoxCollider != null)
        {
            GameObject.Destroy(oldBoxCollider);
        }

        var oldCircleCollider = gameObject.GetComponent<CircleCollider2D>();
        if (oldCircleCollider != null)
        {
            GameObject.Destroy(oldCircleCollider);
        }

        var oldPolygonCollider = gameObject.GetComponent<PolygonCollider2D>();
        if (oldPolygonCollider != null)
        {
            GameObject.Destroy(oldPolygonCollider);
        }

        if (collider == null)
        {
            // at this point we have removed every collider
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
        // note that the tile is not always 1 unit, so this is not necessarily equal to ppu
        var tileWidthPixels = this.SpriteRenderer.sprite.texture.width;
        var tileWidthPixelsHalf = tileWidthPixels / 2;
        var tileHeightPixels = this.SpriteRenderer.sprite.texture.height;
        var tileHeightPixelsHalf = tileHeightPixels / 2;
        float ppu = this.SpriteRenderer.sprite.pixelsPerUnit;

        if (collider is BoxTileCollider)
        {
            var boxCollider = collider as BoxTileCollider;
            var newBoxCollider = gameObject.AddComponent<BoxCollider2D>();
            newBoxCollider.size = new Vector2(
                boxCollider.bounds.width / ppu,
                boxCollider.bounds.height / ppu);
            newBoxCollider.offset = new Vector2(
                ((boxCollider.bounds.width / 2) - tileWidthPixelsHalf + boxCollider.bounds.x) / ppu,
                -((boxCollider.bounds.height / 2) - tileHeightPixelsHalf + boxCollider.bounds.y) / ppu);
            this.Offset = newBoxCollider.offset.y - (this.SpriteRenderer.sprite.texture.height / 2 / ppu);
        }
        else if (collider is CircleTileCollider)
        {
            var circleCollider = collider as CircleTileCollider;
            var newCircleCollider = gameObject.AddComponent<CircleCollider2D>();
            newCircleCollider.radius = circleCollider.Radius / ppu;
            newCircleCollider.offset = new Vector2(
                ((circleCollider.bounds.width / 2) - tileWidthPixelsHalf + circleCollider.bounds.x) / ppu,
                -((circleCollider.bounds.height / 2) - tileHeightPixelsHalf + circleCollider.bounds.y) / ppu);
            this.Offset = newCircleCollider.offset.y - (this.SpriteRenderer.sprite.texture.height / 2 / ppu);
        }
        else if (collider is PolygonTileCollider)
        {
            var polygonCollider = collider as PolygonTileCollider;
            var newPolygonCollider = gameObject.AddComponent<PolygonCollider2D>();
            newPolygonCollider.points = polygonCollider.Vertices.Select(v => new Vector2(v.x / ppu, -v.y / ppu)).ToArray();
            newPolygonCollider.offset = new Vector2(-tileWidthPixelsHalf / ppu, tileHeightPixelsHalf / ppu);
            this.Offset = newPolygonCollider.offset.y - (this.SpriteRenderer.sprite.texture.height / 2 / ppu);
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
        this.SpriteRenderer.sortingOrder = OrderInLayer;
    }
}
