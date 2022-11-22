using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapController))]
public class MapEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		MapController controller = (MapController)target;

		if(GUILayout.Button("Clear Track"))
			controller.ClearTrack();
	}
}
