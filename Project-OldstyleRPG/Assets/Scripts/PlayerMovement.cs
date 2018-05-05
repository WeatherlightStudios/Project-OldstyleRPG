using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public float speed;
	
	void Start () {
		
	}
	
	void Update () {
		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");
		
		Vector3 movement = transform.forward * inputY + transform.right * inputX;
		transform.position += movement * speed * Time.deltaTime;
	}
}
