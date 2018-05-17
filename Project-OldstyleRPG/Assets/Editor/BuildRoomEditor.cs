﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenericRoomBuilder))]
public class BuildRoomEditor : Editor {
	public override void OnInspectorGUI(){
		base.OnInspectorGUI();
		
		GenericRoomBuilder roomBuilder = (GenericRoomBuilder)target;
		
		GUILayout.BeginHorizontal();
		
		if(GUILayout.Button("Generate Room")){
			roomBuilder.info = new RoomInfo(roomBuilder.debugPosition, roomBuilder.debugSize, roomBuilder.debugHeight);
			roomBuilder.DeleteOld();
			roomBuilder.BuildRoom();
		}
		
		if(GUILayout.Button("Empty Childrens")){
			roomBuilder.DeleteOld();
		}
		
		GUILayout.EndHorizontal();
	}
}
