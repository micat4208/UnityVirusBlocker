using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartupFramework;

public sealed class PlayerableCharacter : PlayerableCharacterBase, 
	HealthPointable
{
	// 캐릭터 최대 체력을 나타냅니다.
	[SerializeField] private float _MaxHp = 100.0f;
	// 캐릭터 체력을 나타냅니다.
	[SerializeField] private float _Hp = 100.0f;

	public float maxHp => _MaxHp;
	public float hp => _Hp;

	private void Awake()
	{
		idCollider = GetComponent<CharacterController>();
	}

	protected override void Update()
	{
		base.Update();

		Debug.Log("allocated Character Count = " +
			SceneManager.Instance.sceneInstance.allocatedCharacters.Count);
	}

	public override void OnControllerConnected(PlayerControllerBase connectedController)
	{
		base.OnControllerConnected(connectedController);

		// 추적 타깃을 자신으로 설정합니다.
		(SceneManager.Instance.sceneInstance as GameSceneInstance).trackingCamera.trackingTarget = transform;
	}
}
