using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	//Define ennum for differnt type of enemy movement current and future options
	public enum EnemyMovmentID
	{
		Straight, // 0
		Angled, // 1
		//NOTE: Add addition movement types before count.
		Count // 2 == Count of the type of enemy movement 
	};

	[SerializeField] private float _upperBound = 8.0f;
	[SerializeField] private float _lowerBound = -3.5f;
	[SerializeField] private float _enemySpeed = 4.0f;
	[SerializeField] private GameObject _laserPrefab;
	[SerializeField] private Vector3 _laserOffset;
	[SerializeField] private float _fireRateMax = 7.0f;
	[SerializeField] private float _fireRatemin = 3.0f;
	[SerializeField] private AudioClip _explosionAudioFx; // explosion audio clip

	[SerializeField] private EnemyMovmentID _enemyMovmentID;
	private bool _respawnable;
	private bool _enemyAlive = true;
	private bool _canFire;
	private AudioSource _audioSource;
	private PlayerController _player;
	private Animator _animator;

	public bool EnemyAlive
	{
		get { return _enemyAlive; }
	}

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
		// Generate a random movement type from the movement enuum for each enemy.

		int count = (int)EnemyMovmentID.Count; // Cast the Count as int value
		_enemyMovmentID = (EnemyMovmentID) Random.Range(0, count); //Random.Range cast as enemyMovement

		// inital Random spawn position
		RespawnEnemy();
		StartCoroutine(EnemyFireRoutine());
	}

	void Update()
	{
		MoveEnemy();
	}

	IEnumerator EnemyFireRoutine()
	{
		// while enemy is alive (ie so while true, loop will be destroyed with enemy)
		while (_enemyAlive == true)
		{
			FireLaser();
			float delay = Random.Range(_fireRatemin, _fireRateMax);
			yield return new WaitForSeconds(delay);
		}
	}

	void FireLaser()
	{
		if (_laserPrefab != null)
		{
			GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
			// Get the Laser script from the Laser Prefab, enable that each laser has been fired by enemy
			Laser[] _laser = enemyLaser.GetComponentsInChildren<Laser>();
			foreach (Laser laser in _laser)
			{
				laser.EnemyLaserFired();
			}
		}
		else
		{
			Debug.LogError("No  LaserPrefab attached.");
		}

	}

	void RespawnEnemy()
	{
		float xPos = Random.Range(-10f, 10f);
		transform.position = new Vector3(xPos, _upperBound, 0);
	}

	void MoveEnemy()
	{
		//  move enemy at 4 m/s
		// either down or angled across the screen
		//  if bottom of screen respawn at top with new random postion

		switch(_enemyMovmentID)
		{
			case EnemyMovmentID.Straight:
				transform.Translate(Vector3.down  * _enemySpeed * Time.deltaTime);
				_respawnable = true;
				break;
			case EnemyMovmentID.Angled:
				transform.Translate((Vector3.down + Vector3.right) * _enemySpeed * Time.deltaTime);
				_respawnable = true;
				break;
		}		

		if (transform.position.y < _lowerBound && _respawnable == true)
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
			//Check laser wasnt fired by enemy -  enemy are "immune" to their own laser
			Laser laser = other.GetComponent<Laser>();
			if (laser.EnemyFired == false)
			{
				_player.AddScore(10);
				Destroy(other.gameObject);
				DestroyEnemy();
			}
		}
	}

	private void DestroyEnemy()
	{
		// Send message to animator to activate the trigger "OnEnemyDestroyed"
		_animator.SetTrigger("OnEnemyDestroyed");
		_enemyAlive = false;
		StopCoroutine(EnemyFireRoutine());
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
