using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	[SerializeField] [Range(0, 5)] private float _duration = 2f;
	[SerializeField] [Range(0, 1)] private float _force=0.05f;

	public void Shake()
	{
		StartCoroutine(ShakeIt());
	}

	public IEnumerator ShakeIt()
	{
		Vector3 initialPosition = transform.position;
		float posZ = transform.position.z;
		float timePassed = 0f;
		while (_duration > timePassed)
		{
			// Generate random x,y values for camera shake 
			float posX = Random.Range(-1f, 1f) * _force + transform.position.x;
			float posY = Random.Range(-1f, 1f) * _force + transform.position.y;
			timePassed +=Time.deltaTime;
			transform.position = new Vector3(posX, posY, posZ);
			yield return new WaitForSeconds(0f);
		}
		transform.position = initialPosition;
	}
}
