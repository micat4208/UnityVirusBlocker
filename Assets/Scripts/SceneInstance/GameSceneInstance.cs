using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Util;

public sealed class GameSceneInstance : 
	SceneInstance
{
	[SerializeField] private TrackingMovement _TrackingCamera;

	public TrackingMovement trackingCamera => _TrackingCamera;

	public ObjectPool<ParticleInstance> particlePool { get; private set; } = new ObjectPool<ParticleInstance>();

	public ParticleInstance GetParticleInstance(ParticleInstanceType particleInstType)
	{
		// 매개 변수 particleInstType 에 전달된 타입과 같은 타입의 ParticleInstance 를 찾아 반환합니다.
		return particlePool.GetRecycleObject(
			(obj) => obj.particleInstanceType == particleInstType);
	}



}
