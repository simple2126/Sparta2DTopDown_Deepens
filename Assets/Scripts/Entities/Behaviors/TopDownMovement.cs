using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    // 실제로 이동이 일어날 컴포넌트
    private TopDownController movementController;
    private Rigidbody2D movementRigidbody;
    private CharacterStatsHandler characterStatsHandler;

    private Vector2 movementDirection = Vector2.zero;
    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;

    // 주로 내 컴포넌트 안에서 끝나는 거
    private void Awake()
    {
        // controller랑 TopDownMovement랑 같은 게임 오브젝트 안에 있다라는 가정
        // 다르면 GameObject.Find 사용해야 함
        movementController = GetComponent<TopDownController>();
        movementRigidbody = GetComponent<Rigidbody2D>();
        characterStatsHandler = GetComponent<CharacterStatsHandler>();
    }

    private void Start()
    {
        // OnMoveEvent에 Move를 호출하라고 등록함
        movementController.OnMoveEvent += Move;
    }

    private void FixedUpdate() // 실제로 움직임 처리
    {
        // 물리 업데이트에서 움직임 적용
        // Rigidbody의 값을 바꾸니까 FixedUpdate 실행
        ApplyMovement(movementDirection);

        if(knockbackDuration > 0.0f)
        {
            // 물리 시간
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }

    private void Move(Vector2 direction) // 프레임마다 돌림
    {
        // 이동방향만 정해두고 실제로 움직이지는 않음.
        // 움직이는 것은 물리 업데이트에서 진행(rigidbody가 물리니까)
        movementDirection = direction;
    }

    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        // 방향 other -> transform 방향
        knockback = -(other.position - transform.position).normalized * power;
    }

    private void ApplyMovement(Vector2 direction)
    {
        direction = direction * characterStatsHandler.CurrentStat.speed;

        if (knockbackDuration > 0.0f)
        {
            // 넉백 적용
            direction += knockback;
        }

        movementRigidbody.velocity = direction; // 입력 받은 속도로 이동 가능
    }
}
