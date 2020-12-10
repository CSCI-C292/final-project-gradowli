using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkeletonBoss : MonoBehaviour
{
    [SerializeField] GameObject _axePrefab; 
    [SerializeField] GameObject _skullPrefab;
    [SerializeField] GameObject _winObject;
    bool _directionRight = false;
    float _speed = 1f;
    int _collisionCount = 0;
    int _axeCount = 0;
    int _skullCount = 0;
    int _maxCount;
    int _winCount = 2000;
    Vector3 _skullSpawn1 = new Vector3(99f, -3f, 0);
    Vector3 _skullSpawn2 = new Vector3(86.2f, -3f, 0);
    bool _defeated = false;
    bool _gameWon = false;
    bool _grounded = false;
    float _gravity = -8f;
    float _verticalVelocity = 0f;
    bool _invokedEnd = false;
    int _endCount = 300;


    void Awake()
    {
        GameEvents.ResetPlayer += OnResetPlayer;
        GameEvents.PlayerWin += OnPlayerWin;
    }

    // Start is called before the first frame update
    void Start()
    {
        _maxCount = 100;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (! _gameWon) {
            if (_directionRight) {
                transform.position += new Vector3(_speed * Time.deltaTime, 0, 0);
            }
            else {
                transform.position -= new Vector3(_speed * Time.deltaTime, 0, 0);
            }

            if (_collisionCount > 0) {
                --_collisionCount;
            }

            _grounded = Physics.Raycast(transform.position, -Vector3.up, transform.GetComponent<BoxCollider>().bounds.extents.y + 0.2f);
            _verticalVelocity += _gravity * Time.deltaTime;
            if (_grounded) {
                _verticalVelocity = 0f;
            }

            Vector3 upDownMovementVector = transform.up * _verticalVelocity;
            transform.position += upDownMovementVector * Time.deltaTime;

            if (_axeCount == 0) {
                _axeCount = _maxCount;
                GameObject axe = Instantiate(_axePrefab, new Vector3(transform.position.x - 0.2f - transform.GetComponent<BoxCollider>().bounds.extents.x, transform.position.y + 0.5f + transform.GetComponent<BoxCollider>().bounds.extents.y, 0f), Quaternion.identity);
                float random = UnityEngine.Random.Range(-5f, -2f);
                axe.GetComponent<Axe>()._horizontalVelocity = random;
                axe.GetComponent<Axe>()._verticalVelocity = 3f;
            }

            if(_axeCount > 0) {
                _axeCount--;
            }
            
            if (_skullCount == 0) {
                _skullCount = _maxCount * 2;
                Collider[] colliders = Physics.OverlapSphere(_skullSpawn1, 0.5f);
                if(colliders.Length == 0) {
                    GameObject skull1 = Instantiate(_skullPrefab, _skullSpawn1, Quaternion.identity);
                    skull1.GetComponent<SkullEnemy>()._resetOnDeath = false;
                    skull1.GetComponent<SkullEnemy>().GetComponentInChildren<SpriteRenderer>().flipX = true;
                }
                else if (colliders[0].gameObject.CompareTag("Player")) {
                    GameEvents.InvokeResetPlayer();
                }

                colliders = Physics.OverlapSphere(_skullSpawn2, 0.5f);
                if(colliders.Length == 0) {
                    GameObject skull2 = Instantiate(_skullPrefab, _skullSpawn2, Quaternion.identity);
                    skull2.GetComponent<SkullEnemy>()._resetOnDeath = false;
                    skull2.GetComponent<SkullEnemy>()._directionRight = true;
                }
                else if (colliders[0].gameObject.CompareTag("Player")) {
                    GameEvents.InvokeResetPlayer();
                }
            }

            if (_skullCount > 0) {
                _skullCount--;
            }

            if (_winCount == 0 && ! _defeated) {
                _winObject.SetActive(true);
                _defeated = true;
            }

            if (_winCount > 0) {
                --_winCount;
            }
        }
        else {
            _grounded = Physics.Raycast(transform.position, -Vector3.up, transform.GetComponent<BoxCollider>().bounds.extents.y + 0.2f);
            _verticalVelocity += _gravity * Time.deltaTime;
            if (_grounded) {
                _verticalVelocity = 0f;
            }

            if (_grounded && ! _invokedEnd && _endCount == 0) {
                _invokedEnd = true;
                GameEvents.InvokeStartGameWon();
            }

            Vector3 upDownMovementVector = transform.up * _verticalVelocity;
            transform.position += upDownMovementVector * Time.deltaTime;

            if (_endCount > 0) {
                _endCount--;
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (_collisionCount == 0 && ! _gameWon) {
            _directionRight = ! _directionRight;
            _collisionCount = 10;
        }
    }

    void OnResetPlayer(object sender, EventArgs args) {
        _winCount = 2000;
        this.gameObject.SetActive(false);
    }

    void OnPlayerWin(object sender, EventArgs args) {
        _gameWon = true;
    }
}
