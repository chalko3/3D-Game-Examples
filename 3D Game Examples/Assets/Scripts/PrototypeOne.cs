using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 10f;
    public float RotationalSpeed = 60f;

    private float _horizontalInput;
    private float _verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _verticalInput = Input.GetAxis("Vertical");
        _horizontalInput = Input.GetAxis("Horizontal");
        
        transform.Translate(Vector3.forward * Speed * _verticalInput * Time.deltaTime);
        transform.Rotate(Vector3.up, RotationalSpeed * _horizontalInput * Time.deltaTime);
    }
}