using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartupFramework;

public sealed class PlayerableCharacter : PlayerableCharacterBase, 
	HealthPointable
{
	[Header("무적 시간")]
	[SerializeField] private float _InvincibleTime = 1.0f;

	// 캐릭터 최대 체력을 나타냅니다.
	[SerializeField] private float _MaxHp = 100.0f;
	// 캐릭터 체력을 나타냅니다.
	[SerializeField] private float _Hp = 100.0f;

	// 마지막 무적 시간
	private float _LastInvincibleTime;

	private ParticleInstance _DieParticlePrefab;
	private ParticleInstance _EnemyMissileHitPrefab;

	private AudioClip _PlayerDieSound;
	private AudioClip _PlayerDamageSound;

	public float maxHp => _MaxHp;
	public float hp => _Hp;

	public float maxAttackStemina { get; set; } = 30.0f;
	public float attackStemina { get; set; } = 30.0f;

	private void Awake()
	{
		_DieParticlePrefab = ResourceManager.Instance.LoadResource<GameObject>(
			"CharacterDie",
			"Prefabs/ParticleInstances/CharacterDie").GetComponent<ParticleInstance>();

		_EnemyMissileHitPrefab = ResourceManager.Instance.LoadResource<GameObject>(
			"EnemyMissileHit",
			"Prefabs/ParticleInstances/EnemyMissileHit").GetComponent<ParticleInstance>();

		_PlayerDieSound = ResourceManager.Instance.LoadResource<AudioClip>(
			"Player_Missile_Hit",
			"Sound/Player_Missile_Hit");

		_PlayerDamageSound = ResourceManager.Instance.LoadResource<AudioClip>(
			"GetDamaged",
			"Sound/GetDamaged");


		idCollider = GetComponent<CharacterController>();
		tag = "Player";

		// 플레이어블 캐릭터가 피해를 입었을 경우 실행할 내용 정의
		OnTakeAnyDamage += (damageCauser, componentCauser, damage) =>
		{
			// 무적 상태라면 피해 무시
			if (Time.time - _LastInvincibleTime < _InvincibleTime) return;

			// 마지막으로 피해를 입은 시간을 저장합니다.
			_LastInvincibleTime = Time.time;

			var sceneInstance = SceneManager.Instance.sceneInstance as GameSceneInstance;

			// 파티클 인스턴스 생성
			var hitParticle = sceneInstance.GetParticleInstance(ParticleInstanceType.EnemyMissileHit) ??
				sceneInstance.particlePool.RegisterRecyclableObject(
					Instantiate(_EnemyMissileHitPrefab));

			// 파티클 인스턴스 위치 설정
			hitParticle.transform.position = (componentCauser??this).transform.position;

			// 파티클 재생
			hitParticle.PlayParticle();


			// 피해량만큼 체력 감소
			_Hp -= damage;

			// 체력이 0 이하가 된다면 사망
			if (_Hp <= 0.0f)
			{
				_Hp = 0.0f;

				// 사망
				OnCharacterDie();
			}
			else AudioManager.Instance.PlayAudio(_PlayerDamageSound, false, 0.3f);
		};
	}

	protected override void Update()
	{
		base.Update();
	}

	public override void OnControllerConnected(PlayerControllerBase connectedController)
	{
		base.OnControllerConnected(connectedController);

		// 추적 타깃을 자신으로 설정합니다.
		(SceneManager.Instance.sceneInstance as GameSceneInstance).trackingCamera.trackingTarget = transform;
	}

	// 캐릭터 사망 시 호출되는 메서드
	private void OnCharacterDie()
	{
		GameSceneInstance gameSceneInstance =
			SceneManager.Instance.sceneInstance as GameSceneInstance;

		var characterDieParticle =
			gameSceneInstance.GetParticleInstance(ParticleInstanceType.CharacterDie) ??
			gameSceneInstance.particlePool.RegisterRecyclableObject(
				Instantiate(_DieParticlePrefab));

		characterDieParticle.transform.position = transform.position;
		characterDieParticle.PlayParticle();

		AudioManager.Instance.PlayAudio(_PlayerDieSound, false, 0.3f);

		PlayerManager.Instance.playerController.ClearPlayerableCharacter();
		Destroy(gameObject);

	}
}
