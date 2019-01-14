﻿using UnityEngine;

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
                newCircleCollider.radius = collider.bounds.width / this.SpriteRenderer.sprite.pixelsPerUnit / 2;
                newCircleCollider.offset = new Vector2(
                    ((collider.bounds.width / 2) - tileWidthPixelsHalf + collider.bounds.x) / this.SpriteRenderer.sprite.pixelsPerUnit,
                    -((collider.bounds.height / 2) - tileWidthPixelsHalf + collider.bounds.y) / this.SpriteRenderer.sprite.pixelsPerUnit);
                break;
        }
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
            throw new System.Exception("Looks like I didn't handle all possible variants of tile flipping. Please contact me and tell me I'm dumb.");
        }
    }

    public Tile Clone()
    {
        var newTile = new Tile(SpriteRenderer.sprite, Id, Gid, this.gameObject.transform.position, this.gameObject.transform.localScale.x);
        newTile.SetCollider(this.tileCollider);
        
        return newTile;
    }
}
