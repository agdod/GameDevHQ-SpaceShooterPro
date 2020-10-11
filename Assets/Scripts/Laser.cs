using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField] private float _speed = 8.0f;
	[SerializeField] private float _upperBound = 8.0f;
	[SerializeField] private AudioClip _laserSFX;
	private AudioSource _audioSource;


	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
		if (_audioSource == null)
		{
			Debug.LogError("No audio Source found attached to componet");
		}
		else
		{
			if (_laserSFX != null && _audioSource.isActiveAndEnabled == true)
			{
				_audioSource.clip = _laserSFX;
			}
		}

		LaserSoundFx();
	}

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

	public void LaserSoundFx()
	{
		_audioSource.Play();
	}
}
