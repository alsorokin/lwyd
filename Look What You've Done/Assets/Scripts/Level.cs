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
    }

    public bool CanIGo(Vector2 myPosition, Direction dir)
    {
        int x = (int)((levelWidth / 2) + myPosition.x - 0.5f);
        int y = (int)((levelHeight / 2) + myPosition.y - 0.5f);
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
            // can't go nowhere
            return false;
        }
        return levelTiles[x, y].passable;
    }

    public void AddActor(Actor actor)
    {
        actors.Add(actor);
    }
    
}
