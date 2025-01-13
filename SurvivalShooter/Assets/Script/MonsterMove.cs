using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public class MonsterMove : LivingEntity
{
    public readonly int hashMove = Animator.StringToHash("Move");

    public LayerMask targetLayers;

    public LivingEntity target;
    public MonsterAttack monsterAttack;
    public float findTarget = 10f;

    private Animator animator;
    private NavMeshAgent agent;
    //private AudioSource audioSource;

    private Coroutine coUpdatePath;

    public ParticleSystem hitEffect;
    //public AudioClip hitSound;
    //public AudioClip deathSound;

    private Renderer renderer;

    public void Setup(MonsterData data)
    {
        if (data == null)
        {
            Debug.LogError("MonsterData가 null입니다! Setup 호출 시 데이터를 전달하세요.");
            return;
        }

        maxHp = data.hp;

        if (monsterAttack != null)
        {
            monsterAttack.damege = data.damage;
        }
        else
        {
            Debug.LogError("MonsterAttack 컴포넌트가 없습니다!");
        }

        agent.speed = data.speed;

        // 렌더러가 없을 경우 색상 설정을 생략
        if (renderer != null)
        {
            renderer.material.color = data.skinColor;
        }
        else
        {
            Debug.LogWarning("Renderer가 없습니다. 색상 설정을 건너뜁니다.");
        }
    }

    public bool HasTarget
    {
        get
        {
            return target != null && !target.IsDead;
        }
    }

    private void Awake()
    {
        //audioSource = GetComponent<AudioSource>();
        //zombieAttack = GetComponent<ZombieAttack>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        monsterAttack = GetComponentInChildren<MonsterAttack>(); // 자식에서도 검색
        renderer = GetComponentInChildren<Renderer>();

        if (monsterAttack == null)
        {
            Debug.LogError("MonsterAttack 컴포넌트가 없습니다! 프리팹을 확인하세요.");
        }

        if (renderer == null)
        {
            Debug.LogError("Renderer가 없습니다! 프리팹을 확인하세요.");
        }

        agent.enabled = true;

        var cols = GetComponents<Collider>();
        foreach (var col in cols)
        {
            col.enabled = true;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        coUpdatePath = StartCoroutine(CoUpdatePath());
    }

    protected void OnDisable()
    {


        target = null;
    }

    public void Update()
    {
        animator.SetFloat(hashMove, agent.velocity.magnitude / agent.speed);
    }

    private IEnumerator CoUpdatePath()
    {
        while (true)
        {
            if (!HasTarget)
            {
                agent.isStopped = true;
                target = FindTarget();
            }

            if (HasTarget)
            {
                agent.isStopped = false;
                agent.SetDestination(target.transform.position);
            }

            //yield return null;
            yield return new WaitForSeconds(0.25f);
        }
    }

    public LivingEntity FindTarget()
    {
        var cols = Physics.OverlapSphere(transform.position, findTarget, targetLayers.value);
        foreach (var col in cols)
        {
            var livingEntity = col.GetComponent<LivingEntity>();
            if (livingEntity != null && !livingEntity.IsDead)
            {
                return livingEntity;
            }
        }
        return null;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);

        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();
        //audioSource.PlayOneShot(hitSound);
    }

    public override void Die()
    {
        base.Die();

        //audioSource.PlayOneShot(deathSound);
        animator.SetTrigger("Dead");

        StopCoroutine(coUpdatePath);
        coUpdatePath = null;

        agent.isStopped = true;
        agent.enabled = false;

        var cols = GetComponents<Collider>();
        foreach (var col in cols)
        {
            col.enabled = false;
        }
    }
}
