using UnityEngine;

public enum StatsChangeType
{
    Add,
    Multiple,
    Override
};

// SerializeField의 클래스 버전
// 데이터 폴더처럼 사용
[System.Serializable]
public class CharacterStat
{
    public StatsChangeType statChangeType;
    // 슬라이더 생성
    [Range(1, 100)] public int maxHealth;
    [Range(1f, 20f)] public float speed;
    public AttackSO attackSO;
}