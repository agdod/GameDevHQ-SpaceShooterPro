using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Laser : MonoBehaviour
{
	[SerializeField] private float _speed = 8.0f;
	[SerializeField] private float _upperBound = 8.0f;
	[SerializeField] private float _lowerBound = -3.5f;
	[SerializeField] private bool _enemyFired = false;
	[SerializeField] private AudioClip _laserSFX;

	private AudioSource _audioSource;

	// Getter return bool of _enemyFired to outside class
	public bool EnemyFired
	{
		get { return _enemyFired; }
	}

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
		if (_enemyFired == true)
		{
			FireLaserDown();
		}
		else
		{
			FireLaserUp();
		}

	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && _enemyFired == true)
		{
			other.GetComponent<PlayerController>().DoubleHitCheck();
		}
	}

	void FireLaserUp()
	{
		// Player fired laser - laser moves up
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

	void FireLaserDown()
	{
		// Enemy fired laser -  laser moves down
		transform.Translate(Vector3.down * _speed * Time.deltaTime);

		// Destroy laser when out of bounds (bottom of screen)
		if (transform.position.y < _lowerBound)
		{
			Destroy(gameObject);
		}
	}

	public void EnemyLaserFired()
	{
		_enemyFired = true;
	}

	public void LaserSoundFx()
	{
		_audioSource.Play();
	}
}
