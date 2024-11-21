using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStatsHandler : MonoBehaviour
{
    // 기본 스탯, 추가 스탯 계산해서 최종 스탯 계산
    // 지금은 기본 스탯만

    [SerializeField] private CharacterStat baseStat;

    // 최종 스탯
    public CharacterStat CurrentStat { get; private set; } = new();

    // 추가 스탯 리스트
    public List<CharacterStat> statModifiers = new List<CharacterStat>();

    private readonly float MinAttackDelay = 0.03f;
    private readonly float MinAttackPower = 0.5f;
    private readonly float MinAttackSize = 0.4f;
    private readonly float MinAttackSpeed = .1f;

    private readonly float MinSpeed = 0.8f;

    private readonly int MinMaxHealth = 5;

    private void Awake()
    {
        UpdateCharacterStat();
    
        if(baseStat.attackSO != null)
        {
            // 일단 복사해놓기
            baseStat.attackSO = Instantiate(baseStat.attackSO);
            CurrentStat.attackSO = Instantiate(baseStat.attackSO);
        }
    }

    private void UpdateCharacterStat()
    {
        ApplyStatModifier(baseStat);

        foreach (var modifier in statModifiers.OrderBy(o => o.statChangeType))
        {
            ApplyStatModifier(modifier);
        }
    }

    public void AddStatModifier(CharacterStat modifier)
    {
        statModifiers.Add(modifier);
        UpdateCharacterStat();
    }

    public void RemoveStatModifier(CharacterStat modifier)
    {
        statModifiers.Remove(modifier);
        UpdateCharacterStat();
    }

    private void ApplyStatModifier(CharacterStat modifier)
    {
        Func<float, float, float> operation;
        switch (modifier.statChangeType)
        {
            case StatsChangeType.Add: operation = (current, change) => current + change; break;
            case StatsChangeType.Multiple: operation = (current, change) => current * change; break;
            default: operation = (current, change) => change; break;
        }


        UpdateBasicStats(operation, modifier);
        UpdateAttackStats(operation, modifier);

        if(CurrentStat.attackSO is RangedAttackSO currentRanged && modifier.attackSO is RangedAttackSO newRanged)
        {
            UpdateRangedAttackStats(operation, currentRanged, newRanged);
        }
    }

    private void UpdateBasicStats(Func<float, float, float> operation, CharacterStat modifier)
    {
        CurrentStat.maxHealth = Mathf.Max((int)operation(CurrentStat.maxHealth, modifier.maxHealth), MinMaxHealth);
        CurrentStat.speed = Mathf.Max(operation(CurrentStat.speed, modifier.speed), MinSpeed);
    }

    private void UpdateAttackStats(Func<float, float, float> operation, CharacterStat modifier)
    {
        if (CurrentStat.attackSO == null || modifier.attackSO == null) return;

        var currentAttack = CurrentStat.attackSO;
        var newAttack = modifier.attackSO;

        currentAttack.delay = Mathf.Max(operation(currentAttack.delay, newAttack.delay), MinAttackDelay);
        currentAttack.power = Mathf.Max(operation(currentAttack.power, newAttack.power), MinAttackPower);
        currentAttack.size = Mathf.Max(operation(currentAttack.size, newAttack.size), MinAttackSize);
        currentAttack.speed = Mathf.Max(operation(currentAttack.speed, newAttack.speed), MinAttackSpeed);
    }

    private void UpdateRangedAttackStats(Func<float, float, float> operation, RangedAttackSO currentRanged, RangedAttackSO newRanged)
    {
        currentRanged.multipleProjectilesAngle = operation(currentRanged.multipleProjectilesAngle, newRanged.multipleProjectilesAngle);
        currentRanged.spread = operation(currentRanged.spread, newRanged.spread);
        currentRanged.duration = operation(currentRanged.duration, newRanged.duration);
        currentRanged.numberOfProjectilesPerShoot = Mathf.CeilToInt(operation(currentRanged.numberOfProjectilesPerShoot, newRanged.numberOfProjectilesPerShoot));
        currentRanged.projectileColor = UpdateColor(operation, currentRanged.projectileColor, newRanged.projectileColor);
    }

    private Color UpdateColor(Func<float, float, float> operation, Color current, Color modifier)
    {
        return new Color
        (
            operation(current.r, modifier.r),
            operation(current.g, modifier.g),
            operation(current.b, modifier.b),
            operation(current.a, modifier.a)
        );
    }
}