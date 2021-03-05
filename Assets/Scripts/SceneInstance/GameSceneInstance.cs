using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameSceneInstance : 
	SceneInstance
{
	[SerializeField] private TrackingMovement _TrackingCamera;

	public TrackingMovement trackingCamera => _TrackingCamera;



}
