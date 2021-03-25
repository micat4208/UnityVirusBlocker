using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VBSceneInstance : 
	SceneInstance
{
	[SerializeField] private AudioClip _BgmAudioClip;

	protected virtual void Start()
	{
		AudioManager.Instance.PlayAudio(_BgmAudioClip, 
			true, 
			0.7f, 
			1.0f, 
			AudioType.Bgm);
	}


}
