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
                levelTiles[w, h] = TileFactory.Instance.CreateTile(7, TranslateGridToX(w), TranslateGridToY(h), TileScale);
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

    public void AddActor(Actor actor)
    {
        if (!actors.Contains(actor))
        {
            actors.Add(actor);
        }
    }
}
