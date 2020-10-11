using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private float _upperBound = 8.0f;
	[SerializeField] private float _lowerBound = -3.5f;
	[SerializeField] private float _enemySpeed = 4.0f;
	[SerializeField] private AudioClip _explosionAudioFx; // explosion audio clip
	
	private AudioSource _audioSource;
	private PlayerController _player;
	private Animator _animator;

	void Start()
	{
		// Collect required componets and do null checking
		_player = GameObject.Find("Player").GetComponent<PlayerController>();
		if (_player == null)
		{
			Debug.LogError("No player found in scene. Insert Player.");
		}

		_animator = GetComponent<Animator>();
		if (_animator == null)
		{
			Debug.LogError("No animation found. Ensure animation is attached. ");
		}

		_audioSource = GetComponent<AudioSource>();
		if (_audioSource == null)
		{
			Debug.LogError("No audio souce on component.");
		}
		else
		{
			if (_explosionAudioFx != null)
			{
				_audioSource.clip = _explosionAudioFx;
			}
		}
		// inital Random spawn position
		RespawnEnemy();
	}

	void Update()
	{
		MoveEnemy();
	}

	void RespawnEnemy()
	{
		float xPos = Random.Range(-10f, 10f);
		transform.position = new Vector3(xPos, _upperBound, 0);
	}

	void MoveEnemy()
	{
		//  move down at 4 m/s
		//  if bottom of screen respawn at top with new random postion
		transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
		if (transform.position.y < _lowerBound)
		{
			RespawnEnemy();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			// Damage Player
			PlayerController player = other.GetComponent<PlayerController>();
			if (player != null)
			{
				player.Damage();
			}
			DestroyEnemy();
		}
		else if (other.tag == "Laser")
		{
			_player.AddScore(10);
			Destroy(other.gameObject);
			DestroyEnemy();
		}
	}
	private void DestroyEnemy()
	{

		// Send message to animator to activate the trigger "OnEnemyDestroyed"
		_animator.SetTrigger("OnEnemyDestroyed");
		// Play Explosion Sound effect
		if (_audioSource != null && _explosionAudioFx != null)
		{
			_audioSource.Play();
		}

		// Stop enemy moving  when playing animation
		_enemySpeed = 0.0f;
		Collider2D collider = GetComponent<Collider2D>();
		collider.enabled = false;
		// Destroy enemy(gameobject) after animation has completed
		Destroy(gameObject, 2.3f);
	}
}
