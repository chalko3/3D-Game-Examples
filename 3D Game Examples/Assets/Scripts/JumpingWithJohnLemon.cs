using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JohnLemonMovement : MonoBehaviour
{
    public int score = 0;
    public float turnSpeed = 20f;
    public float moveSpeed = 1f;
    public float JumpForce = 10f;
    public float GravityModifer = 1f;
    public float outOfBounds = -10f;
    public bool IsOnGround = true;
    public bool isAtCheckpoint = false;
    public GameObject checkpointAreaObject;
    public GameObject finishAreaObject;
    Vector3 _Movement;
    Rigidbody _Rigidbody;
    Quaternion _Rotation = Quaternion.identity;
    private Vector3 _defaultGravity = new Vector3(0f, -9.81f, 0f);
    private Vector3 _startingPosition;
    private Vector3 _checkpointPosition;
    private GameObject[] _collectibles;

    // Start is called before the first frame update
    void Start()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        Physics.gravity = _defaultGravity;
        Physics.gravity *= GravityModifer;
        _startingPosition = transform.position;
        _collectibles = GameObject.FindGameObjectsWithTag("Collectible-Return");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && IsOnGround)
        {
            _Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            IsOnGround = false;
        }

        if(transform.position.y < outOfBounds)
        {
            if(isAtCheckpoint)
            {
                ReturningCollectibles();
                transform.position = _checkpointPosition;
            }
            else
            {
                ReturningCollectibles();
                transform.position = _startingPosition;
            }
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

        if(collision.gameObject.CompareTag("Spinner"))
        {
            if(isAtCheckpoint)
            {
                ReturningCollectibles();
                transform.position = checkpointAreaObject.transform.position;
            }
            else
            {
                ReturningCollectibles();
                transform.position = _startingPosition;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == checkpointAreaObject)
        {
            isAtCheckpoint = true;
            _checkpointPosition = checkpointAreaObject.transform.position;
        }

        if(other.gameObject == finishAreaObject)
        {
            isAtCheckpoint = false;
            ReturningCollectibles();
            transform.position = _startingPosition;
        }

        if(other.gameObject.CompareTag("Collectible-Destroy"))
        {
            score++;
            Destroy(other.gameObject);
        }

        if(other.gameObject.CompareTag("Collectible-Return"))
        {
           score++;
           other.gameObject.GetComponent<Collectibles>().HideCollectibles();
        }
    }

    void ReturningCollectibles()
    {
        for(int i = 0; i < _collectibles.Length; i++)
        {
            _collectibles[i].SetActive(true);
            _collectibles[i].GetComponent<Collectibles>().ReturnCollectibles();
        }
    }
}