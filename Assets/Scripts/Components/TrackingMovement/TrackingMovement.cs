using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TrackingMovement: 
	MonoBehaviour
{
	[Header("추적 속도")]
	[Header("- Tracking Setting")]
	[SerializeField] private float _TrackingSpeed = 10.0f;

	[Header("추적을 사용할 것인지를 결정합니다.")]
	[SerializeField] private bool _UseTrackingMovement = true;

	[Header("추적타깃 (UseTrackingTargetParent true 일 경우 자동으로 설정됩니다.)")]
	[SerializeField] private Transform _TrackingTarget;

	[Header("부모 오브젝트를 추적 타깃으로 설정합니다.")]
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

	public Transform trackingTarget { get => _TrackingTarget; set => _TrackingTarget = value; }

	private void Awake()
	{
		// 추적 오브젝트를 부모 오브젝트로 설정합니다.
		if (_UseTrackingTargetParent)
			_TrackingTarget = transform.parent;

		// 최상위 오브젝트로 설정합니다.
		if (_IsRootObject)
			transform.SetParent(null);
	}

	private void Update()
	{
		if (_UseTrackingMovement)
		{
			TrackingTarget();
		}
	}

	// 목표 오브젝트를 추적합니다.
	private void TrackingTarget()
	{
		// 타깃이 존재하지 않는 경우 추적을 하지 않습니다.
		if (!_TrackingTarget) return;

		transform.position = Vector3.Lerp(
			transform.position,
			_TrackingTarget.position,
			_TrackingSpeed * Time.deltaTime);
	}

	// 회전값을 목표 오브젝트 회전과 동일하게 설정합니다.
	private void MatchRotationToTarget()
	{
		// 타깃이 존재하지 않는 경우 회전을 하지 않습니다.
		if (!_TrackingTarget) return;

		Vector3 newEulerAngle = transform.eulerAngles;

		if (_AllowMatchPitchRotationToTarget)
			newEulerAngle.x = _TrackingTarget.eulerAngles.x;

		if (_AllowMatchYawRotationToTarget)
			newEulerAngle.y = _TrackingTarget.eulerAngles.y;

		if (_AllowMatchRollRotationToTarget)
			newEulerAngle.z = _TrackingTarget.eulerAngles.z;

		// 회전을 설정합니다.
		transform.eulerAngles = newEulerAngle;
	}



}
