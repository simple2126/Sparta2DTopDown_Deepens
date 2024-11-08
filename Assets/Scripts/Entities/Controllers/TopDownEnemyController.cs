using UnityEngine;

public class TopDownEnemyController : TopDownController
{
    protected Transform ClosestTarget { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected virtual void Start()
    {
        ClosestTarget = GameManaer.Instance.Player;
    }

    protected virtual void FixedUpdate()
    {

    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, ClosestTarget.position);
    }

    protected Vector2 DirectionToTarget()
    {
        // 플레이어 0, 0 이고 적 1, 1 이면 -1, -1이 돼서 해당 방향으로 이동
        return (ClosestTarget.position - transform.position).normalized;
    }
}
