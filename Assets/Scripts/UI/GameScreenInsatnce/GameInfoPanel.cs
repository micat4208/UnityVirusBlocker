using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameInfoPanel : MonoBehaviour
{
	[SerializeField] private Text WaveText;

	public void SetWaveText(int wave) =>
		WaveText.text = $"Wave {wave}";
}
