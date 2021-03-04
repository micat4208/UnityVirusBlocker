using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public sealed class PlayerCharacterMovement : 
	MonoBehaviour
{
	[Header("이동 속력")]
	[SerializeField] private float _MoveSpeed = 20.0f;

	[Header("제동력")]
	[SerializeField] private float _BrackingForce = 10.0f;

	// 플레이어가 조종할 수 있는 캐릭터를 나타냅니다.
	private PlayerableCharacter _PlayerableCharacter;

	// 함께 사용되는 CharacterController Component 를 나타냅니다.
	private CharacterController _CharacterController;

	private Vector3 _SmoothVelocity;


	private void Awake()
	{
		_PlayerableCharacter = GetComponent<PlayerableCharacter>();
		_CharacterController = GetComponent<CharacterController>();
	}

	private void Update()
	{
		Movement();
	}

	// 캐릭터 이동을 수행합니다.
	private void Movement()
	{
		// 이동 축 값을 저장합니다.
		Vector3 inputMovementAxis = 
		(_PlayerableCharacter.playerController as PlayerController).inputMovementAxis;

#if UNITY_EDITOR
		Vector3 pcInput = new Vector3(
			Input.GetAxisRaw("Horizontal"),
			0.0f,
			Input.GetAxisRaw("Vertical")).normalized;

		if (pcInput.sqrMagnitude >= 0.1f)
			inputMovementAxis = pcInput;
#endif

		// 속도를 저장합니다.
		Vector3 velocity = inputMovementAxis * _MoveSpeed;

		_SmoothVelocity = Vector3.Lerp(
			_SmoothVelocity, velocity, _BrackingForce * Time.deltaTime);
		/// - Vector3.Lerp(Vector3 a, Vector3 b, float t) : a 부터 b 까지 (t * 100)% 만큼
		///   이동시킨 값을 반환합니다.
		///   ex) Vector3.Lerp(Vector3.zero, Vector3.one, 0.5f) => (0.5, 0.5, 0.5)

		// 캐릭터를 이동시킵니다.
		_CharacterController.SimpleMove(_SmoothVelocity);
		/// - SimpleMove(Vector3 speed) : 캐릭터를 speed 속도로 이동시킵니다.
		/// - 간단한 이동을 구현할 때 사용하며, 점프 기능이 없는 캐릭터 이동에 사용됩니다.
	}







}
