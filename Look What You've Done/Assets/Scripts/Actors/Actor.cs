using System.Linq;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    protected MovementController mc;
    private float health = 75;
    protected Level myLevel;
    private bool hasShadow;
    private readonly float originalZ = -10f;

    private static readonly string shadow_tag = "actor_shadow";
    private static readonly string body_tag = "actor_body";
    public readonly string shadow_sprite_name = "Sprites/character-shadow";

    protected SpriteRenderer SpriteRenderer
    {
        get
        {
            return this.gameObject.GetComponent<SpriteRenderer>();
        }
    }

    public GameObject Body
    {
        get
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.tag == body_tag)
                {
                    return child.gameObject;
                }
            }

            return null;
        }
    }

    public GameObject Shadow
    {
        get;
        private set;
    }

    public bool HasShadow
    {
        get
        {
            return hasShadow;
        }
        set
        {
            SetShadowEnabled(value);
        }
    }

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

    private float Offset
    {
        get
        {
            return -(this.Body.GetComponent<SpriteRenderer>().bounds.size.y / 2);
        }
    }

    public abstract GameObject Clone();

    public void LateUpdate()
    {
        this.Body.GetComponent<SpriteRenderer>().sortingOrder = OrderInLayer;
        if (HasShadow)
        {
            // TODO: this actually should be ground layer
            this.Shadow.GetComponent<SpriteRenderer>().sortingOrder = OrderInLayer - 1;
        }
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
        var sr = this.Body.GetComponent<SpriteRenderer>();
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

    private void SetShadowEnabled(bool enabled)
    {
        hasShadow = enabled;

        // Some sort of lazy init here: only create the shadow if it's really needed
        if (this.Shadow == null && hasShadow)
        {
            this.Shadow = new GameObject();
            this.Shadow.transform.tag = shadow_tag;
            var spriteRenderer = this.Shadow.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Resources.Load<Sprite>(shadow_sprite_name);
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.27f);
            Transform shadowTransform = this.Shadow.transform;
            shadowTransform.parent = gameObject.transform;
            shadowTransform.localPosition = new Vector3(0f, -0.5f, 0f);
        }
        else if (this.Shadow != null && !hasShadow)
        {
            this.Shadow.SetActive(false);
        }
        else if (this.Shadow != null && hasShadow)
        {
            this.Shadow.SetActive(true);
        }
    }
}
