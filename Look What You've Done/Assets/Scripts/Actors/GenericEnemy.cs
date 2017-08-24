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
            if (myLevel.CanIGo(transform.position, Direction.Up))
            {
                dirs.Add(Direction.Up);
            }
            if (myLevel.CanIGo(transform.position, Direction.Down))
            {
                dirs.Add(Direction.Down);
            }
            if (myLevel.CanIGo(transform.position, Direction.Left))
            {
                dirs.Add(Direction.Left);
            }
            if (myLevel.CanIGo(transform.position, Direction.Right))
            {
                dirs.Add(Direction.Right);
            }
            if (dirs.Count == 0)
            {
                // relax, there is nowhere to go
                return;
            }
            var rnd = UnityEngine.Random.Range((int)0, (int)dirs.Count);
            Direction direction = dirs[rnd];
            mc.Go(direction);
            Suffer(5);
        }
        
    }
}
