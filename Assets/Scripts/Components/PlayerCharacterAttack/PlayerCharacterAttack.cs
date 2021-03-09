﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Util;

// 플레이어 캐릭터 공격 기능을 담당하는 컴포넌트입니다.
public sealed class PlayerCharacterAttack : MonoBehaviour
{
	[Header("미사일 발사 위치")]
	[SerializeField] private Transform _MissileFireLeftPos;
	[SerializeField] private Transform _MissileFireRightPos;

	[Header("미사일 개수")]
	[SerializeField] private int _MissileCount = 10;

	[Header("미사일 발사 딜레이")]
	[SerializeField] private float _MissileDelay = 0.3f;

	// 마지막 발사 시간을 나타냅니다.
	private float _LastFiredTime;

	// 플레이어 캐릭터
	private PlayerableCharacter _PlayerableCharacter;

	// 미사일 오브젝트 프리팹
	private PlayerMissile _PlayerMissilePrefab;

	// 미사일 오브젝트 풀
	private ObjectPool<PlayerMissile> _PlayerMissilePool = new ObjectPool<PlayerMissile>();

	private void Awake()
	{
		_PlayerableCharacter = GetComponent<PlayerableCharacter>();

		// 미사일 오브젝트 프리팹 로드
		_PlayerMissilePrefab = ResourceManager.Instance.LoadResource<GameObject>(
			"PlayerMissile",
			"Prefabs/Missile/PlayerMissile").GetComponent<PlayerMissile>();
	}

	private void Update()
	{
		FireMissile();
	}

	// 미사일을 발사시킵니다.
	private void FireMissile()
	{
		// 발사 위치를 회전
		void RotateFirePosition()
		{
			// 왼쪽, 오른쪽 발사 위치 회전 초기화
			_MissileFireLeftPos.eulerAngles = _MissileFireRightPos.eulerAngles =
				transform.eulerAngles;

			// 왼쪽, 오른쪽 발사 위치 회전
			_MissileFireLeftPos.eulerAngles += Vector3.up * Random.Range(-50.0f, 50.0f);
			_MissileFireRightPos.eulerAngles += Vector3.up * Random.Range(-50.0f, 50.0f);
		}

		// 미사일 오브젝트를 생성합니다.
		PlayerMissile CreateMissileObject() => 
			_PlayerMissilePool.GetRecycleObject() ??
			_PlayerMissilePool.RegisterRecyclableObject(Instantiate(_PlayerMissilePrefab));



		// 공격용 조이스틱을 얻습니다.
		var attackJoystick = (_PlayerableCharacter.playerController as PlayerController).attackJoystick;

		// 공격 입력 확인
		if (!attackJoystick.isInput) return;


		// 미사일 딜레이만큼 시간이 지나지 않았다면 발사 취소
		if (Time.time - _LastFiredTime < _MissileDelay) return;
		_LastFiredTime = Time.time;



		// 미사일 개수만큼 발사시킵니다.
		for (int i = 0; i < _MissileCount; ++i )
		{
			// 발사 위치 회전
			RotateFirePosition();

			// 미사일 생성
			var newLeftPlayerMissile = CreateMissileObject();
			var newRightPlayerMissile = CreateMissileObject();

			newLeftPlayerMissile.Fire(_MissileFireLeftPos.position,
				_MissileFireLeftPos.forward, 10);

			newRightPlayerMissile.Fire(_MissileFireRightPos.position,
				_MissileFireRightPos.forward, 10);
		}
	}


}
