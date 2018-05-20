using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenericRoomBuilder : MonoBehaviour {
	
	//public List<GameObject> oldObjs;
	
	public GameObject floorTile;
	public GameObject wallTile;
	public GameObject ceilingTile;
	
	//public GameObject trigger;
	
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
		
		ground.transform.localScale = new Vector3(info.size.x, 1 , info.size.y);
		
		/// <summary>
		/// Walls
		/// </summary>
		//4 walls positions = position + 4 directions * size/2
		Vector3 wall1 = info.position + Vector3.up * info.height/2 + Vector3.forward * info.size.y/2;
		Vector3 wall2 = info.position + Vector3.up * info.height/2 + Vector3.back * info.size.y/2;
		Vector3 wall3 = info.position + Vector3.up * info.height/2 + Vector3.right * info.size.x/2;
		Vector3 wall4 = info.position + Vector3.up * info.height/2 + Vector3.left * info.size.x/2;
		
		Vector3 scaleFB = new Vector3(info.size.x, info.height, 1);
		Vector3 scaleLR = new Vector3(info.size.y, info.height, 1);
		
		Quaternion rotF = Quaternion.LookRotation(Vector3.forward, Vector3.up);
		Quaternion rotB = Quaternion.LookRotation(Vector3.back, Vector3.up);
		Quaternion rotR = Quaternion.LookRotation(Vector3.right, Vector3.up);
		Quaternion rotL = Quaternion.LookRotation(Vector3.left, Vector3.up);
		
		SpawnWall(wallTile, wall1, rotF, scaleFB);
		SpawnWall(wallTile, wall2, rotB, scaleFB);
		SpawnWall(wallTile, wall3, rotR, scaleLR);
		SpawnWall(wallTile, wall4, rotL, scaleLR);
	}
	
	private void SpawnWall(GameObject toSpawn, Vector3 position, Quaternion rotation, Vector3 size){
		GameObject spawned = Instantiate(toSpawn, position, rotation, this.transform);
		spawned.transform.localScale = size;
		MeshRenderer rend = spawned.GetComponentInChildren<MeshRenderer>();
		Material temp = new Material(rend.sharedMaterial);
		temp.mainTextureScale = size;
		rend.sharedMaterial = temp;
	}
	
	public void SplitWall(GameObject wall, Vector3 pointOfCollision, float wallWidth, bool isHorizontal){
		if(isHorizontal){
			//vertical split
			Vector3 minWallPos = wall.transform.position - Vector3.forward * wall.transform.localScale.x / 2;
			Vector3 maxWallPos = wall.transform.position + Vector3.forward * wall.transform.localScale.x / 2;
			
			Vector3 minSplit = pointOfCollision - Vector3.forward * wallWidth/2;
			Vector3 maxSplit = pointOfCollision + Vector3.forward * wallWidth/2;
			
			Vector3 posB = (minWallPos + minSplit)/2.0f;
			Vector3 posF = (maxSplit + maxWallPos)/2.0f;
			
			Quaternion wallRot = wall.transform.rotation;
			
			float xSizeBack = minSplit.x - minWallPos.x;
			float xSizeForw = maxWallPos.x - maxSplit.x;
			float height = wall.transform.localScale.y;
			
			Vector3 sizeB = new Vector3(xSizeBack, height, 1);
			Vector3 sizeF = new Vector3(xSizeForw, height, 1);
			
			
			SpawnWall(wallTile, posB, wallRot, sizeB);
			SpawnWall(wallTile, posF, wallRot, sizeF);
			
			if(Application.isEditor)
				DestroyImmediate(wall.gameObject);
			else
				Destroy(wall.gameObject);
		}
		else{
			//horizontal split
			Vector3 minWallPos = wall.transform.position - Vector3.right * wall.transform.localScale.x/2;
			Vector3 maxWallPos = wall.transform.position + Vector3.right * wall.transform.localScale.x/2;
			
			Vector3 minSplit = pointOfCollision - Vector3.right * wallWidth/2;
			Vector3 maxSplit = pointOfCollision + Vector3.right * wallWidth/2;
			
			Vector3 posL = (minWallPos + minSplit)/2.0f;
			Vector3 posR = (maxSplit + maxWallPos)/2.0f;
			
			Quaternion wallRot = wall.transform.rotation;
			
			float xSizeLeft = minSplit.x - minWallPos.x;
			float xSizeRight = maxWallPos.x - maxSplit.x;
			float height = wall.transform.localScale.y;
			
			Vector3 sizeL = new Vector3(xSizeLeft, height, 1);
			Vector3 sizeR = new Vector3(xSizeRight, height, 1);
			
			SpawnWall(wallTile, posL, wallRot, sizeL);
			SpawnWall(wallTile, posR, wallRot, sizeR);
			
			if(Application.isEditor)
				DestroyImmediate(wall.gameObject);
			else
				Destroy(wall.gameObject);
		}
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
