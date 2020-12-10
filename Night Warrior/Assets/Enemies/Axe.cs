using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Axe : MonoBehaviour
{
    public float _verticalVelocity = 3f;
    public float _horizontalVelocity = -2f;
    float _gravity = -4f;
    float _moveSpeed = 2f;
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
            _verticalVelocity += _gravity * Time.deltaTime;
            Vector3 sidewaysMovementVector = new Vector3(_horizontalVelocity, 0, 0);
            Vector3 upDownMovementVector = new Vector3(0, _verticalVelocity, 0);
            Vector3 movementVector = sidewaysMovementVector + upDownMovementVector;
            
            transform.position += movementVector * _moveSpeed * Time.deltaTime;
            transform.Rotate(new Vector3(0,0,5));
        }
    }

    void OnCollisionEnter(Collision collision) {
        Destroy(transform.gameObject);
    }

    void OnResetPlayer(object sender, EventArgs args) {
        Destroy(this.gameObject);
    }

    void OnDestroy() {
        GameEvents.ResetPlayer -= OnResetPlayer;
        GameEvents.PlayerWin -= OnPlayerWin;
    }

    void OnPlayerWin(object sender, EventArgs args) {
        Destroy(gameObject);
    }
}
