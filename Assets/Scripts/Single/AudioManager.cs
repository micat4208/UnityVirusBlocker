using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartUpFramework.Util;

public sealed class AudioManager : 
    ManagerClassBase<AudioManager>
{
	// 처음 생성해둘 AudioInstance 객체 개수를 나타냅니다.
	[Header("Begin AudioInstance Count")]
	[SerializeField] private int _BeginAudioInstanceCount = 10;

    private AudioInstance _AudioInstancePrefab;

	private ObjectPool<AudioInstance> _AudioInstancePool = new ObjectPool<AudioInstance>();

	public override void InitializeManagerClass()
	{
		_AudioInstancePrefab = ResourceManager.Instance.LoadResource<GameObject>(
			"AudioInstance",
			"Prefabs/AudioInstance/AudioInstance").GetComponent<AudioInstance>();

		// 사용 가능한 AudioInstance 객체를 _BeginAudioInstanceCount 개수만큼 생성합니다.
		for (int i = 0; i < _BeginAudioInstanceCount; ++i)
		{
			var audioInstance = _AudioInstancePool.RegisterRecyclableObject(
				Instantiate(_AudioInstancePrefab));

			// 사용 가능한 상태로 설정합니다.
			audioInstance.canRecyclable = true;

			// Scene 이 변경되어도 생성된 AudioInstance 객체는 제거되지 않도록
			// AudioManager 하위 오브젝트로 추가합니다.
			audioInstance.transform.SetParent(transform);
		}
	}

	// 소리를 재생합니다.
	public AudioInstance PlayAudio(
		AudioClip	audioClip, 
		bool		loop		= false, 
		float		volume		= 1.0f, 
		float		pitch		= 1.0f, 
		AudioType	audioType	= AudioType.Effect)
	{
		AudioInstance audioInstance = null;

		if (audioType == AudioType.Bgm)
		{
			audioInstance = _AudioInstancePool.GetRecycleObject(
				(audioInst) => audioInst.m_AudioType == AudioType.Bgm);

			audioInstance?.StopAudio();
		}

		if (audioInstance == null)
		{
			audioInstance = _AudioInstancePool.GetRecycleObject() ??
				_AudioInstancePool.RegisterRecyclableObject(
					Instantiate(_AudioInstancePrefab));
		}

		audioInstance.transform.SetParent(transform);


		// 소리를 재생합니다.
		return audioInstance.PlayAudio(audioClip, loop, volume, pitch, audioType);
	}


}
