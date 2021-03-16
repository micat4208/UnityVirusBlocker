using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 캐릭터를 향해 이동하는 행동
public sealed class AIBHMoveToPlayer : 
	AIBehaviorBase
{
	[Range(0.1f, 10.0f)] [Header("이동 속력")]
	[SerializeField] private float _MoveSpeed = 0.3f;

	[Header("Yaw 회전 사용")]
	[SerializeField] private bool _UseYawRotation = true;

	[Header("방향 갱신 지연 시간")]
	[SerializeField] private float _DirectionUpdateDelay;

	// 마지막 방향 갱신 시간
	private float _LastDirectionUpdatedTime;

	// 플레이어 캐릭터 객체를 나타냅니다.
	private PlayerableCharacter _PlayerableCharacter;

	// 이동 방향
	private Vector3 _MoveDirection;

	// 목표 이동 방향
	private Vector3 _TargetMoveDirection;

	private void Start()
	{
		_LastDirectionUpdatedTime = Time.time - _DirectionUpdateDelay;

		_PlayerableCharacter = 
			PlayerManager.Instance.playerController.playerableCharacter as PlayerableCharacter;
	}

	private void Update()
	{
		// 방향 갱신
		UpdateDirection();

		// 회전
		RotationToDirection();
	}

	// 이동 방향을 바라보도록 회전시킵니다.
	private void RotationToDirection()
	{
		if (_UseYawRotation)
			transform.eulerAngles = Vector3.up * _MoveDirection.ToAngle();
	}

	// 이동 방향을 갱신합니다.
	private void UpdateDirection()
	{
		if (Time.time - _LastDirectionUpdatedTime >= _DirectionUpdateDelay)
		{
			_LastDirectionUpdatedTime = Time.time;

			_TargetMoveDirection = transform.position.To(
				_PlayerableCharacter.transform.position);
		}

		_MoveDirection = Vector3.Lerp(
			_MoveDirection, _TargetMoveDirection, _MoveSpeed * Time.deltaTime);
	}

	public override void Run()
	{

		IEnumerator Behavior()
		{
			while (true)
			{
				// 플레이어 캐릭터와의 거리가 가깝다면 행동 종료
				if (Vector3.Distance(transform.position, _PlayerableCharacter.transform.position) < 0.01f)
					break;
				else
				{
					transform.Translate(
						_MoveDirection * _MoveSpeed * Time.deltaTime, Space.World);
				}

				yield return null;
			}

			behaviorFinished = true;
		}

		StartCoroutine(Behavior());
	}


}
