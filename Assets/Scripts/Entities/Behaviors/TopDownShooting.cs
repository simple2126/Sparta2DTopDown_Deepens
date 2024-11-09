using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class TopDownShooting : MonoBehaviour
{
    private TopDownController controller;

    [SerializeField] private Transform projectileSpawnPosition; // 총알 생성 위치 지정
    private Vector2 aimDirection = Vector2.right;

    [SerializeField] private AudioClip shootingClip;

    private void Awake()
    {
        controller = GetComponent<TopDownController>();
    }

    private void Start()
    {
        controller.OnAttackEvent += OnShoot;

        controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 direction)
    {
        // 마우스 움직일 때마다 바뀜
        aimDirection = direction;
    }

    private void OnShoot(AttackSO attackSO)
    {
        // RangedAttackSO로 형변환 해본 다음에 실패하면 null 뜸
        RangedAttackSO rangedAttackSO = attackSO as RangedAttackSO;
        if (rangedAttackSO == null) return;

        float projecTileAngleSpace = rangedAttackSO.multipleProjectilesAngle;
        int numberOfProjectilesPerShot = rangedAttackSO.numberOfProjectilesPerShoot; // 한번에 몇 발 나가는지

        float minAngle = -(numberOfProjectilesPerShot / 2f) * projecTileAngleSpace + 0.5f * rangedAttackSO.multipleProjectilesAngle;

        for (int i =0;i < numberOfProjectilesPerShot; i++)
        {
            float angle = minAngle + i * projecTileAngleSpace;
            float randomSpread = Random.Range(-rangedAttackSO.spread, rangedAttackSO.spread);
            angle += randomSpread;
            // 투사체 생성
            CreateProjectile(rangedAttackSO, angle);
        }
    }

    private void CreateProjectile(RangedAttackSO rangedAttackSO, float angle)
    {
        GameObject obj = GameManaer.Instance.ObjectPool.SpawnFromPool(rangedAttackSO.bulletNameTag);
        obj.transform.position = projectileSpawnPosition.position;
        ProjectileController attackController = obj.GetComponent<ProjectileController>();
        attackController.initializeAttack(RotateVector2(aimDirection, angle), rangedAttackSO);

        if(shootingClip) SoundManager.PlayClip(shootingClip);
    }

    private static Vector2 RotateVector2(Vector2 v, float angle)
    {
        return Quaternion.Euler(0f, 0f, angle) * v;
    }
}
