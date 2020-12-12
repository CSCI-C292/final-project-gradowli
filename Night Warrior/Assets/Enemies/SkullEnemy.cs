using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkullEnemy : MonoBehaviour
{
    public bool _directionRight = false;
    float _speed = 2f;
    int _collisionCount = 0;
    public bool _resetOnDeath = true;
    bool _gameWon = false;

    void Awake()
    {
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
            if (_directionRight) {
                transform.position += new Vector3(_speed * Time.deltaTime, 0, 0);
            }
            else {
                transform.position -= new Vector3(_speed * Time.deltaTime, 0, 0);
            }

            if (_collisionCount > 0) {
                --_collisionCount;
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (_collisionCount == 0 && ! collision.collider.CompareTag("KillPlayerDestroy")) {
            _directionRight = ! _directionRight;
            _collisionCount = 10;
            if (_directionRight) {
                gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
            else {
                gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
        }
    }

    void OnResetPlayer(object sender, EventArgs args) {
        if (_resetOnDeath) {
            this.gameObject.SetActive(true);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy() {
        GameEvents.ResetPlayer -= OnResetPlayer;
        GameEvents.PlayerWin -= OnPlayerWin;
    }

    void OnPlayerWin(object sender, EventArgs args) {
        _gameWon = true;
    }
}
