using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkullEnemy : MonoBehaviour
{
    bool _directionRight = false;
    float _speed = 2f;
    int _collisionCount = 0;

    void Awake()
    {
        GameEvents.ResetPlayer += OnResetPlayer;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

    void OnCollisionEnter(Collision collision) {
        if (_collisionCount == 0) {
            _directionRight = ! _directionRight;
            _collisionCount = 10;
        }
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
        this.gameObject.SetActive(true);
    }
}
