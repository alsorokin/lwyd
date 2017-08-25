using System;
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

        float offsetX = width / 2 - 0.5f;
        float offsetY = height / 2 - 0.5f;
        for (int w = 0; w < levelWidth; w++)
        {
            for (int h = 0; h < levelHeight; h++)
            {
                float x = w - offsetX;
                float y = h - offsetY;
                if (h == 0 || h == levelHeight - 1 || w == 0 || w == levelWidth - 1)
                {
                    levelTiles[h, w] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", x, y, false);
                }
                else if (h == levelHeight / 3 && w == levelWidth / 3)
                {
                    levelTiles[h, w] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", x, y, false);
                }
                else if (h == 2*levelHeight / 3 && w == 2*levelWidth / 3)
                {
                    levelTiles[h, w] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", x, y, false);
                }
                else if (h == 2*levelHeight / 3 && w == levelWidth / 3)
                {
                    levelTiles[h, w] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", x, y, false);
                }
                else if (h == levelHeight / 3 && w == 2*levelWidth / 3)
                {
                    levelTiles[h, w] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", x, y, false);
                }
                else
                {
                    levelTiles[h, w] = TileFactory.Instance.CreateTileFromResourse("Tiles/GrassTile", x, y, true);
                }
            }
        }

        GameObject genericEnemy = UnityEngine.GameObject.Instantiate(Resources.Load<GameObject>("Tiles/GenericEnemyTile"));
        genericEnemy.transform.position = new Vector3(-1.5f, -1.5f, 1f);
        Actor geActor = genericEnemy.GetComponent<GenericEnemy>();
        geActor.SetLevel(this);
        geActor.fertile = true;
        AddActor(geActor);

        GameObject player = UnityEngine.GameObject.Instantiate(Resources.Load<GameObject>("Tiles/Player"));
        player.transform.position = new Vector3(0.5f, 0.5f, 0f);
        Actor playerActor = player.GetComponent<TestPlayermoveScript>();
        playerActor.SetLevel(this);
        playerActor.fertile = false;
        AddActor(playerActor);
    }

    private int TranslateXToGrid(float x)
    {
        return (int)Math.Round((levelWidth / 2) + x - 0.5f);
    }

    private int TranslateYToGrid(float y)
    {
        return (int)Math.Round((levelHeight / 2) + y - 0.5f);
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
                if (actor != myself && actor.isAlive &&
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

    public void AddActor(Actor actor)
    {
        actors.Add(actor);
    }
    
}
