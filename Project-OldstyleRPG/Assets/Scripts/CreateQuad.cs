using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreateQuad {
	
	public static Mesh createMesh(Vector3 _right, Vector3 _up, int row, int column, Vector2 textureSize){
		Vector3 right = _right.normalized;
		Vector3 up = _up.normalized;
		
		Mesh newMesh = new Mesh();
		
		Vector3[] verts = new Vector3[4];
		int[] tris = new int[6];
		Vector2[] uvs = new Vector2[4];
		
		verts[0] = Vector3.zero;
		verts[1] = right;
		verts[2] = up;
		verts[3] = right + up;
		
		tris[0] = 0;
		tris[1] = 2;
		tris[2] = 3;
		tris[3] = 0;
		tris[4] = 3;
		tris[5] = 1;
		
		float minCell_x = (row)/textureSize.x;
		float maxCell_x = (row+1)/textureSize.x;
		float minCell_y = (column)/textureSize.y;
		float maxCell_y = (column+1)/textureSize.y;
		
		uvs[0] = new Vector2(minCell_x, minCell_y);
		uvs[1] = new Vector2(maxCell_x, minCell_y);
		uvs[2] = new Vector2(minCell_x, maxCell_y);
		uvs[3] = new Vector2(maxCell_x, maxCell_y);
		
		newMesh.vertices = verts;
		newMesh.triangles = tris;
		newMesh.uv = uvs;
		newMesh.RecalculateNormals();
		
		return newMesh;
	}
	
}
