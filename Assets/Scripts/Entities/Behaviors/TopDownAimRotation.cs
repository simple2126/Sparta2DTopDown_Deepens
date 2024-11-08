using System;
using UnityEngine;
using UnityEngine.Animations;

public class TopDownAimRotation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer armRenderer; // 화살표 뒤집기
    [SerializeField] private Transform armPivot; // WeaponPivot

    [SerializeField] private SpriteRenderer characterRenderer; // 캐릭터 뒤집기

    private TopDownController controller;

    private void Awake() 
    { 
        controller = GetComponent<TopDownController>();
    }

    private void Start()
    {
        controller.OnLookEvent += OnAim;
    }

    private void OnAim(Vector2 direction)
    {
        RotateArm(direction);
    }

    private void RotateArm(Vector2 direction)
    {
        // Degree로 변경 -> 쿼터니언.오일러(degree) 사용하기 때문
        // Mathf.Atan2 -> -PI부터 PI까지 값 반환
        // Mathf.Rad2Deg는 약 57.29 -> 1 라디안, PI 라디안일 때 1PI * 57.29 -> 180도
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        characterRenderer.flipX = Mathf.Abs(rotZ) > 90f;
        armRenderer.flipY = characterRenderer.flipX;
        armPivot.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}