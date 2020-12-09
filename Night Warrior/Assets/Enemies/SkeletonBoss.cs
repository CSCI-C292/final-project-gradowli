using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkeletonBoss : MonoBehaviour
{
    [SerializeField] GameObject _axePrefab;
    bool _directionRight = false;
    float _speed = 1f;
    int _collisionCount = 0;
    int _axeCount = 0;
    int _maxCount;

    void Awake()
    {
        GameEvents.ResetPlayer += OnResetPlayer;
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
        if (_directionRight) {
            transform.position += new Vector3(_speed * Time.deltaTime, 0, 0);
        }
        else {
            transform.position -= new Vector3(_speed * Time.deltaTime, 0, 0);
        }

        if (_collisionCount > 0) {
            --_collisionCount;
        }

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

    }

    void OnCollisionEnter(Collision collision) {
        if (_collisionCount == 0) {
            _directionRight = ! _directionRight;
            _collisionCount = 10;
        }
    }

    void OnResetPlayer(object sender, EventArgs args) {
        this.gameObject.SetActive(false);
    }
}
