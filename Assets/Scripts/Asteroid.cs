using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _spinRate = 0.65f;
	private Vector3 _rotationRate;

	private void Start()
	{
		_rotationRate = new Vector3(0, 0, _spinRate);
	}
	// Update is called once per frame
	void Update()
    {
		transform.Rotate(_rotationRate);
	}
}
