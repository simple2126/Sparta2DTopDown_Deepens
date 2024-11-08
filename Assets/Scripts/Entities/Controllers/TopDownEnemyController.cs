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
        // �÷��̾� 0, 0 �̰� �� 1, 1 �̸� -1, -1�� �ż� �ش� �������� �̵�
        return (ClosestTarget.position - transform.position).normalized;
    }
}
