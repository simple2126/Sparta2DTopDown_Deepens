using System.Collections;
using UnityEngine;

public class TopDownContactEnemyController : TopDownEnemyController
{
    // 어느 정도 거리일 때 따라오는지
    [SerializeField][Range(0f, 100f)] private float followRange;
    [SerializeField] private string targetTag = "Player";
    private bool isCollidingWithTarget;

    [SerializeField] private SpriteRenderer characterRenderer;

    private HealthSystem healthSystem;
    private HealthSystem collidingTargetHealthSystem;
    private TopDownMovement collidingMovement;

    protected override void Start()
    {
        base.Start();

        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDamage += OnDamage;
    }

    protected override void FixedUpdate()
    {
        // 단거리적은 플레이어처럼 입력을 받아서 움직이는 것은 아닙니다.
        // 그래서 단거리적은 어떤 식으로 움직일 지 로직을 저희가 직접 구현해야 합니다.

        base.FixedUpdate();

        if (isCollidingWithTarget)
        {
            ApplyHealthChange();
        }

        Vector2 direction = Vector2.zero;
        if (DistanceToTarget() < followRange)
        {
            // 점점 가까워짐
            direction = DirectionToTarget();
        }

        CallMoveEvent(direction);
        Rotate(direction);
    }

    private void OnDamage()
    {
        // 데미지 입으면 따라옴
        followRange = 100f;
    }

    private void Rotate(Vector2 direction)
    {
        // TopDownAimRotation에서 했었죠? 
        // Atan2는 가로와 세로의 비율을 바탕으로 -파이~파이(-180도~180도에 대응, * Rad2Deg가 그 기능)하는 값을 나타내주는 함수였다는 것 기억하시죠?
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject receiver = collision.gameObject;

        if (!receiver.CompareTag(targetTag))
        {
            return;
        }

        collidingTargetHealthSystem = receiver.GetComponent<HealthSystem>();
        if (collidingTargetHealthSystem != null)
        {
            isCollidingWithTarget = true;
        }

        collidingMovement = receiver.GetComponent<TopDownMovement>();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(targetTag))
        {
            return;
        }

        isCollidingWithTarget = false;
    }

    private void ApplyHealthChange()
    {
        AttackSO attackSO = stats.CurrentStat.attackSO;
        bool isAttacked = collidingTargetHealthSystem.isAttacked;
        if (!isAttacked)
        {
            collidingTargetHealthSystem.ChangeHealth(-attackSO.power);
            if (attackSO.isOnKnockback && collidingMovement != null)
            {
                collidingMovement.ApplyKnockback(transform, attackSO.knockbackPower, attackSO.knockbackTime);
            }
        }
    }
}