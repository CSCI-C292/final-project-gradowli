using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    float _moveSpeed = 4f;
    float _jumpHeight = 3f;
    float _verticalVelocity = 0f;
    float _horizontalVelocity = 0f;
    float _gravity = -8f;
    bool _grounded = false;
    bool _jumping = true;
    bool _c = false;
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalVelocity = 1.5f * Input.GetAxis("Horizontal");

        _grounded = controller.isGrounded;

        _verticalVelocity += _gravity * Time.deltaTime;

        if (_grounded) {
            _verticalVelocity = 0f;
            _jumping = false;
        }

        _c = Input.GetKeyDown("c");

        if (! _jumping && _c) {
            _verticalVelocity = _jumpHeight;
            _jumping = true;
        }

        Movement();

    }

    void Movement() {
        Vector3 sidewaysMovementVector = transform.right * _horizontalVelocity;
        Vector3 upDownMovementVector = transform.up * _verticalVelocity;
        Vector3 movementVector = sidewaysMovementVector + upDownMovementVector;
        
        GetComponent<CharacterController>().Move(movementVector * _moveSpeed * Time.deltaTime);
    }
    
}
