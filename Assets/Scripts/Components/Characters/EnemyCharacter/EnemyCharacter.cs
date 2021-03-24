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

	private ParticleInstance _CharacterDieParticlePrefab;
	private AudioClip _EnemyCharacterDieSound;

	public new Collider collider { get; private set; }
	public BehaviorController behaviorController { get; private set; }
	public float maxHp => _MaxHp;
	public float hp => _Hp;

	// 적이 죽을 경우 호출되는 대리자
	public System.Action onEnemyCharacterDie { get; set; }

	private void Awake()
	{
		_CharacterDieParticlePrefab = ResourceManager.Instance.LoadResource<GameObject>(
			"CharacterDie",
			"Prefabs/ParticleInstances/CharacterDie").GetComponent<ParticleInstance>();

		_EnemyCharacterDieSound = ResourceManager.Instance.LoadResource<AudioClip>(
			"Player_Missile_Hit",
			"Sound/Player_Missile_Hit");

		idCollider = collider = GetComponent<Collider>();
		behaviorController = GetComponent<BehaviorController>();
		gameObject.layer = (LayerMask.NameToLayer("Enemy"));

		OnTakeAnyDamage += (damageCauser, componentCauser, damage) =>
		{
			_Hp -= damage;


			if (_Hp <= 0.0f) OnCharacterDie();
			else AudioManager.Instance.PlayAudio(_EnemyCharacterDieSound, false, 0.3f);
		};
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Player"))
			SceneManager.Instance.sceneInstance.allocatedCharacters[other].ApplyDamage(this, this, 10);
	}

	private void OnCharacterDie()
	{
		GameSceneInstance gameSceneInstance = 
			SceneManager.Instance.sceneInstance as GameSceneInstance;


		var characterDieParticle = gameSceneInstance.GetParticleInstance(ParticleInstanceType.CharacterDie) ??
			gameSceneInstance.particlePool.RegisterRecyclableObject(
				Instantiate(_CharacterDieParticlePrefab));

		characterDieParticle.transform.position = transform.position;
		characterDieParticle.PlayParticle();

		behaviorController.StopBehaivor();

		AudioManager.Instance.PlayAudio(_EnemyCharacterDieSound, false, 0.7f);

		onEnemyCharacterDie?.Invoke();

		Destroy(gameObject);
	}

}
