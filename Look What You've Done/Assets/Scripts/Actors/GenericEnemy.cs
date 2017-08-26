using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GenericEnemy : Actor
{
    private float timeElapsed;

    public float moveTimeThreshold = 1f;

    protected override void Start()
    {
        base.Start();
        // start thinking right away!
        timeElapsed = moveTimeThreshold;
    }

    void FixedUpdate()
    {
        if (!alive || mc == null)
        {
            return;
        }

        timeElapsed += Time.deltaTime;

        // go in random direction every second
        if (timeElapsed >= moveTimeThreshold)
        {
            if (!clonable)
            {
                // All you zombies...
                Suffer(1);
            }
            timeElapsed = 0;
            List<Direction> dirs = new List<Direction>();
            if (myLevel.CanIGo(this, Direction.Up))
            {
                dirs.Add(Direction.Up);
            }
            if (myLevel.CanIGo(this, Direction.Down))
            {
                dirs.Add(Direction.Down);
            }
            if (myLevel.CanIGo(this, Direction.Left))
            {
                dirs.Add(Direction.Left);
            }
            if (myLevel.CanIGo(this, Direction.Right))
            {
                dirs.Add(Direction.Right);
            }
            if (dirs.Count == 0)
            {
                // I'm suffocating!
                Suffer(5);
                return;
            }
            var rnd = UnityEngine.Random.Range(0, dirs.Count);
            Direction direction = dirs[rnd];
            mc.Go(direction);
        }
    }

    public override GameObject Clone()
    {
        if (!clonable)
        {
            return null;
        }

        // configuring GameObject
        GameObject result = UnityEngine.GameObject.Instantiate(this.gameObject);
        result.GetComponent<SpriteRenderer>().enabled = true;

        // configuring Actor
        GenericEnemy ge = result.GetComponent<GenericEnemy>();
        ge.SetLevel(myLevel);
        myLevel.AddActor(ge);
        ge.setMaxHealth(maxHealth);
        if (alive)
        {
            ge.health = health;
        } else
        {
            ge.health = maxHealth;
        }
        ge.clonable = false;
        ge.moveTimeThreshold = UnityEngine.Random.Range(0.5f, 1.5f);
        ge.enabled = true;

        //configuring MovementController
        MovementController geMc = result.GetComponent<MovementController>();
        geMc.movementSpeed = UnityEngine.Random.Range(100, 300);

        return result;
    }
}
