using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GenericEnemy : Actor
{
    private float timeElapsed;

    public float moveTimeThreshold = 1f;

    void FixedUpdate()
    {
        if (!alive)
        {
            return;
        }

        timeElapsed += Time.deltaTime;

        // go in random direction every second
        if (timeElapsed >= moveTimeThreshold)
        {
            if (!fertile)
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
            if (UnityEngine.Random.Range(0, 5) == 0 && myLevel.DoIHaveSomewhereToGo(this))
            {
                Clone();
            }
        }
    }

    public override GameObject Clone()
    {
        if (!fertile)
        {
            return null;
        }
        GameObject result = UnityEngine.GameObject.Instantiate(this.gameObject);
        GenericEnemy ge = result.GetComponent<GenericEnemy>();
        ge.SetLevel(myLevel);
        myLevel.AddActor(ge);
        ge.setMaxHealth(maxHealth);
        ge.health = health;
        ge.fertile = false;
        ge.moveTimeThreshold = UnityEngine.Random.Range(0.5f, 1.5f);

        return result;
    }
}
