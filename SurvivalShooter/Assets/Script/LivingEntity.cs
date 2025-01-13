using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float maxHp = 100f;

    public float hp { get; private set; }
    public bool IsDead { get; private set; }

    public event Action onDeath;

    protected virtual void OnEnable()
    {
        IsDead = false;
        hp = maxHp;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
        //throw new System.NotImplementedException();
    }

    public virtual void Die()
    {
        onDeath?.Invoke();
        IsDead = true;
        hp = 0;
    }

    public virtual void AddHp(float add)
    {
        if (IsDead)
        {
            return;
        }
        else
        {
            hp += add;
            if (hp > maxHp)
            {
                hp = maxHp;
            }
        }
    }
}
