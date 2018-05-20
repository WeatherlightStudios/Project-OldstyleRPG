using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCutter : MonoBehaviour {

	private GameObject wall;
	
	public void SplitWall(GameObject wall, Vector3 pointOfCollision, float wallWidth, bool isHorizontal){
		wall = this.gameObject;
		
		if(isHorizontal){
			//vertical split
			//print("vertical split");
			Vector3 minWallPos = wall.transform.position - Vector3.forward * wall.transform.localScale.x / 2;	//OK
			Vector3 maxWallPos = wall.transform.position + Vector3.forward * wall.transform.localScale.x / 2;	//OK
			
			Vector3 minSplit = pointOfCollision - Vector3.forward * wallWidth/2;
			Vector3 maxSplit = pointOfCollision + Vector3.forward * wallWidth/2;
			
			Vector3 posB = (minWallPos + minSplit)/2.0f;
			Vector3 posF = (maxSplit + maxWallPos)/2.0f;
			
			Quaternion wallRot = wall.transform.rotation;
			
			float xSizeBack = minSplit.z - minWallPos.z;
			float xSizeForw = maxWallPos.z - maxSplit.z;
			float height = wall.transform.localScale.y;
			
			Vector3 sizeB = new Vector3(xSizeBack, height, 1);
			Vector3 sizeF = new Vector3(xSizeForw, height, 1);
			
			Debug.DrawLine(posB, posB + Vector3.forward * xSizeForw, Color.cyan, 5f);			
			
			SpawnWall(wall, posB, wallRot, sizeB);
			SpawnWall(wall, posF, wallRot, sizeF);
			
			StartCoroutine(Delete());

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
			
			SpawnWall(wall, posL, wallRot, sizeL);
			SpawnWall(wall, posR, wallRot, sizeR);
			
			StartCoroutine(Delete());
			
		}
	}
	
	private IEnumerator Delete(){
		
			if(Application.isEditor){
				yield return new WaitForSeconds(0.0f);
				DestroyImmediate(this.gameObject);
			}
			else{
				yield return new WaitForSeconds(0.5f);
				Destroy(this.gameObject);
			}
	}
	
	private void SpawnWall(GameObject toSpawn, Vector3 position, Quaternion rotation, Vector3 size){
		
		GameObject spawned = Instantiate(toSpawn, position, rotation, this.transform.parent);
		spawned.transform.localScale = size;
		MeshRenderer rend = spawned.GetComponentInChildren<MeshRenderer>();
		Material temp = new Material(rend.sharedMaterial);
		temp.mainTextureScale = size;
		rend.sharedMaterial = temp;
	}
	
}
