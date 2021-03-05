using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public sealed class PathFixer : EditorWindow
// - EditorWindow : 에디터에서 사용할 수 있는 창 객체를 만들기 위한 형식
{
	public string path { get; set; } = "Input Path...";

	[MenuItem("Window/CustomUtil/PathFixer")]
	private static void Init()
	{
		PathFixer window = EditorWindow.GetWindow(typeof(PathFixer)) as PathFixer;
		window.minSize = new Vector2(20, 100);
	}

	private void OnGUI()
	{
		// 라벨
		GUILayout.Label("Base Path", EditorStyles.boldLabel);

		path = EditorGUILayout.TextField("Input Path", path);

		GUILayout.Button("Convert");
	}
}
