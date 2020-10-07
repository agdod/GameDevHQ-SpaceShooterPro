using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float _upperBound = 8.0f;
	[SerializeField] private float _lowerBound = -3.5f;
	[SerializeField] private float _enemySpeed = 4.0f;

	void Start()
	{
		// inital Random spawn position
		RespawnEnemy();
	}

	void Update()
	{
		MoveEnemy();
	}

	void RespawnEnemy()
	{
		float xPos = Random.Range(-10f, 10f);
		transform.position = new Vector3(xPos, _upperBound, 0);
	}

	void MoveEnemy()
	{
		//  move down at 4 m/s
		//  if bottom of screen respawn at top with new random postion
		transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
		if (transform.position.y < _lowerBound)
		{
			RespawnEnemy();
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		/*	if other is player
		 *		damage player
		 *		destroy enemy
		 *		
		 *	if other is laser
		 *		destroy laser
		 *		destroy enemy
		 */

		Debug.Log("hit : " + other.transform.name);
		if (other.transform.name=="Player")
		{
			// Damage Player
			Destroy(gameObject);
		} 
		else if (other.transform.name == "Laser(Clone)")
		{
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}
}
