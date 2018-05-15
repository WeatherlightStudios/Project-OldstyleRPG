using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
[ExecuteInEditMode]
public class QuadSpawner : MonoBehaviour {

	[Range(1,100)]
	public int textureSizeX;
	[Range(1,100)]
	public int textureSizeY;
	[Range(0,100)]
	public int tileX;
	[Range(0,100)]
	public int tileY;
	
	public Vector3 right;
	public Vector3 forward;
	
	public bool createQuad = false;
	
	
	void Update () {
		if(createQuad){
			createQuad = false;
			Mesh quad = CreateQuad.createMesh(right, forward, tileX, tileY, new Vector2(textureSizeX, textureSizeY));
			gameObject.GetComponent<MeshFilter>().mesh = quad;
		}
	}
}
