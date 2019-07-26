using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    protected MovementController CurrentMovementController;
    private float _health = 75;
    protected Level CurrentLevel;
    private bool _hasShadow;
    private readonly float originalZ = -100f;

    private static readonly string shadow_tag = "actor_shadow";
    private static readonly string body_tag = "actor_body";
    private readonly string shadow_sprite_name = "Sprites/character-shadow";

    protected SpriteRenderer MySpriteRenderer => gameObject.GetComponent<SpriteRenderer>();

    protected SpriteRenderer ShadowSpriteRenderer => Shadow.GetComponent<SpriteRenderer>();

    protected SpriteRenderer BodySpriteRenderer => Body.GetComponent<SpriteRenderer>();

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
        get => _hasShadow;
        set => SetShadowEnabled(value);
    }

    public bool Alive => Health > 0f;

    public float Health
    {
        get => _health;
        set
        {
            _health = value;
            ApplyHealthColor();
        }
    }

    public float MaxHealth { get; set; } = 100;

    public bool Cloneable { get; set; }

    public abstract GameObject Clone();

    public void Suffer(float damage)
    {
        Health -= damage;
        if (Health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Health = 0;
        MySpriteRenderer.enabled = false;
    }

    public void Resurrect()
    {
        Health = MaxHealth;
        MySpriteRenderer.enabled = true;
    }

    public void SetLevel(Level level) => CurrentLevel = level;

    public void SetMaxHealth(float maxHealth) => MaxHealth = maxHealth;

    protected virtual void Start()
    {
        CurrentMovementController = GetComponent<MovementController>();
        ApplyHealthColor();
    }

    protected void ApplyHealthColor() =>
        BodySpriteRenderer.color = new Color(1, Health / MaxHealth, Health / MaxHealth);

    private int OrderInLayer =>
        (int)((-gameObject.transform.position.y - Offset - originalZ) * 100f);

    private int ShadowOrderInLayer =>
        (int)((-gameObject.transform.position.y - Offset + 10f) * 100f);


    // lower sprite boundary
    private float Offset =>
        -(BodySpriteRenderer.bounds.size.y / 2);

    private void LateUpdate()
    {
        BodySpriteRenderer.sortingOrder = OrderInLayer;
        if (HasShadow)
        {
            ShadowSpriteRenderer.sortingOrder = ShadowOrderInLayer;
            // TODO: y-positioning code goes here (if jumping/flying is implemented)
        }
    }

    private void SetShadowEnabled(bool value)
    {
        _hasShadow = value;

        // Some sort of lazy init here: only create the shadow if it's really needed
        if (Shadow == null && _hasShadow)
        {
            Shadow = new GameObject();
            Shadow.name = "shadow";
            Shadow.transform.tag = shadow_tag;
            var shadowSpriteRenderer = Shadow.AddComponent<SpriteRenderer>();
            shadowSpriteRenderer.sprite = Resources.Load<Sprite>(shadow_sprite_name);
            shadowSpriteRenderer.color = new Color(1f, 1f, 1f, 0.27f);
            Transform shadowTransform = Shadow.transform;
            shadowTransform.parent = gameObject.transform;
            shadowTransform.localPosition = new Vector3(0f, Offset, 0f);
        }
        else if (Shadow != null && !_hasShadow)
        {
            Shadow.SetActive(false);
        }
        else if (Shadow != null && _hasShadow)
        {
            Shadow.SetActive(true);
        }
    }
}
