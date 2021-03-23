using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework;

[RequireComponent(typeof(AudioSource))]
public sealed class AudioInstance:
	MonoBehaviour,
	IObjectPoolable
{

	public AudioType m_AudioType = AudioType.Effect;

	private IEnumerator _WaitPlayFin;
	private WaitUntil _WaitAudioPlayFin;

	public AudioSource audioSource { get; private set; }

	public bool canRecyclable { get; set; }
	public Action onRecycleStartEvent { get; set; }


	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		_WaitAudioPlayFin = new WaitUntil(() => !audioSource.isPlaying);
	}

	public AudioInstance PlayAudio(
		AudioClip clip, 
		bool loop, float volume, float pitch,
		AudioType audioType = AudioType.Effect)
	{
		audioSource.clip = clip;
		audioSource.loop = loop;
		audioSource.volume = volume;
		audioSource.pitch = pitch;
		m_AudioType = audioType;

		// 재사용 불가능한 상태로 설정
		canRecyclable = false;

		StartCoroutine(_WaitPlayFin = WaitPlayFin());

		return this;
	}
	
	public void StopAudio()
	{
		StopCoroutine(_WaitPlayFin);
		_WaitPlayFin = null;

		// 재사용 가능 상태로 설정
		canRecyclable = true;

		// 재생을 멈춥니다.
		audioSource.Stop();
	}

	private IEnumerator WaitPlayFin()
	{
		// 재생 끝을 대기합니다.
		yield return _WaitAudioPlayFin;

		// 재사용 가능 상태로 설정
		canRecyclable = true;
	}
}
