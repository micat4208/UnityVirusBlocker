using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TrackingCamera : 
	MonoBehaviour
{
	[Header("추적 속도")]
	[Header("- Tracking Setting")]
	[SerializeField] private float _TrackingSpeed = 10.0f;

	[Header("추적을 사용할 것인지를 결정합니다.")]
	[SerializeField] private bool _UseTrackingMovement = true;

	[Header("추적타깃 (UseTrackingTargetParent true 일 경우 자동으로 설정됩니다.)")]
	[SerializeField] private Transform _TrackingTarget;

	[Header("부모 오브젝트를 추적 타깃을 설정합니다.")]
	[SerializeField] private bool _UseTrackingTargetParent = false;

	[Header("이 오브젝트를 최상위로")]
	[Header("- Default Setting")]
	[Space(40.0f)]
	[SerializeField] private bool _IsRootObject = false;

	[Header("타깃 회전과 Yaw 회전값을 일치시킵니다.")]
	[Header("- Rotation Setting")]
	[Space(40.0f)]
	[SerializeField] private bool _AllowMatchYawRotationToTarget = false;
	[Header("타깃 회전과 Pitch 회전값을 일치시킵니다.")]
	[SerializeField] private bool _AllowMatchPitchRotationToTarget = false;
	[Header("타깃 회전과 Roll 회전값을 일치시킵니다.")]
	[SerializeField] private bool _AllowMatchRollRotationToTarget = false;

	private void Awake()
	{
		// 추적 오브젝트를 부모 오브젝트로 설정합니다.
		if (_UseTrackingTargetParent)
			_TrackingTarget = transform.parent;

		// 최상위 오브젝트로 설정합니다.
		if (_IsRootObject)
			transform.SetParent(null);
	}



}
