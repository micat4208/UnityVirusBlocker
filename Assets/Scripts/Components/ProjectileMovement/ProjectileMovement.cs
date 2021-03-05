using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 투사체 이동을 구현하는 컴포넌트입니다.
public sealed class ProjectileMovement:
	MonoBehaviour
{
	// 투사체의 속력
	public float projectileSpeed { get; set; }

	// 투사체의 이동 방향
	public Vector3 projectileDirection { get; set; }

	// 투사체의 반지름
	public float projectileRadius { get; set; }

	private void Update()
	{
		MoveProjectile();
	}

	// 투사체를 이동시킵니다.
	private void MoveProjectile()
	{
		transform.Translate(
			projectileDirection * projectileSpeed * Time.deltaTime, Space.Self);
	}

}
