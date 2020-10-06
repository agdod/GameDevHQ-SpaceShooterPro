using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField] private float _speed = 8.0f;
	[SerializeField] private float _upperBound = 8.0f;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{ 
		transform.Translate(Vector3.up * _speed * Time.deltaTime, 0);

		// Destroy laser when out of bounds
		if (transform.position.y > _upperBound)
		{
			Destroy(gameObject);
		}
	}
}
