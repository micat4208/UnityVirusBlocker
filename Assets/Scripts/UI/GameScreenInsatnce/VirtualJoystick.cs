using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class VirtualJoystick : MonoBehaviour,
	IDragHandler, IPointerDownHandler, IPointerUpHandler
/// - IDragHandler : 드래깅 콜백을 받기 위해 구현해야 하는 인터페이스
{
	// 사용되는 이미지 컴포넌트
	[Header("Joystick")]
	[SerializeField] private Image _JoystickThumbImage;

	// 조이스틱 축 값을 나타냅니다.
	public Vector2 inputAxis { get; private set; }

	// 조이스틱 입력중을 나타냅니다.
	public bool isInput { get; private set; }

	public RectTransform rectTransform => transform as RectTransform;



	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		// 입력된 위치를 저장합니다.
		Vector2 inputPos = eventData.position / GameStatics.screenRatio;

		// 입력된 위치에서 배경 위치를 뺍니다.
		inputPos -= rectTransform.anchoredPosition;

		// 조이스틱 배경 반지름
		float joystickRadius = rectTransform.sizeDelta.x * 0.5f;

		// 조이스틱을 가둡니다.
		inputPos = (Vector2.Distance(Vector2.zero, inputPos) < joystickRadius) ?
			inputPos : inputPos.normalized * joystickRadius;

		// 조이스틱 위치를 설정합니다.
		_JoystickThumbImage.rectTransform.anchoredPosition = inputPos;

		inputAxis = inputPos / joystickRadius;
	}

	void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
	{
		isInput = true;
		(this as IDragHandler).OnDrag(eventData);
	}

	void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
	{
		isInput = false;

		// 조이스틱 위치를 되돌립니다.
		// 입력 값을 (0, 0) 으로 되돌립니다.
		_JoystickThumbImage.rectTransform.anchoredPosition = inputAxis = 
			Vector2.zero;
	}
}
