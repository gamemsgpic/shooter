using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{


    public float damage = 20f;

    public float fireRate = 0.12f;
    public float reloadTime = 1f;

    public Transform firePosition;

    public float fireDistance = 50f;

    public AudioClip shotClip;
    private AudioSource audioSource;
    private LineRenderer lineRenderer;
    private PlayerHealth playerHealth;

    public ParticleSystem shellEffect;
    public ParticleSystem shotEffect;

    private float lastFireTime;

    private void Start()
    {

    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();
        playerHealth = GetComponent<PlayerHealth>();

        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer 컴포넌트가 없습니다!");
            return;
        }

        // 라인 렌더러 초기화
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;

        // 두께 설정
        lineRenderer.startWidth = 0.05f;  // 라인의 시작 두께
        lineRenderer.endWidth = 0.05f;    // 라인의 끝 두께

        // 색상 설정
        lineRenderer.startColor = Color.red; // 라인의 시작 색상
        lineRenderer.endColor = Color.yellow; // 라인의 끝 색상

    }

    private void OnEnable()
    {
        lastFireTime = 0f;
    }

    public void Fire()
    {
        shotEffect.transform.position = firePosition.position;
        shotEffect.Play();

        if (playerHealth.IsDead || Time.time <= lastFireTime + fireRate)
        {
            return;
        }
        lastFireTime = Time.time;
        var endPos = Vector3.zero;
        Ray ray = new Ray(firePosition.position, firePosition.forward);

        var hits = Physics.RaycastAll(ray, fireDistance);
        System.Array
            .Sort(hits, (x, y) => Vector3.Distance(transform.position, x.point)
            .CompareTo(Vector3.Distance(transform.position, y.point)));
        if (hits.Length > 0)
        {
            var hit2 = hits[0];
            endPos = hit2.point;
            var damagable = hit2.collider.GetComponent<IDamageable>();
            if (damagable != null)
            {
                damagable.OnDamage(damage, hit2.point, hit2.normal);
            }
        }

        // 무한히 쏘고 싶다면 Mathf.Infinity
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            endPos = hit.point;

            var damagable = hit.collider.GetComponent<IDamageable>();
            if (damagable != null)
            {
                damagable.OnDamage(damage, hit.point, hit.normal);
            }
        }
        else
        {
            endPos = firePosition.position + firePosition.forward * fireDistance;

        }
        StartCoroutine(ShotEffect(endPos));
    }

    private IEnumerator ShotEffect(Vector3 hitPoint)
    {
        audioSource.PlayOneShot(shotClip);

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePosition.position);
        lineRenderer.SetPosition(1, hitPoint);

        shellEffect.Play();

        yield return new WaitForSeconds(0.03f);

        lineRenderer.enabled = false;

    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Fire();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(firePosition.position, firePosition.position + firePosition.forward * fireDistance);
    }
}
