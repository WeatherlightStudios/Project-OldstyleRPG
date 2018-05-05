using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour {
	
	public GameObject swordCollider;
	public Animator anim;
	
	void Start () {
		
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			anim.SetTrigger("Attack");
		}
	}
	
	public void EnableCollider(){
		swordCollider.SetActive(true);
	}
	
	public void DisableCollider(){
		swordCollider.SetActive(false);
	}
	
}
