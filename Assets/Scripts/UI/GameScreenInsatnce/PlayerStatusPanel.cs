using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStartupFramework;

public sealed class PlayerStatusPanel : MonoBehaviour
{
	[Header("HP")]
	[SerializeField] private Image _HpBarImage;

	[Header("Attack")]
	[SerializeField] private Image _AttackBarImage;

	public RectTransform rectTransform { get; private set; }

	private PlayerableCharacter _PlayerableCharacter;

	private void Awake()
	{
		rectTransform = transform as RectTransform;
		_PlayerableCharacter = PlayerManager.Instance.playerController.
			playerableCharacter as PlayerableCharacter;
	}

	private void Update()
	{
		UpdateHpbar();
		UpdateAttackSteminabar();

		UpdateScreenPosition();
	}

	private void UpdateScreenPosition()
	{
		// 플레이어가 사망했다면 UI 를 제거합니다.
		if (_PlayerableCharacter == null)
		{
			Destroy(gameObject);
			return;
		}

		var gameSceneInstance = SceneManager.Instance.sceneInstance as GameSceneInstance;
		Camera camera = gameSceneInstance.trackingCamera.GetComponentInChildren<Camera>();

		// 플레이어블 캐릭터의 뷰포트 위치를 얻습니다.
		Vector3 viewportPosition = camera.WorldToViewportPoint(
			_PlayerableCharacter.transform.position);
		/// - WorldToViewportPoint(Vector3 pos) : pos 을 월드 좌표에서 뷰포트 좌표로 변환합니다.

		viewportPosition.x *= (Screen.width  / GameStatics.screenRatio);
		viewportPosition.y *= (Screen.height / GameStatics.screenRatio);

		rectTransform.anchoredPosition = Vector2.Lerp(
			rectTransform.anchoredPosition,
			viewportPosition,
			10.0f * Time.deltaTime);

		Debug.Log("viewportPosition = " + viewportPosition);
	}

	private void UpdateHpbar() =>
		_HpBarImage.fillAmount = 
		_PlayerableCharacter.hp / _PlayerableCharacter.maxHp;

	private void UpdateAttackSteminabar() =>
		_AttackBarImage.fillAmount = 
		_PlayerableCharacter.attackStemina / _PlayerableCharacter.maxAttackStemina;

}
