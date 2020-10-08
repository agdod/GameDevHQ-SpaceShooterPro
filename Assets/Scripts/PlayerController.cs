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
	private SpawnManager _spawnManager;
	[SerializeField] private bool _isTripleShotActive; // Serialized for debugging and testing
	[SerializeField] private float _coolDown = 5.0f;


	void Start()
	{
		// Set player postion to zero
		_isTripleShotActive = false;
		transform.position = Vector3.zero;
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
		//Clamp on y axis
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
		_lives--;
		if (_lives < 1)
		{
			Debug.Log("Player Dead. Game Over");
			_spawnManager.StopSpawning();
			Destroy(gameObject);
		}
	}

	public void ActivateTripleShot()
	{
		_isTripleShotActive = true;
		StartCoroutine(CoolDown());
	}

	IEnumerator CoolDown()
	{
		yield return new WaitForSeconds(_coolDown);
		_isTripleShotActive = false;

	}
}
