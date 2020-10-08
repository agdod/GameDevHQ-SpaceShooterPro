using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField] private float _speed = 8.0f;
	[SerializeField] private float _upperBound = 8.0f;

	void Update()
	{
		transform.Translate(Vector3.up * _speed * Time.deltaTime, 0);

		// Destroy laser when out of bounds
		if (transform.position.y > _upperBound)
		{
			// Check if has parent (i.e is part of triple shot)
			if (transform.parent != null)
			{
				// Get the parent gameObject
				Transform parentObject = transform.parent;
				Destroy(parentObject.gameObject);
			}
			else
			{
				Destroy(gameObject);
			}

		}
	}
}
