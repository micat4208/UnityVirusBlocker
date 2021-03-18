using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartupFramework;
using UnityStartUpFramework;

[RequireComponent(typeof(ProjectileMovement))]
public class EnemyMissile : MonoBehaviour,
	IObjectPoolable
{
	[Header("미사일 속력")]
	[SerializeField] private float _MissileSpeed = 2.0f;

	private ProjectileMovement _ProjectileMovement;

	private ParticleInstance _EnemyMissileHitPrefab;

	// 초기 위치를 나타냅니다.
	private Vector3 _InitialPosition;

	// 이 미사일을 발사한 적 캐릭터
	private EnemyCharacter _EnemyCharacter;

	private WaitUntil waitMissileDisable;

	public bool canRecyclable { get; set; }
	public Action onRecycleStartEvent { get; set; }

	private void Awake()
	{
		_EnemyMissileHitPrefab = ResourceManager.Instance.LoadResource<GameObject>(
			"EnemyMissileHit",
			"Prefabs/ParticleInstances/EnemyMissileHit").GetComponent<ParticleInstance>();

		_ProjectileMovement = GetComponent<ProjectileMovement>();
		waitMissileDisable = new WaitUntil(() => (Vector3.Distance(_InitialPosition, transform.position) > 30.0f));

		_ProjectileMovement.detectableLayer = 1 << LayerMask.NameToLayer("PlayerableCharacter");
		_ProjectileMovement.onProjectileOverlapped += (collider, projectilePosition) =>
		{
			var sceneInstance = SceneManager.Instance.sceneInstance as GameSceneInstance;

			// 플레이어블 캐릭터에게 피해를 가합니다.
			sceneInstance.allocatedCharacters[collider].ApplyDamage(
				_EnemyCharacter, this, 20.0f);

			// 파티클 인스턴스 생성
			var hitParticle = sceneInstance.GetParticleInstance(ParticleInstanceType.EnemyMissileHit) ??
				sceneInstance.particlePool.RegisterRecyclableObject(
					Instantiate(_EnemyMissileHitPrefab));

			// 파티클 인스턴스 위치 설정
			hitParticle.transform.position = projectilePosition;

			// 파티클 재생
			hitParticle.PlayParticle();

			// 미사일 비활성화
			DisableMissile();
		};
	}

	private void DisableMissile()
	{
		canRecyclable = true;
		gameObject.SetActive(false);
	}

	public void Fire(EnemyCharacter enemyCharacter, Vector3 initialPosition, Vector3 direction)
	{
		IEnumerator WaitMissileDisable()
		{
			yield return waitMissileDisable;

			DisableMissile();
		}

		gameObject.SetActive(true);

		_EnemyCharacter = enemyCharacter;

		transform.position = _InitialPosition = initialPosition;

		_ProjectileMovement.projectileDirection = direction;
		_ProjectileMovement.projectileSpeed = _MissileSpeed;

		StartCoroutine(WaitMissileDisable());
	}





}
