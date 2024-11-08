using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : TopDownController
{
    private Camera camera;
    
    protected override void Awake()
    {
        base.Awake();
        camera = Camera.main; // mainCamera 태그가 붙어있는 카메라 가져오기    
    }

    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>().normalized; // 크기가 1인 벡터 변환
        CallMoveEvent(moveInput);

        // 실제 움직이는 처리는 PlayerMovement에서 함
    }

    public void OnLook(InputValue value)
    {
        Vector2 newAim = value.Get<Vector2>();
        Vector2 worldPos = camera.ScreenToWorldPoint(newAim);
        newAim = (worldPos - (Vector2)transform.position).normalized;
    
        CallLookEvent(newAim);
    }

    public void OnFire(InputValue value)
    {
        isAttacking = value.isPressed;
    }
}
