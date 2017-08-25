using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GenericEnemy : Actor
{
    private float timeElapsed;

    void FixedUpdate()
    {
        if (!isAlive)
        {
            return;
        }

        timeElapsed += Time.deltaTime;

        // go in random direction every second
        if (timeElapsed >= 1)
        {
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
                // relax, there is nowhere to go
                return;
            }
            var rnd = UnityEngine.Random.Range(0, dirs.Count);
            Direction direction = dirs[rnd];
            mc.Go(direction);
            //Suffer(5);
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
        Actor geActor = result.GetComponent<GenericEnemy>();
        geActor.SetLevel(myLevel);
        myLevel.AddActor(geActor);
        geActor.setMaxHealth(maxHealth);
        geActor.SetHealth(health);
        geActor.fertile = false;

        return result;
    }
}
