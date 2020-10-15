using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private enum FiringType
	{
		Normal,
		TripleShot,
		Homing
	}

	//  Player Movment  Variables
	[SerializeField] private float _speed = 1.0f;
	[SerializeField] private float _yLowerBound = -2.0f;
	[SerializeField] private float _yUpperBound = 0.0f;
	[SerializeField] private int _lives = 3;


	// Firing
	[SerializeField] private FiringType _firingType;
	[SerializeField] private GameObject _laserPrefab;
	[SerializeField] private Vector3 _laserOffset;
	[SerializeField] private GameObject _tripleShotPrefab;
	[SerializeField] private float _fireRate;
	[SerializeField] private int _ammoCount = 15;
	[SerializeField] private int _ammoRefill = 15;
	private bool _homingLaser = false;

	private float _canFire = -1f;

	//Power Ups
	[SerializeField] private GameObject _shieldEffect;
	[SerializeField] private bool _isTripleShotActive; // Serialized for debugging and testing
	[SerializeField] private bool _isShieldActive;
	[SerializeField] private int _shieldLives = 3;
	[SerializeField] private float _speedModifier = 2.0f;
	[SerializeField] private float _coolDown = 5.0f;

	//Player Damage
	[SerializeField] private AudioClip _explosionAudioFX;
	[SerializeField] private GameObject[] _playerDamage;

	[SerializeField] private int _score = 0;

	[SerializeField] private UI_Manager _uiManager;

	private float _oldSpeedModifier;
	private bool _beenHit = false;  // Set true if hit by enemy laser
	private AudioSource _audioSource;
	private SpawnManager _spawnManager;
	private SpriteRenderer _shieldSpriteRenderer;


	void Start()
	{
		// Set player postion to zero
		transform.position = Vector3.zero;

		// Make sure powerups are off
		_isTripleShotActive = false;
		_isShieldActive = false;
		_shieldEffect.SetActive(false);
		_firingType = FiringType.Normal;

		// make sure Damage visuals is off
		foreach (GameObject damage in _playerDamage)
		{
			damage.SetActive(false);
		}

		// Update /Reset Ammo, Live, Score
		if (_uiManager != null)
		{
			_uiManager.UpdateAmmo(_ammoCount);
			_uiManager.UpdateLives(_lives);
			_uiManager.UpdateScore(_score);
		}
		else
		{
			Debug.LogError("No UI Manager found.");
		}

		// Get various componets
		_shieldSpriteRenderer = _shieldEffect.GetComponent<SpriteRenderer>();

		_spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
		if (_spawnManager == null)
		{
			Debug.LogError("No SpawnManger in Scene. Insert Spawn manager into scene.");
		}

		_audioSource = GetComponent<AudioSource>();
		if (_audioSource == null)
		{
			Debug.LogError("No AudioSource component found.");
		}
		else
		{
			if (_explosionAudioFX != null)
			{
				_audioSource.clip = _explosionAudioFX;
			}
		}
	}

	void Update()
	{
		PlayerMovement();
		// Fire laser at speed _fireRate
		if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
		{
			FireLaser();
		}
		// THrusters - move at increased rate wiht "left shift" key
		// use the already _speedmodifier variable
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			_speed = _speed * (_speedModifier * 0.8f);
		}
		if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			_speed = _speed / (_speedModifier * 0.8f);
		}


	}

	void PlayerMovement()
	{
		// Get vertical and horizontal Inputs
		float horizontalInput = Input.GetAxis("Horizontal");
		float verticalInput = Input.GetAxis("Vertical");

		// calcutale speed and direction
		Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
		transform.Translate(direction * _speed * Time.deltaTime);

		// Limit player bounds on y and x axis
		// Clamp on y axis
		transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _yLowerBound, _yUpperBound), 0);

		// bound on x axis
		if (transform.position.x > 11)
		{
			transform.position = new Vector3(-11, transform.position.y, 0);
		}
		else if (transform.position.x <= -11)
		{
			transform.position = new Vector3(11, transform.position.y, 0);
		}
	}

	void FireNormal()
	{
		if (_ammoCount > 0)
		{
			Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
			_ammoCount--;
			_uiManager.UpdateAmmo(_ammoCount);
		}
		else if (_ammoCount == 0)
		{
			_uiManager.OutOfAmmo();
			// Play out of ammo Sound - a beeep.
		}
	}

	Transform GetTarget()
	{
		// Get first enemy in Spawn manager container as target.
		GameObject container = _spawnManager.EnemyContainer;
		Debug.Log("Container has a child cont of :" + container.transform.childCount);
		if (container.transform.childCount > 0)
		{
			Transform enemyTarget = container.transform.GetChild(0);
			return enemyTarget;
		}
		else
		{
			return null;
		}
	}

	void FireHoming()
	{
		Debug.Log("homing laser fired");
		// Collect laser Game'Object fired.
		GameObject laserFire = Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
		Laser laser = laserFire.GetComponent<Laser>();
		//Set the target of fired laser
		if (GetTarget() != null)
		{
			laser.HomingLaserFired(GetTarget());
		}
		else
		{
			FireNormal();
		}

	}

	void FireLaser()
	{
		_canFire = Time.time + _fireRate;
		switch (_firingType)
		{
			case FiringType.Normal:
				FireNormal();
				break;
			case FiringType.TripleShot:
				Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
				break;
			case FiringType.Homing:
				FireHoming();
				break;
		}
		//if (_isTripleShotActive)
		//{
		//	
		//}
		//else
		//{


		//}
	}

	//Receive damage from enemy laser
	/* two laser hit but require only one set of damge to be called
	 * first laser hit set flag hasbeenhit
	 * on second laser if hasbeenhit = true then set hasbeenhit to false
	 */

	public void DoubleHitCheck()
	{
		// First enemy laser to hit
		if (_beenHit == false)
		{
			_beenHit = true;
			Damage();
		}
		// Second enemy laser hits - doesnt call damage routine
		// Note works for 2 laser enemy only!
		else
		{
			_beenHit = false;
		}
	}

	void ShieldDamage()
	{
		_shieldLives--;
		Color shieldColor = _shieldSpriteRenderer.color;

		// Change color of shield according to lives left (using RGB values)
		switch (_shieldLives)
		{
			case 0:
				// Disable shield Effect
				_shieldEffect.SetActive(false);
				_isShieldActive = false;
				break;
			case 1:
				shieldColor = new Color(255, 0, 0);
				_shieldSpriteRenderer.color = shieldColor;
				break;
			case 2:
				shieldColor = new Color(255, 0, 255);
				_shieldSpriteRenderer.color = shieldColor;
				break;
			default:
				// if shield lives goes negative.
				_shieldLives = 0;
				_shieldEffect.SetActive(false);
				_isShieldActive = false;
				break;
		}
	}

	public void Damage()
	{
		// If shield is active no player damage, shield is destroyed.
		if (_isShieldActive)
		{
			ShieldDamage();
			// No player Damage 
			return;
		}

		_lives--;
		_uiManager.UpdateLives(_lives);
		// Check if dead first
		if (_lives < 1)
		{
			DestroyPlayer();
		}

		// If not dead then display  player damage
		if (_playerDamage != null)
		{
			if (_lives == 2)
			{
				// If has 2 lives display left or right at random for damage
				_playerDamage[Random.Range(0, 2)].SetActive(true);
			}
			else
			{
				// Set both to active for damage display
				_playerDamage[0].SetActive(true);
				_playerDamage[1].SetActive(true);
			}
		}
	}

	void DestroyPlayer()
	{
		Debug.Log("Player Dead. Game Over");
		if (_audioSource != null && _explosionAudioFX != null)
		{
			_audioSource.Play();
		}
		_spawnManager.StopSpawning();
		Destroy(gameObject);
	}

	public void AddScore(int points)
	{
		_score += points;
		_uiManager.UpdateScore(_score);
	}

	/* **** Power Ups **** */

	public void ActivateHomingLaser()
	{
		_firingType = FiringType.Homing;
		StartCoroutine(CoolDown("Homing"));
	}

	public void ActivateHealth()
	{
		// Update visuals for play damage if any first
		if (_playerDamage != null)
		{
			// One live left - has both thuster damage, disbale one thruster damage.
			if (_lives == 1)
			{
				_playerDamage[0].SetActive(false);
			}
			else if (_lives == 2)
			{
				// Either left or right can show damage so both need disabling
				_playerDamage[1].SetActive(false);
				_playerDamage[0].SetActive(false);
			}
		}
		_lives++;
		// Restrict lives to max of 3
		if (_lives > 3)
		{
			_lives = 3;
		}
		_uiManager.UpdateLives(_lives);
	}

	public void ActivateAmmoRefill()
	{
		_ammoCount += _ammoRefill;
		_uiManager.UpdateAmmo(_ammoCount);
	}

	public void ActivateSpeedBoost()
	{
		_speed *= _speedModifier;
		StartCoroutine(CoolDown("Speed"));
	}

	public void ActivateTripleShot()
	{
		_isTripleShotActive = true;
		_firingType = FiringType.TripleShot;
		StartCoroutine(CoolDown("TripleShot"));
	}

	public void ActivateShield()
	{
		_isShieldActive = true;
		_shieldEffect.SetActive(true);
		// Reset shield lives and full shield color.
		_shieldLives = 3;
		Color shieldColor = new Color(255, 255, 255);
		_shieldSpriteRenderer.color = shieldColor;
		// Maybe enable cooldown... but with longer timeout....
		// StartCoroutine(CoolDown("Shield"));
	}

	IEnumerator CoolDown(string powerUp)
	{
		yield return new WaitForSeconds(_coolDown);
		switch (powerUp)
		{
			case "TripleShot":
				_isTripleShotActive = false;
				_firingType = FiringType.Normal;
				break;
			case "Speed":
				_speed /= _speedModifier;
				break;
			case "Homing":
				_firingType = FiringType.Normal;
				break;
			default:
				Debug.Log("unidentified PowerUp!");
				break;
		}
	}
}
