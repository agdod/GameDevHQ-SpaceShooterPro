using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField][Range(-20,20)] private float _spinRate = 10.0f;
	private Vector3 _rotationRate;

	private void Start()
	{
		_rotationRate = new Vector3(0, 0, 1);
	}
	// Update is called once per frame
	void Update()
    {
		transform.Rotate(Vector3.forward * _spinRate * Time.deltaTime);
	}
}
