﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        for (int w = 0; w < levelWidth; w++)
        {
            for (int h = 0; h < levelHeight; h++)
            {
                if (h == 0 || h == levelHeight - 1 || w == 0 || w == levelWidth - 1)
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

        SpawnGenericEnemyAt(new Vector2(1f, 1f));
        SpawnGenericEnemyAt(new Vector2(TranslateGridToX(levelWidth - 2), TranslateGridToY(levelHeight - 2)));

        GameObject player = UnityEngine.GameObject.Instantiate(Resources.Load<GameObject>("Tiles/Player"));
        player.transform.position = new Vector3(TranslateGridToX(levelWidth / 2), TranslateGridToY(levelHeight / 2), 0f);
        Actor playerActor = player.GetComponent<TestPlayermoveScript>();
        playerActor.SetLevel(this);
        playerActor.fertile = false;
        AddActor(playerActor);
    }

    public void SpawnGenericEnemyAt(Vector2 position)
    {
        GameObject genericEnemy = UnityEngine.GameObject.Instantiate(Resources.Load<GameObject>("Tiles/GenericEnemyTile"));
        genericEnemy.transform.position = new Vector3(position.x, position.y, 1f);
        GenericEnemy ge = genericEnemy.GetComponent<GenericEnemy>();
        ge.SetLevel(this);
        ge.fertile = true;
        ge.health = ge.maxHealth;
        AddActor(ge);
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
        return (float)gridX;
    }

    public float TranslateGridToY(int gridY)
    {
        return (float)gridY;
    }

    public bool CanIGo(Actor myself, Vector2 fromPosition, Direction dir)
    {
        int x = TranslateXToGrid(fromPosition.x);
        int y = TranslateYToGrid(fromPosition.y);
        if (dir == Direction.Left)
        {
            x--;
        } else if (dir == Direction.Top)
        {
            y++;
        } else if (dir == Direction.Right)
        {
            x++;
        } else if (dir == Direction.Down)
        {
            y--;
        } else
        {
            // Direction.None or unknown direction
            return false;
        }

        var canGo = levelTiles[x, y].passable;
        // if it's passable, doesn't mean we can go there
        // maybe someone else is occupying it?
        if (canGo)
        {
            foreach (Actor actor in actors)
            {
                if (actor != myself && actor.alive &&
                    TranslateXToGrid(actor.gameObject.transform.position.x) == x &&
                    TranslateYToGrid(actor.gameObject.transform.position.y) == y)
                {
                    canGo = false;
                }

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
        actors.Add(actor);
    }
}
