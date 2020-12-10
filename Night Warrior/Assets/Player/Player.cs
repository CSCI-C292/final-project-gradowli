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
    bool _v = false;
    int _ceilingCount = 0;
    int _bounceCount = 0;
    public bool _super = false;
    int _portalCooldown = 0;
    int _groundedCount = 0;
    int _shotCount = 0;
    bool _directionRight = true;
    bool _gameWon = false;

    void Awake() {
        GameEvents.ResetPlayer += OnResetPlayer;
        GameEvents.PlayerWin += OnPlayerWin;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (! _gameWon) {
            if (_super) {
                _moveSpeed = 6f;
            }
            else {
                _moveSpeed = 4f;
            }

            _horizontalVelocity = 1.5f * Input.GetAxis("Horizontal");

            if (_horizontalVelocity > 0) {
                if (! _directionRight) {
                    _directionRight = true;
                    gameObject.GetComponent<SpriteRenderer>().flipX = false;
                }
            }
            else if (_horizontalVelocity < 0) {
                if (_directionRight) {
                    _directionRight = false;
                    gameObject.GetComponent<SpriteRenderer>().flipX = true;
                }
            }

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
                RaycastHit collision;
                for (float i = 0; i < transform.GetComponent<BoxCollider>().bounds.extents.x; i += transform.GetComponent<BoxCollider>().bounds.extents.x/5) {
                    if (Physics.Raycast(transform.position - new Vector3(i, 0, 0), Vector3.down, out collision, transform.GetComponent<BoxCollider>().bounds.extents.y + 0.1f)) {
                        if (collision.collider.transform.CompareTag("KillEnemyBounce")) {
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
                        else if (collision.collider.transform.CompareTag("Super")) {
                            _super = true;
                            collision.collider.gameObject.SetActive(false);
                        }
                        else if (collision.collider.transform.CompareTag("Ammo")){
                            _shotCount = 3;
                            collision.collider.gameObject.SetActive(false);
                        }
                        else {
                            _grounded = true;
                        }
                    }
                    if (Physics.Raycast(transform.position + new Vector3(i, 0, 0), Vector3.down, out collision, transform.GetComponent<BoxCollider>().bounds.extents.y + 0.1f)) {
                        if (collision.collider.transform.CompareTag("KillEnemyBounce")) {
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
                        else if (collision.collider.transform.CompareTag("Super")) {
                            _super = true;
                            collision.collider.gameObject.SetActive(false);
                        }
                        else if (collision.collider.transform.CompareTag("Ammo")){
                            _shotCount = 3;
                            collision.collider.gameObject.SetActive(false);
                        }
                        else {
                            _grounded = true;
                        }
                    }
                }
            }

            if (_grounded && _bounceCount == 0) {
                _verticalVelocity = 0f;
                _jumping = false;
            }

            if (_horizontalVelocity > 0) {
                for (float i = 0; i < transform.GetComponent<BoxCollider>().bounds.extents.y; i += transform.GetComponent<BoxCollider>().bounds.extents.y/5) {
                    RaycastHit collision;
                    if (Physics.Raycast(transform.position - new Vector3(0, i, 0), Vector3.right, out collision, transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                        if (! collision.collider.transform.CompareTag("Pushable")) {
                            _horizontalVelocity = 0f;
                        }
                        else {
                            for (float j = 0; j < collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.y; j += collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.y/5) {
                                if (Physics.Raycast(collision.collider.transform.position - new Vector3(0, j, 0), Vector3.right, collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                                    _horizontalVelocity = 0f;
                                }
                                if (Physics.Raycast(collision.collider.transform.position + new Vector3(0, j, 0), Vector3.right, collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                                    _horizontalVelocity = 0f;
                                }
                            }
                        }
                    }
                    if (Physics.Raycast(transform.position + new Vector3(0, i, 0), Vector3.right, out collision, transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                        if (! collision.collider.transform.CompareTag("Pushable")) {
                            _horizontalVelocity = 0f;
                        }
                        else {
                            for (float j = 0; j < collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.y; j += collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.y/5) {
                                if (Physics.Raycast(collision.collider.transform.position - new Vector3(0, j, 0), Vector3.right, collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                                    _horizontalVelocity = 0f;
                                }
                                if (Physics.Raycast(collision.collider.transform.position + new Vector3(0, j, 0), Vector3.right, collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                                    _horizontalVelocity = 0f;
                                }
                            }
                        }
                    }
                }
            }
            else {
                for (float i = 0; i < transform.GetComponent<BoxCollider>().bounds.extents.y; i += transform.GetComponent<BoxCollider>().bounds.extents.y/5) {
                    RaycastHit collision;
                    if (Physics.Raycast(transform.position - new Vector3(0, i, 0), Vector3.left, out collision, transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                        if (! collision.collider.transform.CompareTag("Pushable")) {
                            _horizontalVelocity = 0f;
                        }
                        else {
                            for (float j = 0; j < collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.y; j += collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.y/5) {
                                if (Physics.Raycast(collision.collider.transform.position - new Vector3(0, j, 0), Vector3.left, collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                                    _horizontalVelocity = 0f;
                                }
                                if (Physics.Raycast(collision.collider.transform.position + new Vector3(0, j, 0), Vector3.left, collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                                    _horizontalVelocity = 0f;
                                }
                            }
                        }
                    }
                    if (Physics.Raycast(transform.position + new Vector3(0, i, 0), Vector3.left, out collision, transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                        if (! collision.collider.transform.CompareTag("Pushable")) {
                            _horizontalVelocity = 0f;
                        }
                        else {
                            for (float j = 0; j < collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.y; j += collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.y/5) {
                                if (Physics.Raycast(collision.collider.transform.position - new Vector3(0, j, 0), Vector3.left, collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                                    _horizontalVelocity = 0f;
                                }
                                if (Physics.Raycast(collision.collider.transform.position + new Vector3(0, j, 0), Vector3.left, collision.collider.transform.GetComponent<BoxCollider>().bounds.extents.x + 0.02f)) {
                                    _horizontalVelocity = 0f;
                                }
                            }
                        }
                    }
                }
            }

            _c = Input.GetKeyDown("c");
            _x = Input.GetKeyDown("x");
            _v = Input.GetKeyDown("v");
            
            if (_v) {
                RaycastHit collision;
                if (_directionRight) {
                    if (Physics.Raycast(transform.position, Vector3.right, out collision, transform.GetComponent<BoxCollider>().bounds.extents.x + 1f)) {
                        if (collision.collider.transform.CompareTag("Pushable")) {
                            collision.collider.GetComponent<Pushable>().ResetLocation();
                        }
                    }
                }
                else {
                    if (Physics.Raycast(transform.position, Vector3.left, out collision, transform.GetComponent<BoxCollider>().bounds.extents.x + 1f)) {
                        if (collision.collider.transform.CompareTag("Pushable")) {
                            collision.collider.GetComponent<Pushable>().ResetLocation();
                        }
                    }
                }
            }

            if (! _jumping && _c) {
                _verticalVelocity = _jumpHeight;
                _jumping = true;
                _grounded = false; 
            }

            if (_x && _shotCount > 0) {
                --_shotCount;
                if (_directionRight) {
                    GameObject shot = Instantiate(_shotPrefab, new Vector3(transform.position.x + 1, transform.position.y, 0f), Quaternion.identity);
                    shot.GetComponent<PlayerShot>()._horizontalVelocity = 8f;
                }
                else {
                    GameObject shot = Instantiate(_shotPrefab, new Vector3(transform.position.x - 1, transform.position.y, 0f), Quaternion.identity);
                    shot.GetComponent<PlayerShot>()._horizontalVelocity = -8f;
                    shot.GetComponent<PlayerShot>().GetComponent<SpriteRenderer>().flipX = true;
                }
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
                collision.collider.transform.parent.gameObject.SetActive(false);
                GameEvents.InvokeResetPlayer();
            }
            else {
                _super = false;
                collision.collider.transform.parent.gameObject.SetActive(false);
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
                Destroy(collision.collider.gameObject);
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
        else if (CheckPortal(collision)) {

        }
        else if (collision.collider.transform.CompareTag("Super")) {
            _super = true;
            collision.collider.gameObject.SetActive(false);
        }
        else if (collision.collider.transform.CompareTag("Ammo")){
            _shotCount = 3;
            collision.collider.gameObject.SetActive(false);
        }
        else if (collision.collider.transform.CompareTag("Pushable")){
            collision.collider.GetComponent<Pushable>()._horizontalVelocity = _horizontalVelocity; 
        }
        else if (collision.collider.transform.CompareTag("Win")){
            collision.collider.gameObject.SetActive(false);
            GameEvents.InvokePlayerWin(); 
        }
    }

    Boolean CheckPortal(Collision collision) {
        if (_portalCooldown == 0 && collision.collider.transform.CompareTag("Portal1")) {
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
            return true;
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
            return true;
        }
        else {
            return false;
        }
    }

    void OnCollisionStay(Collision collision) {
        if (collision.collider.transform.CompareTag("Pushable")){
            collision.collider.GetComponent<Pushable>()._horizontalVelocity = _horizontalVelocity;
        }
    }

    void OnCollisionExit(Collision collision) {
        if (collision.collider.transform.CompareTag("Pushable")){
            collision.collider.GetComponent<Pushable>()._horizontalVelocity = 0f;
        }
    }

    IEnumerator DestroyPlayer() {
        yield return new WaitForEndOfFrame();
        Destroy(this.gameObject);
    }

    void OnResetPlayer(object sender, EventArgs args) {
        StartCoroutine(DestroyPlayer());
    }

    void OnPlayerWin(object sender, EventArgs args) {
        _gameWon = true;
    }

    void OnDestroy() {
        GameEvents.ResetPlayer -= OnResetPlayer;
    }
    
}
