﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameState : MonoBehaviour
{
    [SerializeField] GameObject _gameOverText1;
    [SerializeField] GameObject _gameOverText2;
    [SerializeField] GameObject _scoreText;
    [SerializeField] GameObject _timerText;
    [SerializeField] GameObject _livesText;
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _level;
    GameObject _currentPlayer;
    float _xSpawn = -7.906882f;
    float _ySpawn = -3.527989f;
    int _lives = 3;
    int _score = 0;
    int _maxTime = 100;
    int _time;
    bool _timerBool = true;
    bool _gameOver = false;

    public static GameState Instance;

    void Awake() {
        Instance = this;
        GameEvents.ResetPlayer += OnResetPlayer;
        GameEvents.ScoreIncreased += OnScoreIncreased;
    }
    // Start is called before the first frame update
    void Start()
    {
        _time = _maxTime;
    }

    IEnumerator DecreaseTime() {
        yield return new WaitForSeconds(1);
        if (! _gameOver && _time > 0) {
            _time--;
            _timerText.GetComponent<TextMeshProUGUI>().text = "Timer\n" + _time;
            _timerBool = true;
        }
        else if (! _gameOver) {
            _timerBool = true;
            GameEvents.InvokeResetPlayer();
            GameEvents.InvokeScoreIncreased(-50);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (! _gameOver && _timerBool) {
            StartCoroutine(DecreaseTime());
            _timerBool = false;
        }
    }

    public void IncreaseScore(int amount) {
        _score += amount;
        _scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + _score;
    }

    void OnScoreIncreased(object sender, ScoreEventArgs args) {
        IncreaseScore(args.score);
    }

    void DecreaseLife() {
        _lives--;
        _livesText.GetComponent<TextMeshProUGUI>().text = "Lives:  " + _lives;
    }

    void OnResetPlayer(object sender, EventArgs args) {
        if (_lives > 1) {
            DecreaseLife();
            _currentPlayer = Instantiate(_playerPrefab, new Vector3(_xSpawn, _ySpawn, 0f), Quaternion.identity);
            Camera.main.GetComponent<CameraController>()._player = _currentPlayer;
             _time = _maxTime;
            _timerText.GetComponent<TextMeshProUGUI>().text = "Timer\n" + _time;
            if (_timerBool) {
                StartCoroutine(DecreaseTime());
                _timerBool = false; 
            }
        }
        else {
            _gameOver = true;
            DecreaseLife();
            Camera.main.GetComponent<CameraController>()._gameOver = true;
            _level.SetActive(false);
            _timerBool = false;
            _timerText.GetComponent<TextMeshProUGUI>().text = "";
            _livesText.GetComponent<TextMeshProUGUI>().text = "";
            _scoreText.GetComponent<TextMeshProUGUI>().text = "";
            _gameOverText1.GetComponent<TextMeshProUGUI>().text = "GAME OVER";
            _gameOverText2.GetComponent<TextMeshProUGUI>().text = "YOUR SCORE WAS " + _score;
            _gameOverText1.SetActive(true);
            _gameOverText2.SetActive(true);
        }
    }
}
