using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PumpkinBoss : MonoBehaviour
{
    int _jumpCount = 0;
    float _gravity = -8f;
    float _verticalVelocity = 0f;
    bool _grounded = false;
    int _shotCount = 0;
    bool _gameWon = false;
    [SerializeField] GameObject _shotPrefab;  
    // Start is called before the first frame update
    void Awake()
    {
        GameEvents.ResetPlayer += OnResetPlayer;
        GameEvents.PlayerWin += OnPlayerWin;
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (! _gameWon) {
            // help with Raycast to find if grounded:
            // https://answers.unity.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html

            if (_shotCount == 0 && ! _grounded && _jumpCount == 0) {
                GameObject topShot = Instantiate(_shotPrefab, new Vector3(transform.position.x - 1.5f, transform.position.y + 1, 0f), Quaternion.identity);
                GameObject middleShot = Instantiate(_shotPrefab, new Vector3(transform.position.x - 1.5f, transform.position.y, 0f), Quaternion.identity);
                GameObject bottomShot = Instantiate(_shotPrefab, new Vector3(transform.position.x - 1.5f, transform.position.y - 1, 0f), Quaternion.identity);
                topShot.GetComponent<PumpkinShot>()._verticalVelocity = 2f;
                bottomShot.GetComponent<PumpkinShot>()._verticalVelocity = -2f;
                _shotCount = 125;
            }

            _grounded = Physics.Raycast(transform.GetChild(0).transform.position, -Vector3.up, transform.GetChild(0).GetComponent<BoxCollider>().bounds.extents.y + 0.2f);
            _verticalVelocity += _gravity * Time.deltaTime;
            if (_grounded) {
                _verticalVelocity = 0f;
            }

            if (_jumpCount == 0 && _grounded) {
                if (UnityEngine.Random.Range(0,50) == 0) {
                    _verticalVelocity = UnityEngine.Random.Range(4,10);
                    _jumpCount = 50;
                    _grounded = false;
                }
            }

            Vector3 upDownMovementVector = transform.up * _verticalVelocity;
            transform.position += upDownMovementVector * Time.deltaTime;

            if (_jumpCount > 0) {
                _jumpCount--;
            }
            if (_shotCount > 0) {
                _shotCount--;
            }
        }
    }


    void OnResetPlayer(object sender, EventArgs args) {
        this.gameObject.SetActive(false);
    }

    void OnPlayerWin(object sender, EventArgs args) {
        _gameWon = true;
    }

    void OnDestroy() {
        GameEvents.ResetPlayer -= OnResetPlayer;
        GameEvents.PlayerWin -= OnPlayerWin;
    }
}
