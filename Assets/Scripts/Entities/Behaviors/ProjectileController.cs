using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public  class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private bool isReady;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;
    private TrailRenderer trailRenderer;

    private RangedAttackSO attackData;
    private float currentDuration;
    private Vector2 direction;
    private bool fxOnDestroy = true;

    private void Awake()
    {
        // Arrow 아래에 있는 renderer 가져옴
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (!isReady)
        {
            return;
        }

        currentDuration += Time.deltaTime;

        if(currentDuration > attackData.duration)
        {
            DestroyProjectile(transform.position, false);
        }

        rigidbody.velocity = direction * attackData.speed;
    }

    public void initializeAttack(Vector2 direction, RangedAttackSO attackData)
    {
        this.attackData = attackData;
        this.direction = direction;

        UpdateProjectileSprite();
        trailRenderer.Clear();
        currentDuration = 0;
        spriteRenderer.color = attackData.projectileColor;

        transform.right = this.direction;

        isReady = true;
    }

    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        if (createFx)
        {
            // TODO : ParticleSystem에 대해서 배우고, 무기 NameTag로 해당하는 Fx 가져오기
            ParticleSystem particleSystem = GameManaer.Instance.EffectParticle;

            particleSystem.transform.position = position;
            ParticleSystem.EmissionModule em = particleSystem.emission;
            em.SetBurst(0, new ParticleSystem.Burst(0, Mathf.Ceil(attackData.size * 5)));
            ParticleSystem.MainModule mm = particleSystem.main;
            mm.startSpeedMultiplier = attackData.size * 10f;
            particleSystem.Play();
        }
        gameObject.SetActive(false);
    }

    private void UpdateProjectileSprite()
    {
        transform.localScale = Vector3.one * attackData.size;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsLayerMatched(levelCollisionLayer.value, collision.gameObject.layer))
        {
            Vector2 destroyPosition = collision.ClosestPoint(transform.position) - direction * 0.2f;
            DestroyProjectile(destroyPosition, fxOnDestroy);
        }
        else if (IsLayerMatched(attackData.target.value, collision.gameObject.layer))
        {
            HealthSystem healthSystem = collision.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                // 충돌한 오브젝트의 체력을 감소시킵니다.
                healthSystem.ChangeHealth(-attackData.power);

                // 넉백이 활성화된 경우, ★드★디★어★ 넉백을 적용합니다.
                if (attackData.isOnKnockback)
                {
                    ApplyKnockback(collision);
                }
            }
            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestroy);
        }
    }

    private void ApplyKnockback(Collider2D collision)
    {
        TopDownMovement movement = collision.GetComponent<TopDownMovement>();
        if (movement != null)
        {
            movement.ApplyKnockback(transform, attackData.knockbackPower, attackData.knockbackTime);
        }
    }

    private bool IsLayerMatched(int value, int layer)
    {
        // 다른 비트 들어왔는지 검사
        return value == (value | (1 << layer));
    }
}
