using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
	public enum PowerupID
	{
		TripleShot,
		Speed,
		Shield,
		Ammo
	};

	[SerializeField] private PowerupID powerupId;
	[SerializeField] private float _decentRate = 3.0f;
	[SerializeField] private float _lowerBounds = -3.5f;
	[SerializeField] private AudioClip _powerUpSoundFx;

	private AudioSource _audioSource;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
		if (_audioSource = null)
		{
			Debug.LogError("No AudioSource component found.");
		}
	}

	void Update()
	{
		transform.Translate(Vector3.down * _decentRate * Time.deltaTime);
		// If PowerUp leaves bottom of screen
		if (transform.position.y < _lowerBounds)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("other : " + other);
		if (other.tag == "Player")
		{
			// Activate powerup
			PlayerController player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				switch (powerupId)
				{
					case PowerupID.TripleShot:
						PowerUpAudio();
						player.ActivateTripleShot();
						break;
					case PowerupID.Speed:
						PowerUpAudio();
						player.ActivateSpeedBoost();
						break;
					case PowerupID.Shield:
						PowerUpAudio();
						player.ActivateShield();
						break;
					case PowerupID.Ammo:
						PowerUpAudio();
						player.ActivateAmmoRefill();
						break;
				}
			}
			Destroy(gameObject);
		}
	}

	void PowerUpAudio()
	{
		if (_powerUpSoundFx != null)
		{
			// works but ausio is very subtle 
			// maybe access Ausio manager to play sound
			// or disbale collider and mesh render and destroy after 1 second..
			AudioSource.PlayClipAtPoint(_powerUpSoundFx, transform.position);
		}
	}

}
