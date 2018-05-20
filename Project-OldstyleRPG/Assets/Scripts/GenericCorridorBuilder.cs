using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GenericCorridorBuilder : MonoBehaviour {
	
	//public List<GameObject> oldObjs;
	
	public GameObject floorTile;
	public GameObject wallTile;
	public GameObject ceilingTile;
	
	public LayerMask wallMask;
	
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
		GameObject ground = Instantiate(floorTile, info.position, Quaternion.identity, this.transform);
		
		MeshRenderer rendGround = ground.GetComponentInChildren<MeshRenderer>();
		Material tempMat = new Material(rendGround.sharedMaterial);
		tempMat.mainTextureScale = info.size;
		rendGround.sharedMaterial = tempMat;
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
		Vector3 raysOffset = Vector3.zero;
		float wallLength = 0;
		Vector3 rayDir = Vector3.one;
		
		if(info.size.y <= 1){
			//wall from left to right
			wall1 += Vector3.forward * info.size.y/2;
			wall2 += Vector3.back * info.size.y/2;
			wallDir = Vector3.forward;
			wallSize = new Vector2(info.size.x, info.height);
			wallScale = new Vector3(info.size.x, info.height, 1);
			wallLength = info.size.x;
			raysOffset.x = 1.1f * wallLength;
			rayDir = Vector3.right;
		}
		else if(info.size.x <= 1){
			//wall from forward to back
			wall1 += Vector3.right * info.size.x / 2;
			wall2 += Vector3.left * info.size.x / 2;
			wallDir = Vector3.right;
			wallSize = new Vector2(info.size.y, info.height);
			wallScale = new Vector3(info.size.y, info.height, 1);
			wallLength = info.size.y;
			raysOffset.z = 1.1f * wallLength;
			rayDir = Vector3.forward;
		}
		
		GameObject wall1Obj = Instantiate(wallTile, wall1, Quaternion.LookRotation(wallDir, Vector3.up), this.transform);
		GameObject wall2Obj = Instantiate(wallTile, wall2, Quaternion.LookRotation(wallDir * -1, Vector3.up), this.transform);
		
		wall1Obj.transform.localScale = wallScale;
		MeshRenderer rendWall1 = wall1Obj.GetComponentInChildren<MeshRenderer>();
		tempMat = new Material(rendWall1.sharedMaterial);
		tempMat.mainTextureScale = wallSize;
		rendWall1.sharedMaterial = tempMat;
		
		wall2Obj.transform.localScale = wallScale;
		wall2Obj.GetComponentInChildren<MeshRenderer>().sharedMaterial = tempMat;
		
		/// <summary>
		/// Rays
		/// </summary>
		//Rays(raysOffset, rayDir, wallLength);
		if(Application.isEditor)
			StartCoroutine(Rays(raysOffset, rayDir, wallLength, 0.0f));
		else
			StartCoroutine(Rays(raysOffset, rayDir, wallLength, 0.5f));
			
	}
	
	private IEnumerator Rays(Vector3 raysOffset, Vector3 rayDir, float wallLength, float timer){
		yield return new WaitForSeconds(timer);
		
		Vector3 rayPos1 = info.position + Vector3.up * info.height / 2 - raysOffset/2;
		Vector3 rayPos2 = info.position + Vector3.up * info.height / 2 + raysOffset/2;
		
		//Debug.DrawRay(rayPos1, rayDir, Color.red, 500f);
		//Debug.DrawRay(rayPos2, rayDir*-1, Color.blue, 500f);
		
		Debug.DrawLine(rayPos2, rayPos2 + rayDir*wallLength*-1.1f, Color.blue, 50f);
		Debug.DrawLine(rayPos1, rayPos1 + rayDir*wallLength*1.1f, Color.red, 50f);
		
		RaycastHit[] hits = Physics.RaycastAll(rayPos1, rayDir, wallLength * 1.1f, wallMask);
		//RaycastHit[] hitsBack = Physics.RaycastAll(rayPos2, rayDir * -1, wallLength * 1.1f, wallMask);
		
		//print(hits.Length + "  " + hitsBack.Length);
		
		bool isHorizontal = false;
		if(info.size.y <= 1)
			isHorizontal = true;
		//for every hit
		//give info to the room gen of the point of collision
		for (int i = 0; i < hits.Length; i++)
		{
			GameObject wall = hits[i].collider.gameObject;
			//print(wall.name);
			WallCutter cutter = wall.GetComponent<WallCutter>();
			cutter.SplitWall(wall, hits[i].point, 1, isHorizontal);
		}
		/*
		for(int i = 0; i < hitsBack.Length; i++){
			GameObject wall = hitsBack[i].collider.gameObject;
			print(wall.name);
			WallCutter cutter = wall.GetComponent<WallCutter>();
			cutter.SplitWall(wall, hitsBack[i].point, 1, isHorizontal);
		}
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
