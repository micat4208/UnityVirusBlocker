using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어 캐릭터 공격 기능을 담당하는 컴포넌트입니다.
public sealed class PlayerCharacterAttack : MonoBehaviour
{
	[Header("미사일 발사 위치")]
	[SerializeField] private Transform _MissileFireLeftPos;
	[SerializeField] private Transform _MissileFireRightPos;

	// 플레이어 캐릭터
	private PlayerableCharacter _PlayerableCharacter;

	// 미사일 오브젝트 프리팹
	private PlayerMissile _PlayerMissilePrefab;

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
		PlayerMissile CreateMissileObject() => Instantiate(_PlayerMissilePrefab);



		// 공격용 조이스틱을 얻습니다.
		var attackJoystick = (_PlayerableCharacter.playerController as PlayerController).attackJoystick;

		// 공격 입력 확인
		if (!attackJoystick.isInput) return;

		// 미사일 생성
		var newLeftPlayerMissile = CreateMissileObject();
		var newRightPlayerMissile = CreateMissileObject();

		newLeftPlayerMissile.Fire(_MissileFireLeftPos.position,
			_MissileFireLeftPos.forward, 10);

		newRightPlayerMissile.Fire(_MissileFireRightPos.position,
			_MissileFireRightPos.forward, 10);


	}


}
