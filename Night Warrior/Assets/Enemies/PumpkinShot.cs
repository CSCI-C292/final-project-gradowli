﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PumpkinShot : MonoBehaviour
{
    public float _horizontalVelocity = -4f;
    public float _verticalVelocity = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        GameEvents.ResetPlayer += OnResetPlayer;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 sidewaysMovementVector = transform.right * _horizontalVelocity;
        Vector3 upDownMovementVector = transform.up * _verticalVelocity;
        Vector3 movementVector = sidewaysMovementVector + upDownMovementVector;
        transform.position += movementVector * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Player")) {
            Destroy(collision.collider.gameObject);
            GameEvents.InvokeResetPlayer();
        }
        Destroy(transform.gameObject);
    }

    void OnResetPlayer(object sender, EventArgs args) {
        Destroy(this.gameObject);
    }
}