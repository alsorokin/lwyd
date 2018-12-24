using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Direction = MovementController.Direction;

public class Level
{
    private int levelWidth;
    private int levelHeight;
    private List<Actor> actors = new List<Actor>();
    private Tile[,] levelTiles;
    public readonly float TileScale = 1f;

    public Tile GetLeftmostTile()
    {
        return levelTiles[0, 0];
    }

    public Tile GetRightmostTile()
    {
        return levelTiles[levelWidth - 1, levelHeight - 1];
    }

    public Tile GetTopmostTile()
    {
        return GetRightmostTile();
    }

    public Tile GetBottommostTile()
    {
        return GetLeftmostTile();
    }

    public Level(int width, int height)
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
                if (h == 0 || h == levelHeight - 1 || w == 0 || w == levelWidth - 1)
                {
                    levelTiles[w, h] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", TranslateGridToX(w), TranslateGridToY(h), TileScale, false);
                }
                else if ((h == 2 * levelHeight / 3 && w == 2 * levelWidth / 3) || (h == levelHeight / 3 && w == levelWidth / 3))
                {
                    PutSpawnerAt(w, h, true);
                }
                else if (h == 2 * levelHeight / 3 && w == levelWidth / 3)
                {
                    levelTiles[w, h] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", TranslateGridToX(w), TranslateGridToY(h), TileScale, false);
                }
                else if (h == levelHeight / 3 && w == 2 * levelWidth / 3)
                {
                    levelTiles[w, h] = TileFactory.Instance.CreateTileFromResourse("Tiles/WallTile", TranslateGridToX(w), TranslateGridToY(h), TileScale, false);
                }
                else
                {
                    levelTiles[w, h] = TileFactory.Instance.CreateTileFromResourse("Tiles/GrassTile", TranslateGridToX(w), TranslateGridToY(h), TileScale, true);
                }
            }
        }

        // Adding player
        GameObject player = GameObject.Instantiate(Resources.Load<GameObject>("Tiles/Player"));
        player.transform.position = new Vector3(TranslateGridToX(levelWidth / 2), TranslateGridToY(levelHeight / 2), 0f);
        var playerLocalScale = player.transform.localScale;
        player.transform.localScale = new Vector3(playerLocalScale.x * TileScale, playerLocalScale.y * TileScale, playerLocalScale.z);
        Actor playerActor = player.GetComponent<Hero>();
        playerActor.SetLevel(this);
        playerActor.Cloneable = false;
        AddActor(playerActor);

        // Adding camera and its controller
        var cameraObj = new GameObject();
        var playerCamera = cameraObj.AddComponent<Camera>();
        cameraObj.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10f);
        playerCamera.orthographic = true;
        cameraObj.AddComponent<CameraMovementController>();
        var cameraController = cameraObj.GetComponent<CameraMovementController>();
        cameraController.player = player;
        cameraController.level = this;
    }

    private void PutSpawnerAt(int w, int h, bool free)
    {
        levelTiles[w, h] = TileFactory.Instance.CreateTileFromResourse("Tiles/GrassTile", TranslateGridToX(w), TranslateGridToY(h), TileScale, false);
        var spawnerObj = GameObject.Instantiate(Resources.Load<GameObject>("Tiles/SpawnerTile"));
        var spawnerScale = spawnerObj.transform.localScale;
        spawnerObj.transform.localScale = new Vector3(spawnerScale.x * TileScale, spawnerScale.y * TileScale, spawnerScale.z);
        spawnerObj.transform.position = new Vector3(TranslateGridToX(w), TranslateGridToY(h), 0.9f);

        var spawner = spawnerObj.GetComponent<Spawner>();
        AddActor(spawner);
        spawner.SetLevel(this);

        var geProto = SpawnGenericEnemyAt(w, h, free);
        geProto.GetComponent<Actor>().enabled = false;
        geProto.GetComponent<SpriteRenderer>().enabled = false;
        spawner.Prototype = geProto;
    }

    // TODO: Use it or remove it
    public GameObject SpawnGenericEnemyAtRandomPoint(List<Tuple<int, int>> points, bool isFree)
    {
        foreach (var p in points)
        {
            if (!this.levelTiles[p.Item1, p.Item2].passable)
            {
                points.Remove(p);
            }
        }

        var chosenPoint = points.ElementAt(UnityEngine.Random.Range(0, points.Count));
        return SpawnGenericEnemyAt(chosenPoint.Item1, chosenPoint.Item2, isFree);
    }

    public GameObject SpawnGenericEnemyAt(int gridX, int gridY, bool isFree)
    {
        var genericEnemy = GameObject.Instantiate(Resources.Load<GameObject>(isFree ? "Tiles/GenericFreeEnemyTile" : "Tiles/GenericGridEnemyTile"));
        genericEnemy.transform.position = new Vector3(TranslateGridToX(gridX), TranslateGridToY(gridY), 1f);
        var localScale = genericEnemy.transform.localScale;
        genericEnemy.transform.localScale = new Vector3(localScale.x * TileScale, localScale.y * TileScale, localScale.z);
        var ge = genericEnemy.GetComponent<GenericEnemy>();
        ge.SetLevel(this);
        ge.Cloneable = true;
        ge.Health = ge.MaxHealth;
        AddActor(ge);

        return genericEnemy;
    }

    public int TranslateXToGrid(float x)
    {
        return (int)Math.Round(x / TileScale);
    }

    public int TranslateYToGrid(float y)
    {
        return (int)Math.Round(y / TileScale);
    }

    public float TranslateGridToX(int gridX)
    {
        return gridX * TileScale;
    }

    public float TranslateGridToY(int gridY)
    {
        return gridY * TileScale;
    }

    public bool CanIGo(Actor myself, Vector2 fromPosition, Direction dir)
    {
        int x = TranslateXToGrid(fromPosition.x);
        int y = TranslateYToGrid(fromPosition.y);

        switch (dir)
        {
            case Direction.Left:
                x--;
                break;
            case Direction.Top:
                y++;
                break;
            case Direction.Right:
                x++;
                break;
            case Direction.Down:
                y--;
                break;
            case Direction.TopLeft:
                x--;
                y++;
                break;
            case Direction.TopRight:
                x++;
                y++;
                break;
            case Direction.BottomRight:
                x++;
                y--;
                break;
            case Direction.BottomLeft:
                x--;
                y--;
                break;
            case Direction.None:
                break;
            default:
                return false;
        }

        // edge cases
        if (x < 0 || x >= levelWidth || y < 0 || y >= levelHeight)
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
