using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public Vector2 position { get; set; }
    public bool isAlive
    {
        get
        {
            return health > 0f;
        }
    }

    protected MovementController mc;
    protected float health = 75;
    protected float maxHealth = 100;
    protected Level myLevel;

    void Start()
    {
        mc = GetComponent<MovementController>();
    }

    public void Suffer (float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die ()
    {
        health = 0;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SetLevel(Level level)
    {
        myLevel = level;
    }
    
}
