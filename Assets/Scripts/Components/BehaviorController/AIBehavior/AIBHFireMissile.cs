using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Util;

public sealed class AIBHFireMissile:
	AIBehaviorBase
{
	[Header("발사시킬 미사일 개수")]
	[Range(1, 20)]
	[SerializeField] private int _MissileCount = 8;

	[Header("미사일 발사 위치")]
	[SerializeField] private Transform _MissileFirePosition;

	private EnemyMissile _EnemyMissilePrefab;

	private EnemyCharacter _EnemyCharacter;

	private ObjectPool<EnemyMissile> _MissilePool = new ObjectPool<EnemyMissile>();

	protected override void Awake()
	{
		base.Awake();

		_EnemyCharacter = GetComponent<EnemyCharacter>();

		_EnemyMissilePrefab = ResourceManager.Instance.LoadResource<GameObject>(
			"EnemyMissile",
			"Prefabs/Missile/EnemyMissile").GetComponent<EnemyMissile>();
	}

	public override void Run()
	{
		// 미사일을 발사시킵니다.
		void FireMissile()
		{
			_MissileFirePosition.localEulerAngles = Vector3.zero;

			float addYawAngle = 360.0f / _MissileCount;

			for (int i = 0; i < _MissileCount; ++i)
			{
				// 미사일 생성
				var enemyMissile = _MissilePool.GetRecycleObject() ??
					_MissilePool.RegisterRecyclableObject(Instantiate(_EnemyMissilePrefab));

				_MissileFirePosition.localEulerAngles = Vector3.up * (addYawAngle * i);

				// 미사일 발사
				enemyMissile.Fire(_EnemyCharacter, _MissileFirePosition.position, _MissileFirePosition.forward);
			}
		}

		FireMissile();

		behaviorFinished = true;
	}

	private void OnDestroy()
	{
		foreach(var missile in _MissilePool.poolObjects)
		{
			missile.DisableMissile();
			Destroy(missile);
		}

		_MissilePool.poolObjects.Clear();
	}
}