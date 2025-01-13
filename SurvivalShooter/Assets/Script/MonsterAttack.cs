using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private float attackTime = 0f;
    public float attackDeley = 2f;
    public float damege = 10f;

    private bool canAttack = false;

    private void OnTriggerEnter(Collider other)
    {
        canAttack = true;

        if (canAttack)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<LivingEntity>();
                if (player != null && !player.IsDead)
                {
                    player.OnDamage(damege, Vector3.zero, Vector3.zero);
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {

        attackTime += Time.deltaTime;

        if (attackTime > attackDeley && canAttack)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<LivingEntity>();
                if (player != null && !player.IsDead)
                {
                    player.OnDamage(damege, Vector3.zero, Vector3.zero);
                }
            }
            attackTime = 0f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        attackTime = 0f;
        canAttack = false;
    }


}
