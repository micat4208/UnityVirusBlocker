using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public sealed class EnemyCharacter : CharacterBase, HealthPointable
{
	// 적 캐릭터 최대 체력을 나타냅니다.
	[SerializeField] private float _MaxHp = 100.0f;
	// 적 캐릭터 체력을 나타냅니다.
	[SerializeField] private float _Hp = 100.0f;

	public new Collider collider { get; private set; }

	public float maxHp => _MaxHp;
	public float hp => _Hp;

	private void Awake()
	{
		idCollider = collider = GetComponent<Collider>();
	}

}
