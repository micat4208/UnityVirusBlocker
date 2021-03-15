using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartupFramework;

[RequireComponent(typeof(Collider), typeof(BehaviorController))]
public sealed class EnemyCharacter : CharacterBase, HealthPointable
{
	// 적 캐릭터 최대 체력을 나타냅니다.
	[SerializeField] private float _MaxHp = 100.0f;

	// 적 캐릭터 체력을 나타냅니다.
	[SerializeField] private float _Hp = 100.0f;

	public new Collider collider { get; private set; }
	public BehaviorController behaviorController { get; private set; }

	public float maxHp => _MaxHp;
	public float hp => _Hp;

	private void Awake()
	{
		idCollider = collider = GetComponent<Collider>();
		behaviorController = GetComponent<BehaviorController>();
		gameObject.layer = (LayerMask.NameToLayer("Enemy"));
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
			SceneManager.Instance.sceneInstance.allocatedCharacters[other].ApplyDamage(this, this, 10);
	}

}
