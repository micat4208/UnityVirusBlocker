using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartupFramework;

public sealed class PlayerableCharacter : PlayerableCharacterBase
{
    public override void OnControllerConnected(PlayerControllerBase connectedController)
	{
		base.OnControllerConnected(connectedController);

		// 추적 타깃을 자신으로 설정합니다.
		(SceneManager.Instance.sceneInstance as GameSceneInstance).trackingCamera.trackingTarget = transform;

		Debug.Log("23was");
	}


}
