using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    [SerializeField] public GameObject _player;
    public bool _gameOver = false;
    int _endCount = 200;
    bool _bossDropped = false;
    bool _gameWon = false;

    void Awake()
    {
        GameEvents.PlayerWin += OnPlayerWin;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (! _gameOver && _player != null){
            transform.position = new Vector3(Math.Max(0, _player.transform.position.x), 0, -10);
        } 
        else if (_gameWon) {
            if (_endCount == 0 && ! _bossDropped) {
                _bossDropped = true;
                GameEvents.InvokeDropBoss();
            }
            else if (_endCount == 0) {
                
            }
            else if (_endCount > 100) {
                _endCount--;
            }
            else {
                _endCount--;
                transform.position += new Vector3(0.1f, 0f, 0f);
            }
        }
    }

    void OnPlayerWin(object sender, EventArgs args) {
        _gameOver = true;
        _gameWon = true;
    }
}
