using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShot : MonoBehaviour
{
    // Start is called before the first frame update
    public float _horizontalVelocity = 8f;
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
        Vector3 movementVector = transform.right * _horizontalVelocity;
        transform.position += movementVector * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.transform.CompareTag("KillPlayer") || collision.collider.transform.CompareTag("KillEnemyBounce")){
            GameEvents.InvokeScoreIncreased(20);
            collision.collider.transform.parent.gameObject.SetActive(false);
        }
        else if (collision.collider.transform.CompareTag("KillPlayerBoss") || collision.collider.transform.CompareTag("KillEnemyBounceBoss")){
            GameEvents.InvokeScoreIncreased(100);
            collision.collider.transform.parent.gameObject.SetActive(false);
        }
        else if (collision.collider.transform.CompareTag("KillPlayerDestroy")){
            Destroy(collision.collider.gameObject);
        }
        Destroy(transform.gameObject);
    }

    void OnResetPlayer(object sender, EventArgs args) {
        Destroy(this.gameObject);
    }

    void OnDestroy() {
        GameEvents.ResetPlayer -= OnResetPlayer;
    }
}
