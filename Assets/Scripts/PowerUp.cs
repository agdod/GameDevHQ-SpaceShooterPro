using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{

	[SerializeField] private float _speed = 3.0f;
	[SerializeField] private float _lowerBounds = -3.5f;

	void Update()
	{
		transform.Translate(Vector3.down * _speed * Time.deltaTime);
		// If PowerUp leaves bottom of screen
		if (transform.position.y < _lowerBounds)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("other : " + other);
		if (other.tag == "Player")
		{
			// Activate powerup
			PlayerController player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				player.ActivateTripleShot();
			}
			Destroy(gameObject);
		}
	}
}
