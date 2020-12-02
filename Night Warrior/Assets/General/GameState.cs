using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameState : MonoBehaviour
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _level;
    GameObject _currentPlayer;
    float _xSpawn = -7.906882f;
    float _ySpawn = -3.527989f;
    int _lives = 1;

    public static GameState Instance;

    void Awake() {
        Instance = this;
        GameEvents.ResetPlayer += OnResetPlayer;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnResetPlayer(object sender, EventArgs args) {
        if (_lives > 0) {
            _lives--;
            _currentPlayer = Instantiate(_playerPrefab, new Vector3(_xSpawn, _ySpawn, 0f), Quaternion.identity);
            Camera.main.GetComponent<CameraController>()._player = _currentPlayer; 
        }
        else {
            Camera.main.GetComponent<CameraController>()._gameOver = true;
            _level.SetActive(false);
        }
    }
}
