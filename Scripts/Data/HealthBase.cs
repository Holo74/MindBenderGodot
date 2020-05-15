using Godot;
using System;

public abstract class HealthBase : KinematicBody
{
    private bool init = false;
    private HealthBase master;
    public void Init(float maxHealth)
    {
        health = maxHealth;
        this.maxHealth = maxHealth;
        init = true;
    }
    public void Init(HealthBase parent, float health)
    {
        this.health = health;
        this.maxHealth = health;
        master = parent;
        init = true;
    }
    private float health = 0, maxHealth = 0;
    public float GetHealth()
    {
        if (!init)
            GD.Print("Health has not been initiated just yet on " + Name);
        return health;
    }
    public virtual bool TakeDamage(float damage, DamageType typing)
    {
        if (master != null)
        {
            master.health -= damage;
        }
        Damaged(damage);
        return true;
    }
    protected void Damaged(float damage)
    {
        if (!init)
            GD.Print("Health has not been initiated just yet on " + Name);
        if (master != null)
        {
            master.health -= damage;
        }
        health -= damage;
    }
    public virtual bool IsDead()
    {
        return GetHealth() <= 0;
    }
    public virtual bool Heal(float amount)
    {
        health = Mathf.Clamp(health + amount, -1, maxHealth);
        return true;
    }
}
