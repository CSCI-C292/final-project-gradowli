using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] public GameObject _player;
    public bool _gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (! _gameOver && _player != null){
            transform.position = new Vector3(Math.Max(0, _player.transform.position.x), 0, -10);
        } 
        else {

        }
    }
}
