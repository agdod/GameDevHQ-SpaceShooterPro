using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	// Spawn Game objects every 5 seconds
	// create a coroutine of type ienumerator -- yield events
	//Enemies
	[SerializeField] private GameObject _enemyContainer;
	[SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private float _delay = 5.0f;
	[SerializeField] private Vector3 _spawnOffset = new Vector3(0, 6, 0);
	//PowerUps
	[SerializeField] private GameObject _powerupTripleShotPrefab;
	//[SerializeField] private float _powerupDelay;
	private bool _stopSpawning = false;

	private void Start()
	{
		StartCoroutine(SpawnEnemyRoutine());
		StartCoroutine(SpawnPowerup());
	}

	IEnumerator SpawnEnemyRoutine()
	{
		while (_stopSpawning == false)
		{
			GameObject newEnemy = Instantiate(_enemyPrefab, transform.position + _spawnOffset, Quaternion.identity);
			newEnemy.transform.parent = _enemyContainer.transform;
			yield return new WaitForSeconds(_delay);
		}
	}

	IEnumerator SpawnPowerup()
	{
		while (_stopSpawning == false)
		{
			float _powerupDelay = Random.Range(3f, 7f);
			Vector3 spawnPos = new Vector3(Random.Range(-9f, 9f), 8, 0);
			Instantiate(_powerupTripleShotPrefab, spawnPos, Quaternion.identity);
			yield return new WaitForSeconds(_powerupDelay);
		}
	}
	public void StopSpawning()
	{
		_stopSpawning = true;
	}
}
