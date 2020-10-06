using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float _speed = 1.0f;


	void Start()
	{
		// take current positon = new postion (0,0,0)
		transform.position = new Vector3(0, 0, 0);
	}

	// Update is called once per frame
	void Update()
	{
		// Get vertical and horizontal Inputs
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");
		// calcutale speeds
		//float verticalSpeed = 1 * verticalInput * _speed * Time.deltaTime;
		//float horizontalSpeed = 1 * horizontalInput * _speed * Time.deltaTime;
		//Clean up code - (*_speed* Time.deltaime is commmon to both )
		Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
		transform.Translate(direction * _speed * Time.deltaTime);

		// Limit player bounds
		/*	if player position on the y axis is greater than 0
		 *		y pos = 0
		 *	else if position on the y is less than -3.8f (only use else if if checking same value ie y)
		 *		y pos = -3.8f

		 *	if player on the x > 11
		 *		x pos = -11
		 *	else if player on the x is less than -11
		 *	x pos = 11
		 */
		//bound on y axis
		if (transform.position.y >= 0)
		{
			transform.position = new Vector3(transform.position.x, 0, 0);
		} 
		else if (transform.position.y <= -2.0f)
		{
			transform.position = new Vector3(transform.position.x, -2.0f, 0);
		}
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
