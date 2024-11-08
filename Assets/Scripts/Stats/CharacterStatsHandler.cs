using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsHandler : MonoBehaviour
{
    // �⺻ ����, �߰� ���� ����ؼ� ���� ���� ���
    // ������ �⺻ ���ȸ�

    [SerializeField] private CharacterStat baseStat;

    // ���� ����
    public CharacterStat CurrentStat { get; private set; }

    // �߰� ���� ����Ʈ
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
        // TODO : ������ �⺻ �ɷ�ġ�� ���߿� �⺻ �ɷ�ġ ��ȭ �߰�
        CurrentStat.statChangeType = baseStat.statChangeType;
        CurrentStat.maxHealth = baseStat.maxHealth;
        CurrentStat.speed = baseStat.speed;
    }
}