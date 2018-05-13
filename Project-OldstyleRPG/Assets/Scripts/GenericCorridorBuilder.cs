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
		ground.GetComponentInChildren<SpriteRenderer>().size = info.size;
		
		/// <summary>
		/// Walls
		/// </summary>
		//4 walls positions = position + 4 directions * size/2
		Vector3 wall1 = info.position;
		Vector3 wall2 = info.position;
		Vector3 wallDir = Vector3.zero;
		Vector2 wallSize = Vector3.one;
		
		if(info.size.y <= 1){
			wall1 += Vector3.forward * info.size.y/2;
			wall2 += Vector3.back * info.size.y/2;
			wallDir = Vector3.back;
			wallSize = new Vector2(info.size.x, info.height);
		}
		else if(info.size.x <= 1){
			wall1 += Vector3.right * info.size.x / 2;
			wall2 += Vector3.left * info.size.x / 2;
			wallDir = Vector3.left;
			wallSize = new Vector2(info.size.y, info.height);
		}
		
		
		GameObject wall1Obj = Instantiate(wallTile, wall1, Quaternion.LookRotation(wallDir, Vector3.up), this.transform);
		GameObject wall2Obj = Instantiate(wallTile, wall2, Quaternion.LookRotation(wallDir * -1, Vector3.up), this.transform);
		
		wall1Obj.GetComponentInChildren<SpriteRenderer>().size = wallSize;
		wall2Obj.GetComponentInChildren<SpriteRenderer>().size = wallSize;
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
