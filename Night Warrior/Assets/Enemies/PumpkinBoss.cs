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
    int _groundedCount = 0;
    int _shotCount = 300;
    [SerializeField] GameObject _shotPrefab;  
    // Start is called before the first frame update
    void Awake()
    {
        GameEvents.ResetPlayer += OnResetPlayer;
    }

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // help with Raycast to find if grounded:
        // https://answers.unity.com/questions/196381/how-do-i-check-if-my-rigidbody-player-is-grounded.html

        if (_shotCount == 0 && ! _grounded && _jumpCount == 0) {
            GameObject topShot = Instantiate(_shotPrefab, new Vector3(transform.position.x - 1.5f, transform.position.y + 1, 0f), Quaternion.identity);
            GameObject middleShot = Instantiate(_shotPrefab, new Vector3(transform.position.x - 1.5f, transform.position.y, 0f), Quaternion.identity);
            GameObject bottomShot = Instantiate(_shotPrefab, new Vector3(transform.position.x - 1.5f, transform.position.y - 1, 0f), Quaternion.identity);
            topShot.GetComponent<PumpkinShot>()._verticalVelocity = 2f;
            bottomShot.GetComponent<PumpkinShot>()._verticalVelocity = -2f;
            _shotCount = 300;
        }

        if (_groundedCount == 0) {
            _grounded = Physics.Raycast(transform.GetChild(0).transform.position, -Vector3.up, transform.GetChild(0).GetComponent<BoxCollider>().bounds.extents.y + 0.2f);
        }
        _verticalVelocity += _gravity * Time.deltaTime;
        if (_grounded) {
            _verticalVelocity = 0f;
            _groundedCount = 20;
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
        if (_groundedCount > 0) {
            _groundedCount--;
        }
        if (_shotCount > 0) {
            _shotCount--;
        }

    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            if (! collision.collider.GetComponent<Player>()._super) {
                GameEvents.InvokeResetPlayer();
                GameEvents.InvokeScoreIncreased(-50);
            }
            else {
                collision.collider.GetComponent<Player>()._super = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    void OnResetPlayer(object sender, EventArgs args) {
        this.gameObject.SetActive(false);
    }
}
