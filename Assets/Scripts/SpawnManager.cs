using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using Random = UnityEngine.Random;



public class SpawnManager : MonoBehaviour
{
	[Serializable]
	public class PowerUpPrefab
	{
		[SerializeField] private GameObject _prefab;
		public enum Probability
		{
			Normal = 0,
			Rare = 6
		};

		[SerializeField] private Probability _probability;
		private int countDown = 6;

		// Getters
		public GameObject Prefab
		{
			get { return _prefab; }
		}

		public bool Active
		{
			// if Power up is classed as rare it becomes active every 1 in X calls
			get
			{
				if (_probability == Probability.Normal)
				{
					return true;
				}
				else
				{
					if (countDown == 0)
					{
						countDown = (int)Probability.Rare;
						return true;
					}
					else
					{
						countDown--;
						return false;
					}
				}
			}
		}
	}

	[SerializeField] [Range(0, 5)] private float _initalDelay = 3.0f;
	//Enemies
	[SerializeField] private GameObject _enemyContainer;
	[SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private float _delay = 5.0f;
	[SerializeField] private Vector3 _spawnOffset = new Vector3(0, 6, 0);

	//PowerUps
	//[SerializeField] private GameObject[] _powerupPrefab;

	[SerializeField] private PowerUpPrefab[] _powerupPrefab;

	private bool _stopSpawning = false;

	// Getters
	public GameObject EnemyContainer
	{
		get { return _enemyContainer; }
	}

	IEnumerator SpawnEnemyRoutine()
	{
		yield return new WaitForSeconds(_initalDelay);
		while (_stopSpawning == false)
		{
			GameObject newEnemy = Instantiate(_enemyPrefab, transform.position + _spawnOffset, Quaternion.identity);
			newEnemy.transform.parent = _enemyContainer.transform;
			yield return new WaitForSeconds(_delay);
		}
	}

	IEnumerator SpawnPowerup()
	{
		yield return new WaitForSeconds(_initalDelay);
		while (_stopSpawning == false)
		{
			float _powerupDelay = Random.Range(3f, 7f);
			float posX = Random.Range(-9f, 9f);
			Vector3 spawnPos = new Vector3(posX, 0, 0) + _spawnOffset;
			bool notSpawned = false;

			// Spawn an active Powerup, if selected isnt active random pick again.
			while (notSpawned == false)
			{
				int index = Random.Range(0, _powerupPrefab.Length);
				if (_powerupPrefab[index].Active == true)
				{
					Instantiate(_powerupPrefab[index].Prefab, spawnPos, Quaternion.identity);
					notSpawned = true;
				}
			}

			yield return new WaitForSeconds(_powerupDelay);
		}
	}

	public void StartSpawning()
	{
		StartCoroutine(SpawnEnemyRoutine());
		StartCoroutine(SpawnPowerup());
	}

	public void StopSpawning()
	{
		_stopSpawning = true;
	}
}
