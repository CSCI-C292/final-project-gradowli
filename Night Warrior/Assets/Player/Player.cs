using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public bool _super = false;
    int _portalCooldown = 0;
    
    CharacterController controller;

    void Awake() {
        GameEvents.ResetPlayer += OnResetPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_super) {
            _moveSpeed = 6f;
        }
        else {
            _moveSpeed = 4f;
        }

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
        if (_portalCooldown > 0) {
            _portalCooldown--;
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
            if (! _super) {
                GameEvents.InvokeScoreIncreased(-50);
                GameEvents.InvokeResetPlayer();
            }
            else {
                _super = false;
            }
        }
        else if (collider.transform.CompareTag("KillPlayerDestroy")){
            if (! _super) {
                GameEvents.InvokeScoreIncreased(-50);
                Destroy(collider.gameObject);
                GameEvents.InvokeResetPlayer();
            }
            else {
                _super = false;
            }
        }
        else if (collider.transform.CompareTag("KillEnemyBounce")) {
            collider.transform.parent.gameObject.SetActive(false);
            GameEvents.InvokeScoreIncreased(20);
            _verticalVelocity = _bounceHeight;
            _jumping = true;
            _bounceCount = 10;
        }
        else if (collider.transform.CompareTag("KillEnemyBounceBoss")) {
            collider.transform.parent.gameObject.SetActive(false);
            GameEvents.InvokeScoreIncreased(100);
            _verticalVelocity = _bounceHeight;
            _jumping = true;
            _bounceCount = 10;
        }
        else if (collider.transform.CompareTag("Checkpoint")) {
            collider.gameObject.GetComponent<BossCheckpoint>().ActivateBoss();
            collider.gameObject.SetActive(false);
        }
        else if (_portalCooldown == 0 && collider.transform.CompareTag("Portal1")) {
            _portalCooldown = 20;
            _verticalVelocity = 0f;
            _horizontalVelocity = 0f;
            if (this.transform.position.x < collider.transform.position.x) {
                this.transform.position = new Vector3(collider.transform.parent.GetChild(1).gameObject.transform.position.x + 1,
                                                    collider.transform.parent.GetChild(1).gameObject.transform.position.y, 0);
            }
            else {
                this.transform.position = new Vector3(collider.transform.parent.GetChild(1).gameObject.transform.position.x - 1,
                                                    collider.transform.parent.GetChild(1).gameObject.transform.position.y, 0);
            }
        }
        else if (_portalCooldown == 0 && collider.transform.CompareTag("Portal2")) {
            _verticalVelocity = 0f;
            _horizontalVelocity = 0f;
            _portalCooldown = 20;
            if (this.transform.position.x < collider.transform.position.x) {
                this.transform.position = new Vector3(collider.transform.parent.GetChild(0).gameObject.transform.position.x + 1,
                                                    collider.transform.parent.GetChild(0).gameObject.transform.position.y, 0);
            }
            else {
                this.transform.position = new Vector3(collider.transform.parent.GetChild(0).gameObject.transform.position.x - 1,
                                                    collider.transform.parent.GetChild(0).gameObject.transform.position.y, 0);
            }
        }
        else if (collider.transform.CompareTag("Super")) {
            _super = true;
            collider.gameObject.SetActive(false);
        }
    }

    IEnumerator DestroyPlayer() {
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }

    void OnResetPlayer(object sender, EventArgs args) {
        StartCoroutine(DestroyPlayer());
    }

    void OnDestroy() {
        GameEvents.ResetPlayer -= OnResetPlayer;
    }
    
}
