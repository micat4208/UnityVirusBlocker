using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStartupFramework;

// 게임 화면 객체를 나타낼 때 사용될 컴폰넌트
public sealed class GameScreenInsatnce : 
    ScreenInstance
{
    [Header("이동을 담당하는 조이스틱")]
    [SerializeField] private VirtualJoystick _MovementJoystick;

    [Header("공격을 담당하는 조이스틱")]
    [SerializeField] private VirtualJoystick _AttackJoystick;

    [Header("GameInfoPanel")]
    [SerializeField] private GameInfoPanel _GameInfoPanel;
    public GameInfoPanel gameInfoPanel => _GameInfoPanel;

    [Header("GameOverPanel")]
    [SerializeField] private GameObject _GameOverPanel;


    public VirtualJoystick movementJoystick => _MovementJoystick;
    public VirtualJoystick attackJoystick => _AttackJoystick;

    public void ShowGameOverPanel()
	{
        IEnumerator Wait3SecAndGoToTitle()
		{
            
            _GameOverPanel.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            SceneManager.Instance.LoadScene("TitleScene");

        }
        StartCoroutine(Wait3SecAndGoToTitle());
    }
}
