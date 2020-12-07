using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{

    [SerializeField] GameObject _shotPrefab;
    float _moveSpeed = 4f;
    float _jumpHeight = 3f;
    float _bounceHeight = 1.5f;
    float _verticalVelocity = 0f;
    float _horizontalVelocity = 0f;
    float _gravity = -8f;
    bool _grounded = false;
    bool _jumping = true;
    bool _c = false;
    bool _x = false;
    int _ceilingCount = 0;
    int _bounceCount = 0;
    public bool _super = false;
    int _portalCooldown = 0;
    int _groundedCount = 0;
    int _shotCount = 0;

    void Awake() {
        GameEvents.ResetPlayer += OnResetPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {

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

        _grounded = Physics.Raycast(transform.position, -Vector3.up, transform.GetComponent<BoxCollider>().bounds.extents.y + 0f);

        _verticalVelocity += _gravity * Time.deltaTime;

        if (_verticalVelocity > 0) {
            for (float i = 0; i < transform.GetComponent<BoxCollider>().bounds.extents.x; i += transform.GetComponent<BoxCollider>().bounds.extents.x/5) {
                if (Physics.Raycast(transform.position - new Vector3(i, 0, 0), Vector3.up, transform.GetComponent<BoxCollider>().bounds.extents.y + 0.02f)) {
                    _verticalVelocity = 0f;
                    _ceilingCount = 20;
                }
                if (Physics.Raycast(transform.position + new Vector3(i, 0, 0), Vector3.up, transform.GetComponent<BoxCollider>().bounds.extents.y + 0.02f)) {
                    _verticalVelocity = 0f;
                    _ceilingCount = 20;
                }
            }
        }
        else if (_groundedCount == 0) {
            for (float i = 0; i < transform.GetComponent<BoxCollider>().bounds.extents.x; i += transform.GetComponent<BoxCollider>().bounds.extents.x/5) {
                if (Physics.Raycast(transform.position - new Vector3(i, 0, 0), Vector3.down, transform.GetComponent<BoxCollider>().bounds.extents.y + 0.0f)) {
                    _grounded = true;
                }
                if (Physics.Raycast(transform.position + new Vector3(i, 0, 0), Vector3.down, transform.GetComponent<BoxCollider>().bounds.extents.y + 0.00f)) {
                    _grounded = true;
                }
            }
        }

        if (_grounded && _bounceCount == 0) {
            _verticalVelocity = 0f;
            _jumping = false;
        }

        if (_horizontalVelocity > 0) {
            for (float i = 0; i < transform.GetComponent<BoxCollider>().bounds.extents.y; i += transform.GetComponent<BoxCollider>().bounds.extents.y/5) {
                if (Physics.Raycast(transform.position - new Vector3(0, i, 0), Vector3.right, transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                _horizontalVelocity = 0f;
                }
                if (Physics.Raycast(transform.position + new Vector3(0, i, 0), Vector3.right, transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                _horizontalVelocity = 0f;
                }
            }
        }
        else {
            for (float i = 0; i < transform.GetComponent<BoxCollider>().bounds.extents.y; i += transform.GetComponent<BoxCollider>().bounds.extents.y/5) {
                if (Physics.Raycast(transform.position - new Vector3(0, i, 0), Vector3.left, transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                _horizontalVelocity = 0f;
                }
                if (Physics.Raycast(transform.position + new Vector3(0, i, 0), Vector3.left, transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                _horizontalVelocity = 0f;
                }
            }
        }

        _c = Input.GetKeyDown("c");
        _x = Input.GetKeyDown("x");

        if (! _jumping && _c) {
            _verticalVelocity = _jumpHeight;
            _jumping = true;
            _grounded = false; 
        }

        if (_x && _shotCount > 0) {
            --_shotCount;
            Instantiate(_shotPrefab, new Vector3(transform.position.x + 1, transform.position.y, 0f), Quaternion.identity);
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
        
        transform.position += movementVector * _moveSpeed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.transform.CompareTag("KillPlayer") || collision.collider.transform.CompareTag("KillPlayerBoss")){
            if (! _super) {
                GameEvents.InvokeScoreIncreased(-50);
                GameEvents.InvokeResetPlayer();
            }
            else {
                _super = false;
            }
        }
        else if (collision.collider.transform.CompareTag("KillPlayerDestroy")){
            if (! _super) {
                GameEvents.InvokeScoreIncreased(-50);
                Destroy(collision.collider.gameObject);
                GameEvents.InvokeResetPlayer();
            }
            else {
                _super = false;
            }
        }
        else if (collision.collider.transform.CompareTag("KillEnemyBounce")) {
            collision.collider.transform.parent.gameObject.SetActive(false);
            GameEvents.InvokeScoreIncreased(20);
            _verticalVelocity = _bounceHeight;
            _jumping = true;
            _bounceCount = 10;
        }
        else if (collision.collider.transform.CompareTag("KillEnemyBounceBoss")) {
            collision.collider.transform.parent.gameObject.SetActive(false);
            GameEvents.InvokeScoreIncreased(100);
            _verticalVelocity = _bounceHeight;
            _jumping = true;
            _bounceCount = 10;
        }
        else if (collision.collider.transform.CompareTag("Checkpoint")) {
            collision.collider.gameObject.GetComponent<BossCheckpoint>().ActivateBoss();
            collision.collider.gameObject.SetActive(false);
        }
        else if (_portalCooldown == 0 && collision.collider.transform.CompareTag("Portal1")) {
            _verticalVelocity = 0f;
            _horizontalVelocity = 0f;
            if (this.transform.position.x < collision.collider.transform.position.x) {
                this.transform.position = new Vector3(collision.collider.transform.parent.GetChild(1).gameObject.transform.position.x + 1,
                                                    collision.collider.transform.parent.GetChild(1).gameObject.transform.position.y, 0);
            }
            else {
                this.transform.position = new Vector3(collision.collider.transform.parent.GetChild(1).gameObject.transform.position.x - 1,
                                                    collision.collider.transform.parent.GetChild(1).gameObject.transform.position.y, 0);
            }
        }
        else if (_portalCooldown == 0 && collision.collider.transform.CompareTag("Portal2")) {
            _verticalVelocity = 0f;
            _horizontalVelocity = 0f;
            if (this.transform.position.x < collision.collider.transform.position.x) {
                this.transform.position = new Vector3(collision.collider.transform.parent.GetChild(0).gameObject.transform.position.x + 1,
                                                    collision.collider.transform.parent.GetChild(0).gameObject.transform.position.y, 0);
            }
            else {
                this.transform.position = new Vector3(collision.collider.transform.parent.GetChild(0).gameObject.transform.position.x - 1,
                                                    collision.collider.transform.parent.GetChild(0).gameObject.transform.position.y, 0);
            }
        }
        else if (collision.collider.transform.CompareTag("Super")) {
            _super = true;
            collision.collider.gameObject.SetActive(false);
        }
        else if (collision.collider.transform.CompareTag("Ammo")){
            _shotCount = 3;
            collision.collider.gameObject.SetActive(false);
        }
    }

    void OnCollisionStay(Collision collision) {
        if (collision.collider.transform.CompareTag("KillPlayer") || collision.collider.transform.CompareTag("KillPlayerBoss")){
            if (! _super) {
                GameEvents.InvokeScoreIncreased(-50);
                GameEvents.InvokeResetPlayer();
            }
            else {
                _super = false;
            }
        }
        else if (collision.collider.transform.CompareTag("KillPlayerDestroy")){
            if (! _super) {
                GameEvents.InvokeScoreIncreased(-50);
                Destroy(collision.collider.gameObject);
                GameEvents.InvokeResetPlayer();
            }
            else {
                _super = false;
            }
        }
        else if (collision.collider.transform.CompareTag("KillEnemyBounce")) {
            collision.collider.transform.parent.gameObject.SetActive(false);
            GameEvents.InvokeScoreIncreased(20);
            _verticalVelocity = _bounceHeight;
            _jumping = true;
            _bounceCount = 10;
        }
        else if (collision.collider.transform.CompareTag("KillEnemyBounceBoss")) {
            collision.collider.transform.parent.gameObject.SetActive(false);
            GameEvents.InvokeScoreIncreased(100);
            _verticalVelocity = _bounceHeight;
            _jumping = true;
            _bounceCount = 10;
        }
        else if (collision.collider.transform.CompareTag("Checkpoint")) {
            collision.collider.gameObject.GetComponent<BossCheckpoint>().ActivateBoss();
            collision.collider.gameObject.SetActive(false);
        }
        else if (_portalCooldown == 0 && collision.collider.transform.CompareTag("Portal1")) {
            _verticalVelocity = 0f;
            _horizontalVelocity = 0f;
            if (this.transform.position.x < collision.collider.transform.position.x) {
                this.transform.position = new Vector3(collision.collider.transform.parent.GetChild(1).gameObject.transform.position.x + 1,
                                                    collision.collider.transform.parent.GetChild(1).gameObject.transform.position.y, 0);
            }
            else {
                this.transform.position = new Vector3(collision.collider.transform.parent.GetChild(1).gameObject.transform.position.x - 1,
                                                    collision.collider.transform.parent.GetChild(1).gameObject.transform.position.y, 0);
            }
        }
        else if (_portalCooldown == 0 && collision.collider.transform.CompareTag("Portal2")) {
            _verticalVelocity = 0f;
            _horizontalVelocity = 0f;
            if (this.transform.position.x < collision.collider.transform.position.x) {
                this.transform.position = new Vector3(collision.collider.transform.parent.GetChild(0).gameObject.transform.position.x + 1,
                                                    collision.collider.transform.parent.GetChild(0).gameObject.transform.position.y, 0);
            }
            else {
                this.transform.position = new Vector3(collision.collider.transform.parent.GetChild(0).gameObject.transform.position.x - 1,
                                                    collision.collider.transform.parent.GetChild(0).gameObject.transform.position.y, 0);
            }
        }
        else if (collision.collider.transform.CompareTag("Super")) {
            _super = true;
            collision.collider.gameObject.SetActive(false);
        }
        else if (collision.collider.transform.CompareTag("Ammo")){
            _shotCount = 3;
            collision.collider.gameObject.SetActive(false);
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
