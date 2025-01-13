using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;

    public AudioClip deathSound;
    public AudioClip hitSound;
    public AudioClip itemPickupSound;

    private AudioSource audioSource;
    private Animator animator;
    private PlayerMove movement;
    private PlayerAiming aiming;
    //private PlayerShooter shooter;
    //private UIManager uiManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMove>();
        aiming = GetComponent<PlayerAiming>();
        //shooter = GetComponent<PlayerShooter>();

        movement.enabled = true;
        aiming.enabled = true;
        //shooter.enabled = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        //healthSlider.gameObject.SetActive(true);

        //healthSlider.maxValue = maxHp;
        //healthSlider.minValue = 0f;
        //healthSlider.value = hp;

        //healthSlider.value = hp / maxHp;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);

        //healthSlider.value = hp;
        if (!IsDead)
        {
            //audioSource.PlayOneShot(hitSound);
        }
    }

    public override void Die()
    {
        base.Die();

        //healthSlider.gameObject.SetActive(false);
        animator.SetTrigger("Die");

        //audioSource.PlayOneShot(deathSound);

        movement.enabled = false;
        aiming.enabled = false;
        //shooter.enabled = false;
    }

    public override void AddHp(float add)
    {
        base.AddHp(add);
        //healthSlider.value = hp;
    }

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnDamage(20, Vector3.zero, Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AddHp(20);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsDead)
        {
            return;
        }

        //if (other.CompareTag("Item"))
        //{
        //    var item = other.GetComponent<IItem>();
        //    item?.Use(gameObject);
        //
        //    audioSource.PlayOneShot(itemPickupSound);
        //}
    }
}

