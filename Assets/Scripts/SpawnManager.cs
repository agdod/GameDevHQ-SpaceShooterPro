using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	//Enemies
	[SerializeField] private GameObject _enemyContainer;
	[SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private float _delay = 5.0f;
	[SerializeField] private Vector3 _spawnOffset = new Vector3(0, 6, 0);

	//PowerUps
	[SerializeField] private GameObject[] _powerupPrefab;

	private bool _stopSpawning = false;

	private void Start()
	{
		Debug.Log("PowerUp prefba length : " + _powerupPrefab.Length);
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
			float posX = Random.Range(-9f, 9f);
			Vector3 spawnPos = new Vector3(posX, 0, 0) + _spawnOffset;
			int index = Random.Range(0, _powerupPrefab.Length);
			Instantiate(_powerupPrefab[index], spawnPos, Quaternion.identity);
			yield return new WaitForSeconds(_powerupDelay);
		}
	}

	public void StopSpawning()
	{
		_stopSpawning = true;
	}
}
