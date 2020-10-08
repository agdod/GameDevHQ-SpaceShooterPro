using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    /* Move down at defined speed
     * When leave screen, destroy
     * OnTriggerCollsion
     *      Only collecable by player
     *      on collected destroy
     */
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _lowerBounds = -3.5f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < _lowerBounds)
		{
            Destroy(gameObject);
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
            //activate powerup
            Destroy(gameObject);
		}
	}
}
