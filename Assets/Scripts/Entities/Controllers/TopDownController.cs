using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent; // event ������ void�� ��ȯ �ƴϸ� Func
    public event Action<Vector2> OnLookEvent;
    public event Action<AttackSO> OnAttackEvent; // �޴� �� ���� �����ٴ� ��Ǹ� Ȯ���ϸ� ��

    protected bool isAttacking { get; set; }

    // ���� ���� ���� �� �� ��������
    private float timeSinceLastAttack = float.MaxValue;

    protected CharacterStatsHandler stats { get; private set; }

    protected virtual void Awake()
    {
        stats = GetComponent<CharacterStatsHandler>();
    }

    protected virtual void Update()
    {
        HandleAttackDelay();
    }

    private void HandleAttackDelay()
    {
        if(timeSinceLastAttack < stats.CurrentStat.attackSO.delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
        else if (isAttacking && timeSinceLastAttack >= stats.CurrentStat.attackSO.delay)
        {
            timeSinceLastAttack = 0f;
            CallAttackEvent(stats.CurrentStat.attackSO);
        }
    }


    public void CallMoveEvent(Vector2 direction) // MoveEvent �߻� �� Invoke ����
    {
        OnMoveEvent?.Invoke(direction); // ? -> ������ ���� ������ ����!
    }

    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }

    public void CallAttackEvent(AttackSO attackSO)
    {
        OnAttackEvent?.Invoke(attackSO);
    }
}
