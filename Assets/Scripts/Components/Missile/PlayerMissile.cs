using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework;

// 플레이어 캐릭터가 발사하는 미사일을 나타내기 위한 컴포넌트
[RequireComponent(typeof(ProjectileMovement))]
public sealed class PlayerMissile : MonoBehaviour,
	IObjectPoolable
{
	[SerializeField] private Transform _MissileMeshTransform;

	// 미사일 발사 위치
	private Vector3 _InitialPosition;

	// 함께 사용되는 ProjectileMovement Component 를 나타냅니다.
	public ProjectileMovement projectileMovement { get; private set; }

	public bool canRecyclable { get; set; }
	public System.Action onRecycleStartEvent { get; set; }


	private void Awake()
	{
		projectileMovement = GetComponent<ProjectileMovement>();

		// 투사체가 갑지할 오브젝트 레이어를 지정합니다.
		projectileMovement.detectableLayer = (1 << LayerMask.NameToLayer("Enemy"));

		// 투사체 반지름 설정
		projectileMovement.projectileRadius = 0.1f;

		// 투사체 겹침시 실행할 내용을 정의합니다.
		projectileMovement.onProjectileOverlapped += (Collider collider) =>
		{
			Debug.Log(collider.gameObject.name);
		};
	}

	private void Update()
	{
		// 미사일이 이동한 거리를 확인합니다.
		if (Vector3.Distance(_InitialPosition, transform.position) > 10.0f)
			DisableMissile();

		RotatingMissileMesh();
	}

	// 미사일 오브젝트를 비활성화 시킵니다.
	private void DisableMissile()
	{
		// 오브젝트를 재활용 가능한 상태로 설정합니다.
		canRecyclable = true;

		// 오브젝트 비활성화
		gameObject.SetActive(false);
	}

	// 미사일의 Yaw 회전을 구현합니다.
	private void RotatingMissileMesh() =>
		_MissileMeshTransform.eulerAngles += Vector3.up * 1440.0f * Time.deltaTime;

	// 미사일을 발사시킵니다.
	/// - initialPosition : 미사일 오브젝트의 초기 위치를 전달합니다.
	/// - direction : 투사체 방향을 전달합니다.
	/// - speed : 투사체 속력을 전달합니다.
	public void Fire(Vector3 initialPosition, Vector3 direction, float speed)
	{
		// 오브젝트 위치를 초기 위치로 설정합니다.
		transform.position = _InitialPosition = initialPosition;

		// 오브젝트 활성화
		gameObject.SetActive(true);

		// 투사체 이동 컴포넌트의 내용을 초기화합니다.
		projectileMovement.projectileDirection = direction;
		projectileMovement.projectileSpeed = speed;
	}

}
