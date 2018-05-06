using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
	
	public GameObject target;
	public GameObject projectile;
	public Color fine;
	
	public float speed;
	
	public int health;
	
	private Vector3 direction;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetDir = target.transform.position - transform.position;
		targetDir = Vector3.ProjectOnPlane(targetDir, Vector3.up).normalized;
		
		transform.position += targetDir * speed * Time.deltaTime;
		direction = targetDir;
		
		if(health <= 0){
			this.gameObject.SetActive(false);
		}
	}
	
	public void ColorTimer(float time){
		StartCoroutine(RepaintTimer(time));
	}
	
	IEnumerator RepaintTimer(float time){
		yield return new WaitForSeconds(time);
		this.GetComponent<SpriteRenderer>().material.color = fine;
	}
}
