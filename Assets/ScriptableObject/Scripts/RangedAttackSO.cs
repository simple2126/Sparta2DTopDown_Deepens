using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttackSO", menuName = "TopDownController/Attacks/Ranged", order =1)]

public class RangedAttackSO : AttackSO
{
    [Header("Ranged Attack Info")]
    public string bulletNameTag;
    public float duration; // 어느 시간동안 나가는지
    public float spread; // 랜덤으로 얼마나 퍼질지
    public int numberOfProjectilesPerShoot; // 한번 나갈 때 몇 개씩 나가는지
    public float multipleProjectilesAngle;
    public Color projectileColor;
}