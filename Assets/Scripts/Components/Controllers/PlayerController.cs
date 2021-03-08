using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerController : PlayerControllerBase
{

    public VirtualJoystick moveJoystick { get; private set; }
    public VirtualJoystick attackJoystick { get; private set; }

    // 이동 입력 값을 나타냅니다.
    public Vector3 inputMovementAxis { get; private set; }

    // 공격 입력 값을 나타냅니다.
    public Vector3 inputAttackAxis { get; private set; }

	protected override void CreateUICanvas()
	{
		base.CreateUICanvas();

        moveJoystick = (screenInstance as GameScreenInsatnce).movementJoystick;
        attackJoystick = (screenInstance as GameScreenInsatnce).attackJoystick;
    }

	private void Update()
	{
        // 입력 축 값 갱신
        UpdateInputAxisValue();

        // 컨트롤러 회전
        RotateController();
    }

    // 입력 축 값을 갱신합니다.
    private void UpdateInputAxisValue()
    {
        inputMovementAxis = new Vector3(
            moveJoystick.inputAxis.x, 0.0f, moveJoystick.inputAxis.y);

        inputAttackAxis = new Vector3(
            attackJoystick.inputAxis.x, 0.0f, attackJoystick.inputAxis.y);
    }

    // 컨트롤러를 회전시킵니다.
    private void RotateController()
	{
        // 조이스틱 입력이 없다면 실행 X
        if (!attackJoystick.isInput) return;

        // 입력 방향을 Y 축 기준 회전값으로 변경합니다.
        float yawAngle = inputAttackAxis.ToAngle();

        Vector3 eulerAngle = new Vector3(0.0f, yawAngle, 0.0f);

        // 컨트롤러를 회전시킵니다.
        SetControlRotation(eulerAngle);
	}
}
