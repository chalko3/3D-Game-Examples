using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JohnLemonMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float moveSpeed = 10f;
    public float JumpForce = 10f;
    public float GravityModifer = 1f;
    public bool IsOnGround = true;
    Rigidbody _Rigidbody;
    Vector3 _Movement;
    Quaternion _Rotation = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        Physics.gravity *= GravityModifer;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsOnGround)
        {
            _Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            IsOnGround = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _Movement.Set(horizontal, 0f, vertical);
        _Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, _Movement, turnSpeed * Time.deltaTime, 0f);
        _Rotation = Quaternion.LookRotation(desiredForward);

        _Rigidbody.MovePosition(_Rigidbody.position + _Movement * moveSpeed * Time.deltaTime);
        _Rigidbody.MoveRotation(_Rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            IsOnGround = true;
        }
    }
}