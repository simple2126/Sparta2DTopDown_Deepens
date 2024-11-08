using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsHandler : MonoBehaviour
{
    // 기본 스탯, 추가 스탯 계산해서 최종 스탯 계산
    // 지금은 기본 스탯만

    [SerializeField] private CharacterStat baseStat;

    // 최종 스탯
    public CharacterStat CurrentStat { get; private set; }

    // 추가 스탯 리스트
    public List<CharacterStat> statModifiers = new List<CharacterStat>();

    private void Awake()
    {
        UpdateCharacterStat();
    }

    private void UpdateCharacterStat()
    {
        AttackSO attackSO = null;
        if(baseStat.attackSO != null)
        {
            attackSO = Instantiate(baseStat.attackSO);
        }

        CurrentStat = new CharacterStat { attackSO = attackSO };
        // TODO : 지금은 기본 능력치만 나중에 기본 능력치 강화 추가
        CurrentStat.statChangeType = baseStat.statChangeType;
        CurrentStat.maxHealth = baseStat.maxHealth;
        CurrentStat.speed = baseStat.speed;
    }
}