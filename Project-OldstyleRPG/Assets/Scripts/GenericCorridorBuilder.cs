using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenericCorridorBuilder : MonoBehaviour {
	
	//public List<GameObject> oldObjs;
	
	public GameObject floorTile;
	public GameObject wallTile;
	public GameObject ceilingTile;
	
	public CorridorInfo info;
	
	public Vector3 debugPosition;
	public Vector2 debugSize = Vector2.one;
	public int debugHeight = 4;
	
	// Use this for initialization
	void Start () {
		DeleteOldSafe();
		if(info != null)
			BuildCorridor();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void BuildCorridor(){
		transform.position = info.position;
		
		/// <summary>
		/// Ground
		/// </summary>
		//GameObject ground = Instantiate(floorTile, info.position, Quaternion.identity, this.transform);
		//ground.GetComponentInChildren<MeshRenderer>().size = info.size;
		
		
		GameObject ground = Instantiate(floorTile, info.position, Quaternion.identity, this.transform);
		
		MeshRenderer rendGround = ground.GetComponentInChildren<MeshRenderer>();
		Material tempMat = new Material(rendGround.sharedMaterial);
		tempMat.mainTextureScale = info.size;
		rendGround.sharedMaterial = tempMat;
		//print("size: " + info.size + " pos: " + info.position);
		ground.transform.localScale = new Vector3(info.size.x, 1 , info.size.y);
		
		/// <summary>
		/// Walls
		/// </summary>
		//4 walls positions = position + 4 directions * size/2
		Vector3 wall1 = info.position + Vector3.up * info.height/2;
		Vector3 wall2 = info.position + Vector3.up * info.height/2;
		Vector3 wallDir = Vector3.zero;
		Vector2 wallSize = Vector3.one;
		Vector2 wallScale = Vector3.one;
		
		if(info.size.y <= 1){
			//wall from left to right
			wall1 += Vector3.forward * info.size.y/2;
			wall2 += Vector3.back * info.size.y/2;
			wallDir = Vector3.forward;
			wallSize = new Vector2(info.size.x, info.height);
			wallScale = new Vector3(info.size.x, info.height, 1);
		}
		else if(info.size.x <= 1){
			//wall from forward to back
			wall1 += Vector3.right * info.size.x / 2;
			wall2 += Vector3.left * info.size.x / 2;
			wallDir = Vector3.right;
			wallSize = new Vector2(info.size.y, info.height);
			wallScale = new Vector3(info.size.y, info.height, 1);
		}
		
		
		GameObject wall1Obj = Instantiate(wallTile, wall1, Quaternion.LookRotation(wallDir, Vector3.up), this.transform);
		GameObject wall2Obj = Instantiate(wallTile, wall2, Quaternion.LookRotation(wallDir * -1, Vector3.up), this.transform);
		
		//wall1Obj.GetComponentInChildren<SpriteRenderer>().size = wallSize;
		//wall2Obj.GetComponentInChildren<SpriteRenderer>().size = wallSize;
		/*
		Vector2 sizeLR = new Vector2(info.size.y, info.height);
		Vector2 sizeFB = new Vector2(info.size.x, info.height);
		Vector3 scaleFB = new Vector3(info.size.x, info.height, 1);
		Vector3 scaleLR = new Vector3(info.size.y, info.height, 1);
		 */
		
		wall1Obj.transform.localScale = wallScale;
		MeshRenderer rendWall1 = wall1Obj.GetComponentInChildren<MeshRenderer>();
		tempMat = new Material(rendWall1.sharedMaterial);
		tempMat.mainTextureScale = wallSize;
		rendWall1.sharedMaterial = tempMat;
		
		wall2Obj.transform.localScale = wallScale;
		wall2Obj.GetComponentInChildren<MeshRenderer>().sharedMaterial = tempMat;
		
	}
	
	public void DeleteOld(){
		while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
	}
	
	
	private void DeleteOldSafe(){
		for(int i=0; i < transform.childCount; i++){
			Destroy(transform.GetChild(i).gameObject);
		}
	}
}

public class CorridorInfo{
	public Vector3 position;
	public Vector2 size;
	public int height = 2;
	public CorridorInfo(Vector3 position, Vector2 size, int height){
		this.position = position;
		this.size = size;
		this.height = height;
	}
}
