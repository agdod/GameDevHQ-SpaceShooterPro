using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
	[SerializeField] [Range(-20, 20)] private float _spinRate = 10.0f;
	[SerializeField] [Range(0f, 3f)] private float _delay;
	[SerializeField] private GameObject _explosion;

	private void Start()
	{
		transform.position = new Vector3(Random.Range(-10, 10), 6, 0);
	}

	void Update()
	{
		transform.Rotate(Vector3.forward * _spinRate * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Laser")
		{
			if (_explosion != null)
			{
				Instantiate(_explosion, transform.position, Quaternion.identity);

				Destroy(gameObject, _delay);
			}
		}
	}
}
