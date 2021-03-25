using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Util;

public sealed class GameSceneInstance :
	VBSceneInstance
{
	[Header("시작 전 딜레이")] [Range(0.0f, 10.0f)]
	[SerializeField] private float _StageBeinDelay = 3.0f;

	[Header("스테이지 원 반지름")]
	[SerializeField] private float _StageRadius = 22.0f;


	[SerializeField] private TrackingMovement _TrackingCamera;

	[Header("Wave Info")]
	[SerializeField] private List<StageWaveInfo> _WaveInfos = new List<StageWaveInfo>();

	public TrackingMovement trackingCamera => _TrackingCamera;

	public ObjectPool<ParticleInstance> particlePool { get; private set; } = new ObjectPool<ParticleInstance>();

	private Dictionary<EnemyType, EnemyCharacter> _EnemyPrefabs = new Dictionary<EnemyType, EnemyCharacter>();


	protected override void Awake()
	{
		base.Awake();

		_EnemyPrefabs.Add(EnemyType.Enemy1, ResourceManager.Instance.LoadResource<GameObject>(
			"Enemy1",
			"Prefabs/Characters/EnemyCharacter/Enemy1").GetComponent<EnemyCharacter>());

		_EnemyPrefabs.Add(EnemyType.Enemy2, ResourceManager.Instance.LoadResource<GameObject>(
			"Enemy2",
			"Prefabs/Characters/EnemyCharacter/Enemy2").GetComponent<EnemyCharacter>());

		_EnemyPrefabs.Add(EnemyType.Enemy3, ResourceManager.Instance.LoadResource<GameObject>(
			"Enemy3",
			"Prefabs/Characters/EnemyCharacter/Enemy3").GetComponent<EnemyCharacter>());

		StartStage();
	}

	// 스테이지를 시작시킵니다.
	private void StartStage()
	{
		// 스포너를 시작시킵니다.
		IEnumerator StartSpawner()
		{
			// 현재 웨이브를 나타냅니다.
			int currentWave = 0;

			// 생성한 적을 저장할 리스트
			List<EnemyCharacter> spawnedEnemies = new List<EnemyCharacter>();

			// 해당 웨이브가 끝날 때까지 대기합니다.
			WaitUntil waitWaveFin = new WaitUntil(() => spawnedEnemies.Count == 0);

			foreach (var waveInfo in _WaveInfos)
			{
				// 적을 생성합니다.
				SpawnEnemy(waveInfo, spawnedEnemies);

				var gameScreenInst = (PlayerManager.Instance.playerController.screenInstance as GameScreenInsatnce);
				gameScreenInst.gameInfoPanel.SetWaveText(currentWave + 1);

				// 웨이브가 끝날 때까지 대기합니다.
				yield return waitWaveFin;

				// 웨이브 카운트 증가
				++currentWave;

			}
		}

		// 스테이지 시작 전 딜레이를 줍니다.
		IEnumerator StartBeiginDelay()
		{
			WaitForSeconds waitSec = new WaitForSeconds(_StageBeinDelay);
			yield return waitSec;

			// 스테이지 시작
			StartCoroutine(StartSpawner());
		}

		StartCoroutine(StartBeiginDelay());
	}

	// 적을 생성합니다.
	private void SpawnEnemy(StageWaveInfo stageInfo, List<EnemyCharacter> spawnedEnemies)
	{
		// 원 가장자리 위치를 반환합니다.
		Vector3 GetRandomSpawnPosition() =>
			new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)).normalized * _StageRadius;

		// 적을 특정한 위치에 생성합니다.
		EnemyCharacter SpawnEnemy(EnemyType enemyType)
		{
			var newEnemyCharacter = Instantiate(_EnemyPrefabs[enemyType]);

			newEnemyCharacter.transform.position = GetRandomSpawnPosition();

			spawnedEnemies.Add(newEnemyCharacter);

			newEnemyCharacter.onEnemyCharacterDie += 
				() => spawnedEnemies.Remove(newEnemyCharacter);


			return newEnemyCharacter;
		}

		for (int i = 0; i < stageInfo.enemyType1; ++i)
			SpawnEnemy(EnemyType.Enemy1);
		for (int i = 0; i < stageInfo.enemyType2; ++i)
			SpawnEnemy(EnemyType.Enemy2);
		for (int i = 0; i < stageInfo.enemyType3; ++i)
			SpawnEnemy(EnemyType.Enemy3);
	}


	public ParticleInstance GetParticleInstance(ParticleInstanceType particleInstType)
	{
		// 매개 변수 particleInstType 에 전달된 타입과 같은 타입의 ParticleInstance 를 찾아 반환합니다.
		return particlePool.GetRecycleObject(
			(obj) => 
			obj.particleInstanceType == particleInstType && 
			obj.canRecyclable);
	}





}
