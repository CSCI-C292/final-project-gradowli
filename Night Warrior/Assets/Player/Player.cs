using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    float _moveSpeed = 4f;
    float _jumpHeight = 3f;
    float _bounceHeight = 1.5f;
    float _verticalVelocity = 0f;
    float _horizontalVelocity = 0f;
    float _gravity = -8f;
    bool _grounded = false;
    bool _jumping = true;
    bool _c = false;
    int _ceilingCount = 0;
    int _bounceCount = 0;
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

        if (_grounded && _bounceCount == 0) {
            _verticalVelocity = 0f;
            _jumping = false;
        }

        _c = Input.GetKeyDown("c");

        if (! _jumping && _c) {
            _verticalVelocity = _jumpHeight;
            _jumping = true;
        }

        Movement();

        if (_ceilingCount > 0) {
            _ceilingCount--;
        } 
        if (_bounceCount > 0) {
            _bounceCount--;
        }

    }

    void Movement() {
        Vector3 sidewaysMovementVector = transform.right * _horizontalVelocity;
        Vector3 upDownMovementVector = transform.up * _verticalVelocity;
        Vector3 movementVector = sidewaysMovementVector + upDownMovementVector;
        
        GetComponent<CharacterController>().Move(movementVector * _moveSpeed * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit collider) {
        if (_ceilingCount == 0 && (controller.collisionFlags & CollisionFlags.Above) != 0) { //stops upward velocity if player hits head
            _verticalVelocity = 0f;
            _ceilingCount = 20;
        }

        if (collider.transform.CompareTag("KillPlayer")){
            GameEvents.InvokeResetPlayer();
            //GameEvents.InvokeScoreIncreased(-20);
            Destroy(this.gameObject);
        }
        else if (collider.transform.CompareTag("KillPlayerDestroy")){
            GameEvents.InvokeResetPlayer();
            //GameEvents.InvokeScoreIncreased(-20);
            Destroy(this.gameObject);
            Destroy(collider.gameObject);
        }
        else if (collider.transform.CompareTag("KillEnemyBounce")) {
            collider.transform.parent.gameObject.SetActive(false);
            _verticalVelocity = _bounceHeight;
            _jumping = true;
            _bounceCount = 10;
        }
        else if (collider.transform.CompareTag("Checkpoint")) {
            collider.gameObject.GetComponent<BossCheckpoint>().ActivateBoss();
            collider.gameObject.SetActive(false);
            
        }
    }
    
}
