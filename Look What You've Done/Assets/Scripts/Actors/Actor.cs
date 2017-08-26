using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    protected MovementController mc;
    private float _health = 75;
    protected float _maxHealth = 100;
    protected Level myLevel;

    public bool alive
    {
        get
        {
            return health > 0f;
        }
    }
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            ApplyHealthColor();
        }
    }
    public float maxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }
    public bool clonable { get; set; }

    public abstract GameObject Clone();

    protected virtual void Start()
    {
        mc = GetComponent<MovementController>();
        ApplyHealthColor();
    }

    public void Suffer (float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    protected void ApplyHealthColor()
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, health / maxHealth, health / maxHealth);
    }

    public void Die()
    {
        health = 0;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Resurrect()
    {
        health = maxHealth;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void SetLevel(Level level)
    {
        myLevel = level;
    }

    public void setMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
    }
}
