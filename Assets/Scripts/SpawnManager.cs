using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
	// Spawn Game objects every 5 seconds
	// create a coroutine of type ienumerator -- yield events
	[SerializeField] private GameObject _enemyContainer;
	[SerializeField] private GameObject _enemyPrefab;
	[SerializeField] private float _delay = 5.0f;
	[SerializeField] private Vector3 _spawnOffset = new Vector3(0, 6, 0);
	private bool _stopSpawning = false;
	private void Start()
	{
		StartCoroutine(SpawnRoutine());
	}

	IEnumerator SpawnRoutine()
	{
		while (_stopSpawning == false)
		{
			GameObject newEnemy = Instantiate(_enemyPrefab, transform.position+_spawnOffset, Quaternion.identity);
			newEnemy.transform.parent = _enemyContainer.transform;
			yield return new WaitForSeconds(_delay);
		}
	}

	public void StopSpawning()
	{
		_stopSpawning = true;
	}
}
