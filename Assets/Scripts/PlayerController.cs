using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	//  Player Movment  Variables
	[SerializeField] private float _speed = 1.0f;
	[SerializeField] private float _yLowerBound = -2.0f;
	[SerializeField] private float _yUpperBound = 0.0f;
	[SerializeField] private int _lives = 3;

	// Firing
	[SerializeField] private GameObject _laserPrefab;
	[SerializeField] private Vector3 _laserOffset;
	[SerializeField] private GameObject _tripleShotPrefab;
	[SerializeField] private float _fireRate;
	private float _canFire = -1f;


	[SerializeField] private GameObject _shieldEffect;
	[SerializeField] private bool _isTripleShotActive; // Serialized for debugging and testing
	[SerializeField] private bool _isShieldActive;
	[SerializeField] private float _speedModifier = 1.5f;
	[SerializeField] private float _coolDown = 5.0f;

	[SerializeField] private int _score = 0;

	private SpawnManager _spawnManager;
	[SerializeField] private UI_Manager _uiManager;

	void Start()
	{
		// Make sure powerups are off
		_isTripleShotActive = false;
		_isShieldActive = false;
		_shieldEffect.SetActive(false);

		// Set player postion to zero
		transform.position = Vector3.zero;

		// initilise score
		AddScore(0);
		_spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
		if (_spawnManager == null)
		{
			Debug.LogError("No SpawnManger in Scene. Insert Spawn manager into scene.");
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

	void FireLaser()
	{
		_canFire = Time.time + _fireRate;
		if (_isTripleShotActive)
		{
			Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
		}
		else
		{
			Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
		}
	}

	public void Damage()
	{
		// If shield is active no player damage, shield is destroyed.
		if (_isShieldActive)
		{
			// Disable shield Effect
			_shieldEffect.SetActive(false);
			_isShieldActive = false;
			// no player damage 
			return;
		}

		_lives--;
		if (_lives < 1)
		{
			Debug.Log("Player Dead. Game Over");
			_spawnManager.StopSpawning();
			Destroy(gameObject);
		}
	}

	public void AddScore(int points)
	{
		_score += points;
		_uiManager.UpdateScore(_score);
	}

	/* **** Power Ups **** */

	public void ActivateSpeedBoost()
	{
		_speed *= _speedModifier;
		StartCoroutine(CoolDown("Speed"));
	}

	public void ActivateTripleShot()
	{
		_isTripleShotActive = true;
		StartCoroutine(CoolDown("TripleShot"));
	}

	public void ActivateShield()
	{
		_isShieldActive = true;
		_shieldEffect.SetActive(true);
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
				break;
			case "Speed":
				_speed /= _speedModifier;
				break;
			default:
				Debug.Log("unidentified PowerUp!");
				break;
		}
	}
}
