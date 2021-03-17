using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework;

[RequireComponent(typeof(ProjectileMovement))]
public class EnemyMissile : MonoBehaviour,
	IObjectPoolable
{
	[Header("미사일 속력")]
	[SerializeField] private float _MissileSpeed = 2.0f;

	private ProjectileMovement _ProjectileMovement;

	// 초기 위치를 나타냅니다.
	private Vector3 _InitialPosition;

	public bool canRecyclable { get; set; }
	public Action onRecycleStartEvent { get; set; }

	private void Awake()
	{
		_ProjectileMovement = GetComponent<ProjectileMovement>();
	}

	public void Fire(Vector3 initialPosition, Vector3 direction)
	{
		transform.position = _InitialPosition = initialPosition;

		_ProjectileMovement.projectileDirection = direction;
		_ProjectileMovement.projectileSpeed = _MissileSpeed;
	}


}
