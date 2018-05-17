using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenericRoomBuilder : MonoBehaviour {
	
	//public List<GameObject> oldObjs;
	
	public GameObject floorTile;
	public GameObject wallTile;
	public GameObject ceilingTile;
	
	public RoomInfo info;
	
	public GameObject quadSpawner;
	
	public Vector3 debugPosition;
	public Vector2 debugSize = Vector2.one;
	public int debugHeight = 4;
	
	// Use this for initialization
	void Start () {
		DeleteOldSafe();
		if(info != null)
			BuildRoom();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void BuildRoom(){
		transform.position = info.position;
		
		/// <summary>
		/// Ground
		/// </summary>
		GameObject ground = Instantiate(floorTile, info.position, Quaternion.identity, this.transform);
		MeshRenderer rendGround = ground.GetComponentInChildren<MeshRenderer>();
		Material tempMat = new Material(rendGround.sharedMaterial);
		tempMat.mainTextureScale = info.size;
		rendGround.sharedMaterial = tempMat;
		
		/*
		MeshRenderer rendGround = ground.GetComponentInChildren<MeshRenderer>();
		Material tempMat = new Material(rendGround.sharedMaterial);
		tempMat.mainTextureScale = info.size;
		rendGround.sharedMaterial = tempMat;
		 */
		
		//print("size: " + info.size + " pos: " + info.position);
		ground.transform.localScale = new Vector3(info.size.x, 1 , info.size.y);
		
		/// <summary>
		/// Walls
		/// </summary>
		//4 walls positions = position + 4 directions * size/2
		Vector3 wall1 = info.position + Vector3.up * info.height/2 + Vector3.forward * info.size.y/2;
		Vector3 wall2 = info.position + Vector3.up * info.height/2 + Vector3.back * info.size.y/2;
		Vector3 wall3 = info.position + Vector3.up * info.height/2 + Vector3.right * info.size.x/2;
		Vector3 wall4 = info.position + Vector3.up * info.height/2 + Vector3.left * info.size.x/2;
		
		Vector2 sizeLR = new Vector2(info.size.y, info.height);
		Vector2 sizeFB = new Vector2(info.size.x, info.height);
		Vector3 scaleFB = new Vector3(info.size.x, info.height, 1);
		Vector3 scaleLR = new Vector3(info.size.y, info.height, 1);
		
		GameObject wall1Obj = Instantiate(wallTile, wall1, Quaternion.LookRotation(Vector3.forward, Vector3.up), this.transform);
		GameObject wall2Obj = Instantiate(wallTile, wall2, Quaternion.LookRotation(Vector3.back, Vector3.up), this.transform);
		GameObject wall3Obj = Instantiate(wallTile, wall3, Quaternion.LookRotation(Vector3.right, Vector3.up), this.transform);
		GameObject wall4Obj = Instantiate(wallTile, wall4, Quaternion.LookRotation(Vector3.left, Vector3.up), this.transform);
		
		
		wall1Obj.transform.localScale = scaleFB;
		MeshRenderer rendWall1 = wall1Obj.GetComponentInChildren<MeshRenderer>();
		tempMat = new Material(rendWall1.sharedMaterial);
		tempMat.mainTextureScale = sizeFB;
		rendWall1.sharedMaterial = tempMat;
		
		wall2Obj.transform.localScale = scaleFB;
		MeshRenderer rendWall2 = wall2Obj.GetComponentInChildren<MeshRenderer>();
		tempMat = new Material(rendWall2.sharedMaterial);
		tempMat.mainTextureScale = sizeFB;
		rendWall2.sharedMaterial = tempMat;
		
		wall3Obj.transform.localScale = scaleLR;
		MeshRenderer rendWall3 = wall3Obj.GetComponentInChildren<MeshRenderer>();
		tempMat = new Material(rendWall3.sharedMaterial);
		tempMat.mainTextureScale = sizeLR;
		rendWall3.sharedMaterial = tempMat;
		
		wall4Obj.transform.localScale = scaleLR;
		MeshRenderer rendWall4 = wall4Obj.GetComponentInChildren<MeshRenderer>();
		tempMat = new Material(rendWall4.sharedMaterial);
		tempMat.mainTextureScale = sizeLR;
		rendWall4.sharedMaterial = tempMat;
		
		/*
		wall1Obj.GetComponentInChildren<SpriteRenderer>().size = sizeFB;
		wall2Obj.GetComponentInChildren<SpriteRenderer>().size = sizeFB;
		wall3Obj.GetComponentInChildren<SpriteRenderer>().size = sizeLR;
		wall4Obj.GetComponentInChildren<SpriteRenderer>().size = sizeLR;
		 */
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

public class RoomInfo{
	public Vector3 position;
	public Vector2 size;
	public int height = 2;
	public RoomInfo(Vector3 position, Vector2 size, int height){
		this.position = position;
		this.size = size;
		this.height = height;
	}
}
