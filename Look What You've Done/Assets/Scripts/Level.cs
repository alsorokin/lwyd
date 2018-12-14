﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level
{
    private int levelWidth;
    private int levelHeight;
    private List<Actor> actors = new List<Actor>();
    private Tile[,] levelTiles;

    public Level(int width, int height, Texture2D tilesTexture)
    {
        // TODO: load from file
        levelWidth = width;
        levelHeight = height;
        levelTiles = new Tile[levelWidth, levelHeight];

        // A stub of level generation
        for (int w = 0; w < levelWidth; w++)
        {
            for (int h = 0; h < levelHeight; h++)
            {
                if (h == 0 && w == levelWidth / 2)
                {
                    levelTiles[w, h] = TileFactory.Instance.CreateTileFromResourse("Tiles/GrassTile", w, h, false);
                    GameObject spawnerObj = UnityEngine.GameObject.Instantiate(Resources.Load<GameObject>("Tiles/SpawnerTile"));
                    spawnerObj.transform.position = new Vector3(TranslateGridToX(w), TranslateGridToY(h), 0.9f);

                    Spawner spawner = spawnerObj.GetComponent<Spawner>();
                    AddActor(spawner);
                    spawner.SetLevel(this);

                    GameObject geProto = SpawnGenericEnemyAt(new Vector2(w, h));
                    geProto.GetComponent<Actor>().enabled = false;
                    geProto.GetComponent<SpriteRenderer>().enabled = false;
                    spawner.Prototype = geProto;
                }
                else if (h == 0 || h == levelHeight - 1 || w == 0 || w == levelWidth - 1)
                {
                    levelTiles[w, h] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", w, h, false);
                }
                else if (h == levelHeight / 3 && w == levelWidth / 3)
                {
                    levelTiles[w, h] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", w, h, false);
                }
                else if (h == 2*levelHeight / 3 && w == 2*levelWidth / 3)
                {
                    levelTiles[w, h] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", w, h, false);
                }
                else if (h == 2*levelHeight / 3 && w == levelWidth / 3)
                {
                    levelTiles[w, h] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", w, h, false);
                }
                else if (h == levelHeight / 3 && w == 2*levelWidth / 3)
                {
                    levelTiles[w, h] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", w, h, false);
                }
                else
                {
                    levelTiles[w, h] = TileFactory.Instance.CreateTileFromResourse("Tiles/GrassTile", w, h, true);
                }
            }
        }

        GameObject player = GameObject.Instantiate(Resources.Load<GameObject>("Tiles/Player"));
        player.transform.position = new Vector3(TranslateGridToX(levelWidth / 2), TranslateGridToY(levelHeight / 2), 0f);
        Actor playerActor = player.GetComponent<Hero>();
        playerActor.SetLevel(this);
        playerActor.Cloneable = false;
        AddActor(playerActor);
    }

    public GameObject SpawnGenericEnemyAt(Vector2 position)
    {
        GameObject genericEnemy = UnityEngine.GameObject.Instantiate(Resources.Load<GameObject>("Tiles/GenericEnemyTile"));
        genericEnemy.transform.position = new Vector3(position.x, position.y, 1f);
        GenericEnemy ge = genericEnemy.GetComponent<GenericEnemy>();
        ge.SetLevel(this);
        ge.Cloneable = true;
        ge.Health = ge.MaxHealth;
        AddActor(ge);

        return genericEnemy;
    }

    public int TranslateXToGrid(float x)
    {
        return (int)Math.Round(x);
    }

    public int TranslateYToGrid(float y)
    {
        return (int)Math.Round(y);
    }

    public float TranslateGridToX(int gridX)
    {
        return gridX;
    }

    public float TranslateGridToY(int gridY)
    {
        return gridY;
    }

    public bool CanIGo(Actor myself, Vector2 fromPosition, Direction dir)
    {
        int x = TranslateXToGrid(fromPosition.x);
        int y = TranslateYToGrid(fromPosition.y);
        if (dir == Direction.Left)
        {
            x--;
        }
        else if (dir == Direction.Top)
        {
            y++;
        }
        else if (dir == Direction.Right)
        {
            x++;
        }
        else if (dir == Direction.Down)
        {
            y--;
        }
        else
        {
            // Direction.None or unknown direction
            return false;
        }

        // edge cases
        if ((x < 0 && dir == Direction.Left) ||
            (x >= levelWidth && dir == Direction.Right) ||
            (y < 0 && dir == Direction.Down) ||
            (y >= levelHeight && dir == Direction.Up))
        {
            return false;
        }

        var canGo = levelTiles[x, y].passable;
        // if it's passable, doesn't mean we can go there
        // maybe someone else is occupying it?
        // so if there's someone, and we are not a fighter, then we can't go
        // if we're a fighter, we can attack (and this is the same action as moving, so can go)
        if (canGo)
        {
            if (actors.Any(a => a != myself && a.Alive && 
                TranslateXToGrid(a.gameObject.transform.position.x) == x &&
                TranslateYToGrid(a.gameObject.transform.position.y) == y &&
                !(myself is Fighter)))
            {
                canGo = false;
            }
        }

        return canGo;
    }

    public bool CanIGo(Actor myself, Direction dir)
    {
        // we assume that the actor is not moving
        // this is a bad thing to assume, maybe we should check it instead
        return CanIGo(myself, myself.gameObject.transform.position, dir);
    }

    public bool DoIHaveSomewhereToGo(Actor myself)
    {
        // we assume that the actor is not moving
        // this is a bad thing to assume, maybe we should check it instead
        return 
            CanIGo(myself, myself.gameObject.transform.position, Direction.Up) ||
            CanIGo(myself, myself.gameObject.transform.position, Direction.Down) ||
            CanIGo(myself, myself.gameObject.transform.position, Direction.Left) ||
            CanIGo(myself, myself.gameObject.transform.position, Direction.Right);
    }

    public void AddActor(Actor actor)
    {
        if (!actors.Contains(actor))
        {
            actors.Add(actor);
        }
    }
}
