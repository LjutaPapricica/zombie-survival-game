﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public int health = 100;

	[SerializeField] private GameObject gameOverText;
	[SerializeField] private float speed;
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private ScreenController screenController;
	[SerializeField] private AudioClip damageSound;

	private Vector3 direction;
	private Rigidbody rigidbodyPlayer;
	private Animator animatorPlayer;

	void Start () {
		// Starts the game without been paused
		Time.timeScale = 1;

		rigidbodyPlayer = GetComponent<Rigidbody>();
		animatorPlayer = GetComponent<Animator>();
	}

	void Update () {

		// player movement inputs. Stores the X and Z direction using the pressed keys
		float xAxis = Input.GetAxis("Horizontal");
		float zAxis = Input.GetAxis("Vertical");

		// creates a Vector3 with the new direction
		direction = new Vector3 (xAxis, 0, zAxis);

		// player animations transition
		if (direction != Vector3.zero)
			animatorPlayer.SetBool("Running", true);
		else
			animatorPlayer.SetBool("Running", false);

		// if the player isn't alive anymore 
		// and the mouse button was clicked, restart the game
		if (health <= 0) {
			if (Input.GetButtonDown ("Fire1"))
				SceneManager.LoadScene("Game");
		}
	}
		
	void FixedUpdate () {
		// moves the player by second using physics
		// use physics (rigidbody) to compute the player movement is better than transform.position 
		// because prevents the player to "bug" when colliding with other objects
		rigidbodyPlayer.MovePosition(
			rigidbodyPlayer.position + (direction * Time.deltaTime * speed));

		// makes the player rotation follows the mouse position
		// it uses a LayerMask that computes only the Raycasts that collide with the ground
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100, groundMask)) {
			Vector3 positionPoint = hit.point - transform.position;
			positionPoint.y = transform.position.y;
			Quaternion newRotation = Quaternion.LookRotation(positionPoint);
			rigidbodyPlayer.MoveRotation(newRotation);
		}
	}

	/// <summary>
	/// Loses health based on the damage value. 
	/// If health is equal to or less than 0 the game ends.
	/// </summary>
	/// <param name="damage">Damage taken.</param>
	public void LoseHealth (int damage) {
		health -= damage;
		screenController.UpdateHealthSlider();

		// plays the damage sound
		AudioController.instance.PlayOneShot(damageSound);

		if (health <= 0)
			GameOver();
	}

	/// <summary>
	/// Pauses the game and display the Game Over message on the screen.
	/// </summary>
	private void GameOver () {
		Time.timeScale = 0;
		gameOverText.SetActive(true);
	}
}
