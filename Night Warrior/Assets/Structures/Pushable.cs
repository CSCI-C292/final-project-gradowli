using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pushable : MonoBehaviour
{
    float _verticalVelocity = 0f;
    public float _horizontalVelocity = 0f;
    float _gravity = 0f;
    float _moveSpeed = 4f;
    float _startingX;
    float _startingY;

    void Awake()
    {
        GameEvents.ResetPlayer += OnResetPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        _startingX = transform.position.x;
        _startingY = transform.position.y;
        _gravity = -4f;
    }

    // Update is called once per frame
    void Update()
    {
        _verticalVelocity += _gravity * Time.deltaTime;

        bool grounded = false;

        if (_verticalVelocity < 0) {
            for (float i = 0; i < transform.GetComponent<BoxCollider>().bounds.extents.x; i += transform.GetComponent<BoxCollider>().bounds.extents.x/5) {
                if (Physics.Raycast(transform.position - new Vector3(i, 0, 0), Vector3.down, transform.GetComponent<BoxCollider>().bounds.extents.y + 0.1f)) {
                    grounded = true;
                }
                if (Physics.Raycast(transform.position + new Vector3(i, 0, 0), Vector3.down, transform.GetComponent<BoxCollider>().bounds.extents.y + 0.1f)) {
                    grounded = true;
                }
            }
        }
        

        if (grounded) {
            _verticalVelocity = 0f;
        }

        bool left = false;
        bool right = false;

        
        for (float i = 0; i < transform.GetComponent<BoxCollider>().bounds.extents.y; i += transform.GetComponent<BoxCollider>().bounds.extents.y/5) {
            if (Physics.Raycast(transform.position - new Vector3(0, i, 0), Vector3.right, transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                right = true;
                if (_horizontalVelocity > 0) {
                    _horizontalVelocity = 0f;
                }
            }
            if (Physics.Raycast(transform.position + new Vector3(0, i, 0), Vector3.right, transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                right = true;
                if (_horizontalVelocity > 0) {
                    _horizontalVelocity = 0f;
                }
            }
        }
        
        for (float i = 0; i < transform.GetComponent<BoxCollider>().bounds.extents.y; i += transform.GetComponent<BoxCollider>().bounds.extents.y/5) {
            if (Physics.Raycast(transform.position - new Vector3(0, i, 0), Vector3.left, transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                left = true;
                if (_horizontalVelocity < 0) {
                    _horizontalVelocity = 0f;
                }
            }
            if (Physics.Raycast(transform.position + new Vector3(0, i, 0), Vector3.left, transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                left = true;
                if (_horizontalVelocity < 0) {
                    _horizontalVelocity = 0f;
                }
            }
        }

        if ((! left) && (! right)) {
            _horizontalVelocity = 0f;
        }

        Vector3 sidewaysMovementVector = new Vector3(_horizontalVelocity, 0, 0);
        Vector3 upDownMovementVector = new Vector3(0, _verticalVelocity, 0);
        Vector3 movementVector = sidewaysMovementVector + upDownMovementVector;
        
        transform.position += movementVector * _moveSpeed * Time.deltaTime;
    }

    void OnResetPlayer(object sender, EventArgs args) {
        ResetLocation();
    }

    public void ResetLocation() {
        //Help found online to check if a position is occupied
        //https://answers.unity.com/questions/434100/check-if-a-spawn-point-it-occupied.html?_ga=2.159947129.53386711.1607450197-204514712.1598574612
        Vector3 spawn = new Vector3(_startingX, _startingY, 0);
        var colliders = Physics.OverlapSphere(spawn, 0.1f);
        if(colliders.Length == 0) {
            transform.position = new Vector3(_startingX, _startingY, 0);
        }
    }
    void OnDestroy() {
        GameEvents.ResetPlayer -= OnResetPlayer;
    }
}
