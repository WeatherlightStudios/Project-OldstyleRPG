using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemy : MonoBehaviour {

	public int damage;
	public float force;
	public Color damaged;
	
	/// <summary>
	/// OnTriggerEnter is called when the Collider other enters the trigger.
	/// </summary>
	/// <param name="other">The other Collider involved in this collision.</param>
	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Enemy"){
			other.GetComponent<EnemyBehaviour>().health -= damage;
			other.GetComponent<SpriteRenderer>().material.color = damaged;
			other.GetComponent<EnemyBehaviour>().ColorTimer(0.5f);
			Vector3 enemyDir = other.transform.position - transform.parent.position;
			enemyDir = Vector3.ProjectOnPlane(enemyDir, Vector3.up).normalized;
			enemyDir += Vector3.up * 0.4f;
			other.GetComponent<Rigidbody>().AddForce(enemyDir * force);
		}
	}
}
