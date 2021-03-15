using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 투사체 이동을 구현하는 컴포넌트입니다.
public sealed class ProjectileMovement:
	MonoBehaviour
{
	// 투사체의 속력
	public float projectileSpeed { get; set; }

	// 투사체의 이동 방향
	public Vector3 projectileDirection { get; set; }

	// 투사체의 반지름
	public float projectileRadius { get; set; }

	// 투사체 충동을 감지할 레이어
	public int detectableLayer { get; set; } = 1;

	// 투사체와 다른 오브젝트간의 겹침이 발생했을 경우 호출되는 대리자
	public System.Action<Collider> onProjectileOverlapped { get; set; }

	private void Update()
	{
		MoveProjectile();

		if (onProjectileOverlapped != null)
			ProjectileOverlapCheck();
	}

	// 투사체를 이동시킵니다.
	private void MoveProjectile()
	{
		transform.Translate(
			projectileDirection * projectileSpeed * Time.deltaTime, Space.Self);
	}

	// 투사체 겹침을 확인하는 메서드
	private void ProjectileOverlapCheck()
	{
		/* 오브젝트를 감지하는 방법 2 가지
		 * 
		 * OnTrigger / Collision Enter, Stay, Exit 이용한 감지
		 * 방법 : OnTriggerEnter, Exit 메서드 정의
		 * 
		 * 장점
		 *   꽤 간단하게 처리할 수 있다.
		 *   드는 비용이 싸다
		 * 
		 * 단점
		 *   만약 피충돌체중 하나라도 너무 빠르게 이동한다면 감지가 불가능해짐.
		 *   
		 * 결론
		 *   두 피충돌체가 느리게 이동하거나, 이동이 없을 경우 적합
		 * 
		 * Raycast, SphereCast ... 을 이용한 감지
		 * Physics 에서 제공하는 메서드 사용
		 * 
		 * 장점
		 *   피충돌체가 아무리 빠르게 이동해도 정확하게 감지
		 *   
		 * 단점
		 *   OnTrigger / Collision Enter, Stay, Exit 방식보다 조금 복잡하며, 
		 *   드는 비용이 비교적 비싸다
		 *   
		 * 결론
		 *   두 피충돌체중 하나라도 빠르게 이동할 경우 적합
		 */


		// 레이 정보를 저장할 변수를 선언합니다.
		Ray ray = new Ray(transform.position, projectileDirection);

		// ray.origin 을 기준으로 projectileRadius 내에 겹친 오브젝트가 존재하는지 검사합니다.
		var colliders = Physics.OverlapSphere(ray.origin, projectileRadius, detectableLayer);

		// 어떠한 오브젝트를 감지했다면
		if (colliders.Length > 0)
		{
			// 감지한 첫 번째 객체를 전달합니다.
			onProjectileOverlapped?.Invoke(colliders[0]);
			return;
		}

		// 감지 결과를 저장할 변수
		RaycastHit hit;

		// SphereCast 를 진행합니다.
		/// - 감지된 객체가 존재한다면
		if (Physics.SphereCast(ray.origin, projectileRadius,
			ray.direction, out hit, projectileSpeed * Time.deltaTime, detectableLayer))
		{
			// 감지한 객체를 전달합니다.
			onProjectileOverlapped?.Invoke(hit.collider);
		}
	}
}
