using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//  Player Movment  Variables
	[SerializeField] private float _speed = 1.0f;
	[SerializeField] private float _yLowerBound = -2.0f;
	[SerializeField] private float _yUpperBound = 0.0f;
	// Firing
	[SerializeField] private GameObject _laserPrefab;


	void Start()
	{
		// Set player postion to zero
		transform.position = Vector3.zero;
	}

	void Update()
	{
		PlayerMovement();
		if (Input.GetKeyDown(KeyCode.Space))
		{
			// Fire Laser
			Debug.Log("Space Pressed fire laser");
			Instantiate(_laserPrefab, transform.position, Quaternion.identity);
		}
	}

	void PlayerMovement()
	{
		// Get vertical and horizontal Inputs
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		// calcutale speed and direction
		Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
		transform.Translate(direction * _speed * Time.deltaTime);

		// Limit player bounds on y and x axis
		//Clamp on y axis
		transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _yLowerBound, _yUpperBound), 0);

		// bound on x axis
		if (transform.position.x > 11)
		{
			transform.position = new Vector3(-11, transform.position.y, 0);
		}
		else if (transform.position.x <= -11)
		{
			transform.position = new Vector3(11, transform.position.y, 0);
		}
	}
}
