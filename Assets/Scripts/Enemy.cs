using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	//Define ennum for differnt type of enemy movement current and future options
	public enum EnemyMovementID
	{
		Straight,   // 0
		Angled,     // 1
		Bounce,     // 2
					//NOTE: Add addition movement types before count.
		Count   //  3 == Count of the type of enemy movement 
	};

	public enum EnemyFireTypeID
	{
		Normal,     // 0
		BigLaser,   // 1
					// Add additional fire types here.
		Count
	};

	// Enemy type are combinations of different movement and firing patterns

	public enum EnemyTypeID
	{
		Normal,
		Random,
		ZigZag, // Bounces around screen firing big laser

		Count
	}

	[SerializeField] private float _upperBound = 8.0f;
	[SerializeField] private float _lowerBound = -3.5f;
	[SerializeField] private float _minHorizontalBound = -9.3f;
	[SerializeField] private float _maxHorizontalBound = 10.5f;
	[SerializeField] private float _enemySpeed = 4.0f;

	[SerializeField] private GameObject _laserPrefab;
	[SerializeField] private Vector3 _laserOffset;

	[SerializeField] private LaserBeamController _laserBeamController;

	[SerializeField] private float _fireRateMax = 7.0f;
	[SerializeField] private float _fireRatemin = 3.0f;
	[SerializeField] private AudioClip _explosionAudioFx; // explosion audio clip

	[SerializeField] private EnemyMovementID _enemyMovementID;
	[SerializeField] private EnemyFireTypeID _enemyFireTypeID;
	[SerializeField] private EnemyTypeID _enemyTypeID;
	private bool _respawnable;
	private bool _enemyAlive = true;
	private bool _canFire;
	private AudioSource _audioSource;
	private PlayerController _player;
	private Animator _animator;

	// Directional movement modifiers. Negative modifer will move in opposite direction
	private int _verticalMovemntAdjuster = 1;
	private int _horizontalMovementAdjuster = 1;

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

		GenerateEnemyType();

		// inital Random spawn position
		RespawnEnemy();
		StartCoroutine(EnemyFireRoutine());
	}

	void Update()
	{
		MoveEnemy();
	}

	void GenerateEnemyType()
	{
		// Generate a random movement type from the movement enuum for each enemy.

		int count = (int)EnemyTypeID.Count; // Cast the Count as int value - used to get the length of the Enemy Type Enum
		_enemyTypeID = (EnemyTypeID)Random.Range(0, count); //Random.Range cast as enemyMovement

		switch (_enemyTypeID)
		{
			case EnemyTypeID.Normal:
				_enemyMovementID = EnemyMovementID.Straight;
				_enemyFireTypeID = EnemyFireTypeID.Normal;
				break;
			case EnemyTypeID.Random:
				count = (int)EnemyMovementID.Count;
				_enemyMovementID = (EnemyMovementID)Random.Range(0, count);
				count = (int)EnemyFireTypeID.Count;
				_enemyFireTypeID = (EnemyFireTypeID)Random.Range(0, count);
				break;
			case EnemyTypeID.ZigZag:
				_enemyMovementID = EnemyMovementID.Bounce;
				_enemyFireTypeID = EnemyFireTypeID.BigLaser;
				break;
		}
	}

	IEnumerator EnemyFireRoutine()
	{
		// while enemy is alive (ie so while true, loop will be destroyed with enemy)
		while (_enemyAlive == true)
		{
			switch (_enemyFireTypeID)
			{
				case EnemyFireTypeID.Normal:
					FireLaser();
					break;
				case EnemyFireTypeID.BigLaser:
					FireLaserBeam();
					break;
			}

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

	void FireLaserBeam()
	{
		if (_laserBeamController != null)
		{
			_laserBeamController.FireLaserBeam();
		}
	}

	void RespawnEnemy()
	{
		float xPos = Random.Range(-10f, 10f);
		transform.position = new Vector3(xPos, _upperBound, 0);
	}

	void EnemyBounce()
	{

		_respawnable = false;
		transform.Translate(((Vector3.down * _verticalMovemntAdjuster) + (Vector3.right * _horizontalMovementAdjuster)) * _enemySpeed * Time.deltaTime); // Additional brackets for clarification
		if (transform.position.y < _lowerBound)
		{
			_verticalMovemntAdjuster = -1;
		}
		else if (transform.position.y > _upperBound)
		{
			_verticalMovemntAdjuster = 1;
		}
		if (transform.position.x < _minHorizontalBound)
		{
			_horizontalMovementAdjuster = 1;
		}
		else if (transform.position.x > _maxHorizontalBound)
		{
			_horizontalMovementAdjuster = -1;
		}
	}

	void MoveEnemy()
	{
		//  move enemy at 4 m/s
		// either down or angled across the screen
		//  if bottom of screen respawn at top with new random postion

		switch (_enemyMovementID)
		{
			case EnemyMovementID.Straight:
				transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
				_respawnable = true;
				break;
			case EnemyMovementID.Angled:
				transform.Translate((Vector3.down + Vector3.right) * _enemySpeed * Time.deltaTime);
				_respawnable = true;
				break;
			case EnemyMovementID.Bounce:
				EnemyBounce();
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
