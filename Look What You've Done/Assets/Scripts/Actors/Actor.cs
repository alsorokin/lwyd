using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    protected MovementController mc;
    private float health = 75;
    protected Level myLevel;
    private float Offset
    {
        get
        {
            return -(this.GetComponent<SpriteRenderer>().bounds.size.y / 2);
        }
    }
    private readonly float originalZ = -10f;

    public bool Alive
    {
        get
        {
            return Health > 0f;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            ApplyHealthColor();
        }
    }

    public float MaxHealth { get; set; } = 100;

    public bool Cloneable { get; set; }

    private int OrderInLayer
    {
        get
        {
            return (int)((-this.gameObject.transform.position.y - Offset - originalZ) * 100f);
        }
    }

    public abstract GameObject Clone();

    public void LateUpdate()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = OrderInLayer;
    }

    protected virtual void Start()
    {
        mc = GetComponent<MovementController>();
        ApplyHealthColor();
    }

    public void Suffer (float damage)
    {
        Health -= damage;
        if (Health <= 0f)
        {
            Die();
        }
    }

    protected void ApplyHealthColor()
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, Health / MaxHealth, Health / MaxHealth);
    }

    public void Die()
    {
        Health = 0;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Resurrect()
    {
        Health = MaxHealth;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void SetLevel(Level level)
    {
        myLevel = level;
    }

    public void SetMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
    }
}
