using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenInstance: ScreenInstance
{
	[SerializeField] private Button _StartButton;

	private void Awake()
	{
		_StartButton.onClick.AddListener(() =>
			UnityStartupFramework.SceneManager.Instance.LoadScene("GameScene"));
	}

}
