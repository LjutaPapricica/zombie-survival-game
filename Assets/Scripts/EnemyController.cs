﻿using UnityEngine;

public class EnemyController : MonoBehaviour {

	[SerializeField] private Transform player;
	[SerializeField] private float speed;

	void FixedUpdate () {

		// get the distance between this enemy and the player
		float distance = Vector3.Distance(transform.position, player.position);
	
		// check if enemy and player are colliding.
		// The 2.5f is because both enemy and player have a Capsule Collider with radius equal 1,
		// so if the distance is bigger than both radius they are colliding
		if (distance > 2.5f) {
			// get the final position, that is, 
			// the distance between the enemy and the player
			Vector3 direction = player.position - transform.position;

			// moves the enemy as in the PlayerController but 
			// instead using the GetAxis method it uses the normalized direction vector
			GetComponent<Rigidbody>().MovePosition(
				GetComponent<Rigidbody>().position + (direction.normalized * Time.deltaTime * speed));

			// rotates the enemy towards the player
			Quaternion newRotation = Quaternion.LookRotation(direction);
			GetComponent<Rigidbody>().MoveRotation(newRotation);
		} 
	}
}