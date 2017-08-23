using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GenericEnemy : Actor
{
    MovementController mc;
    float time;

    void Start()
    {
        mc = GetComponent<MovementController>();
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        // go in random direction every second
        if (time >= 1)
        {
            time = 0;
            Direction direction = (Direction)UnityEngine.Random.Range(1, 4);
            mc.Go(direction);
        }
        
    }
}
