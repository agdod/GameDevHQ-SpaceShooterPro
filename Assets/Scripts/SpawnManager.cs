using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Spawn Game objects every 5 seconds
    // create a coroutine of type ienumerator -- yield events
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float delay = 5.0f;
	private void Start()
	{
		StartCoroutine(SpawnRoutine());
	}

	IEnumerator SpawnRoutine()
	{
        while (true)
		{
            Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(delay);
		}
	}
   
}
