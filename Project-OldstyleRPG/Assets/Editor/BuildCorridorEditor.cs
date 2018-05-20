using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenericCorridorBuilder))]
public class BuildCorridorEditor : Editor {
	public override void OnInspectorGUI(){
		base.OnInspectorGUI();
		
		GenericCorridorBuilder roomBuilder = (GenericCorridorBuilder)target;
		
		GUILayout.BeginHorizontal();
		
		if(GUILayout.Button("Generate Corridor")){
			roomBuilder.info = new CorridorInfo(roomBuilder.debugPosition, roomBuilder.debugSize, roomBuilder.debugHeight);
			roomBuilder.DeleteOld();
			roomBuilder.BuildCorridor();
		}
		
		if(GUILayout.Button("Delete Childrens")){
			roomBuilder.DeleteOld();
		}
		
		GUILayout.EndHorizontal();
	}
}
