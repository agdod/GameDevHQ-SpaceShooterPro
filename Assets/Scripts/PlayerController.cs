﻿using System.Collections;
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
	[SerializeField] private Vector3 _laserOffset;
	[SerializeField] private float _fireRate;
	private float _canFire = -1f;


	void Start()
	{
		// Set player postion to zero
		transform.position = Vector3.zero;
	}

	void Update()
	{
		PlayerMovement();
		// Fire laser at speed _fireRate
		if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
		{
			FireLaser();
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

	void FireLaser()
	{
		
		Debug.Log("Space Pressed fire laser");
		_canFire = Time.time + _fireRate;
		Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
	}
}
