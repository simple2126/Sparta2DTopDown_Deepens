using System;
using UnityEngine;

public class TopDownRangeEnemyController : TopDownEnemyController
{
    [SerializeField][Range(0f, 100f)] private float followRange = 15f;
    [SerializeField][Range(0f, 100f)] private float shootRange = 10f;

    private int layerMaskTarget;

    protected override void Start()
    {
        base.Start();
        layerMaskTarget = stats.CurrentStat.attackSO.target;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        float distanceToTarget = DistanceToTarget();
        Vector2 directionToTarget = DirectionToTarget();

        UpdateEnemyState(distanceToTarget, directionToTarget);
    }

    private void UpdateEnemyState(float distanceToTarget, Vector2 directionToTarget)
    {
        isAttacking = false;
        if(distanceToTarget < followRange)
        {
            // 따라가야 됨
            CheckIfNear(distanceToTarget, directionToTarget);
        }
    }

    private void CheckIfNear(float distance, Vector2 direction)
    {
        // 가까우면 공격
        if(distance < shootRange)
        {
            TryShootAtTarget(direction);
        }
        else
        {
            CallMoveEvent(direction);
        }
    }

    private void TryShootAtTarget(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, shootRange, layerMaskTarget);

        // 맞았다면
        if(hit.collider != null)
        {
            PerformAttackAction(direction);
        }
        else
        {
            CallMoveEvent(direction);
        }
    }

    private void PerformAttackAction(Vector2 direction)
    {
        CallLookEvent(direction);
        // 때리기 위해 멈춤
        CallMoveEvent(Vector2.zero);
        isAttacking = true;
    }
}