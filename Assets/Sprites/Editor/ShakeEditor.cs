using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraShake))]
public class ShakeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GUI.enabled = Application.isPlaying;
		CameraShake e = target as CameraShake;
		if (GUILayout.Button("Shake"))
		{
			Debug.Log("Editor buttone pressed");
			e.Shake();
		}
	}
}
