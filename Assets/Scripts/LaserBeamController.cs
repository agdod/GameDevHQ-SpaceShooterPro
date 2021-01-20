using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamController : MonoBehaviour
{
	[SerializeField] private GameObject _bigLaserPrefab;

	private float _lowerBound = -3.5f;
	private bool _hasHit;
	private bool _fireLaser;

	// Fire continus laser to bottom of screen.
	float laserSpacing = 0.5f;                          // Space between each laser
	float laserTracker;   // Inital laser postion including offset
	Vector3 laserPosition;
	List<GameObject> lasers;    // List to hold the lasers that are fired
	float _volume = 1.0f;

	int pos = 0;                                        // postion marker for List

	//Getters 

	public bool HasHit
	{
		get { return _hasHit; }
	}

	public void FireLaserBeam()
	{
		lasers = new List<GameObject>();
		laserTracker = transform.position.y - 1.2f;
		laserPosition = transform.position;
		pos = 0;
		laserPosition.y = laserTracker;
		_hasHit = false;
		_fireLaser = true;
		_volume = 1.0f;

		while (_fireLaser)
		{
			// Instantiate and add laser to control list
			GameObject laser = Instantiate(_bigLaserPrefab, laserPosition, Quaternion.identity);
			lasers.Add(laser);

			// Collect required componets from laser
			Laser _laser = lasers[pos].GetComponentInChildren<Laser>();
			AudioSource _audio = lasers[pos].GetComponent<AudioSource>();

			// Set laser properties ie is fired by enemy, and is part of laser beam.
			_laser.EnemyLaserFired();
			// Set laser as part of laser beam and pass in this as laser controller
			_laser.SetToLaserBeam(this);

			// Adjust volume for constective lasers instantiated - prevents laser audio becoming excesively loud
			_volume = _volume * 0.85f;
			_audio.volume = _volume;

			// Adjust postion for next laser to be instantiated 
			// and perfrom out of bounds check
			laserTracker -= laserSpacing;
			laserPosition.y = laserTracker;
			if (laserTracker < _lowerBound)
			{
				_fireLaser = false;
			}
			pos++;
		}

	}

	public void LaserBeamHit()
	{
		_hasHit = true;
		_fireLaser = false;
	}

	public void RemoveFromList()
	{
		lasers.RemoveAt(pos - 1);
		pos--;
	}

}
