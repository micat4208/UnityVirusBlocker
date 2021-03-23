using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameArea : 
	MonoBehaviour
{
	private bool _IsPlayerIn = true;

	private void FixedUpdate()
	{
		if (!_IsPlayerIn) 
			ApplyOusideDamage();
	}

	private void ApplyOusideDamage()
	{
		PlayerManager.Instance.playerController.playerableCharacter?.ApplyDamage(
			null, null, 10.0f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
			_IsPlayerIn = true;
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
			_IsPlayerIn = false;
	}

}
