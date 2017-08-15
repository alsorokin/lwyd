using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Actor
{
    public Vector2 position { get; set; }
    public bool isAlive
    {
        get
        {
            // TODO: implement death
            return true;
        }
    }
    protected float health = 75;
    protected float maxHealth = 100;
}
