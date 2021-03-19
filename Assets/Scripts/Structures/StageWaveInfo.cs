using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 하나의 스테이지에 사용되는 웨이브를 나타내기 위해 사용되는 구조체
[System.Serializable]
public struct StageWaveInfo
{
	// 스테이지에 생성되는 적 개체 수
	public int enemyType1;
	public int enemyType2;
	public int enemyType3;
}
