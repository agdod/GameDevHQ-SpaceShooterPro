using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    
 
    void Start()
    {
        // take current positon = new postion (0,0,0)
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // Get vertical and horizontal Inputs
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        // calcutale speeds
        float verticalSpeed = 1 * verticalInput * _speed * Time.deltaTime;
        float horizontalSpeed = 1 * horizontalInput * _speed * Time.deltaTime;
        transform.Translate(horizontalSpeed,verticalSpeed,0);
    }
}
