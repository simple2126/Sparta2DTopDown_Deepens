using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownController : MonoBehaviour
{
    public event Action<Vector2> OnMoveEvent; // event 무조건 void만 반환 아니면 Func
    public event Action<Vector2> OnLookEvent;
    public event Action<AttackSO> OnAttackEvent; // 받는 게 없이 눌렀다는 사실만 확인하면 됨

    protected bool isAttacking { get; set; }

    // 저번 공격 이후 몇 초 지났는지
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


    public void CallMoveEvent(Vector2 direction) // MoveEvent 발생 시 Invoke 역할
    {
        OnMoveEvent?.Invoke(direction); // ? -> 없으면 말고 있으면 실행!
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
