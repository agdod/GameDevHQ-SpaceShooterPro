using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	[SerializeField] private float _delay = 2.38f;
	void Start()
	{
		// Destroy the explosion after animation has completed
		Destroy(gameObject, _delay);
	}
}
