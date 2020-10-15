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
	[SerializeField] private bool _isHoming = false;
	[SerializeField] private AudioClip _laserSFX;

	private Transform _target;
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
			if (_isHoming == false)
			{
				FireLaserUp();
			}
			else if (_isHoming == true)
			{
				FireHomingLaser();
			}
		}

	}

	void FireHomingLaser()
	{
		// Check that _target hasnt already been destroyed
		if (_target != null)
		{
			// move closer towards the target
			//Calcuate distance to move
			// Rotate laser to _target
			Vector3 relativeTarget = (_target.position - transform.position).normalized;
			Quaternion toQuaternion = Quaternion.FromToRotation(Vector3.up, _target.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, toQuaternion, _speed * Time.deltaTime);
			//transform.LookAt(Vector3.forward, Vector3.Cross(Vector3.up, _target.position));
			float step = _speed * Time.deltaTime;

			transform.position = Vector3.MoveTowards(transform.position, _target.position, step);
			// Check the distance to see if laser has arrived
			if (Vector3.Distance(transform.position, _target.position) < 0.001f)
			{
				//get the enemy script from the enemy
				Enemy enemy = _target.GetComponent<Enemy>();
				if (enemy.EnemyAlive != true)
				{
					// enemy has already been destroyed
					Destroy(gameObject);
				}
			}
		}
		else
		{
			// if target has been destroyed laser carrie on upward path
			// ** Note:  would be better for laser to carry on same path and fade off screen edges **
			Destroy(gameObject);
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
			Transform parentObject = transform.parent;
			Destroy(parentObject.gameObject);
		}
	}

	public void HomingLaserFired(Transform target)
	{
		_isHoming = true;
		_target = target;
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
