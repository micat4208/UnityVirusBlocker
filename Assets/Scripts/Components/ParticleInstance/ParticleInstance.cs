using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework;

[RequireComponent(typeof(ParticleSystem))]
public sealed class ParticleInstance:
	MonoBehaviour, IObjectPoolable
{
	public ParticleInstanceType particleInstanceType { get; set; }

	public bool canRecyclable { get; set; }
	public Action onRecycleStartEvent { get; set; }

	public new ParticleSystem particleSystem { get; private set; }

	private WaitUntil _WaitParticlePlayFin;

	private void Awake()
	{
		particleSystem = GetComponent<ParticleSystem>();
		_WaitParticlePlayFin = new WaitUntil(() => particleSystem.isPlaying);
	}

	// 파티클을 재생 시킵니다.
	public void PlayParticle()
	{
		particleSystem.Play();

		StartCoroutine(WaitParticlePlayFin());
	}

	// 파티클 재생 끝을 대기합니다.
	private IEnumerator WaitParticlePlayFin()
	{
		// 파티클 재생이 끝날 때까지 대기합니다.
		yield return _WaitParticlePlayFin;

		// 재사용 가능한 상태로 변경합니다.
		canRecyclable = true;
	}
}
