using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    [SerializeField] private RectTransform _backgroundCircle1, _backgroundCircle2;
    [SerializeField] private RectTransform _playerCircle;

    [SerializeField] private float _initTimer;
    [SerializeField] private float _initDistanceTimer;
    [SerializeField] private Text _scoreText2;

    [SerializeField] private float _bgMinSize;
    [SerializeField] private float _bgMaxSize;
    [SerializeField] private float _bgMinDistance;
    [SerializeField] private float _bgMaxDistance;
    [SerializeField] private float _bgMinSpeed;
    [SerializeField] private float _bgMaxSpeed;
    [SerializeField] private float _addAmount;

    [SerializeField] private GameObject _gameOverModal;
    [SerializeField] private Text _scoreText, _timerText;


    private float _survived;
    private bool _started, _gameOver;
    private float _timer;
    private float _newDistanceTimer;
    private float _imaginaryCircleRadius;
    private float _bgCircleDistance;
    private float _bgCurrCircleDistance;
    private float _bgCircleSpeed;
    private float _bgCurrCircleSpeed;

    private void Start()
    {
        _timer = _initTimer;
        _newDistanceTimer = _initDistanceTimer;
        _bgCurrCircleDistance = _bgCircleDistance = Random.Range(_bgMinDistance, _bgMaxDistance);
        _bgCurrCircleSpeed = _bgCircleSpeed = Random.Range(_bgMinSpeed, _bgMaxSpeed);
    }

    public void QuitGame ()
    {
    	Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (_gameOver) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerCircle.sizeDelta += Vector2.one * _addAmount;
            _started = true;
        }


        if (!_started) return;

        if (_playerCircle.sizeDelta.x < _imaginaryCircleRadius - _bgCircleDistance / 2 ||
            _playerCircle.sizeDelta.x > _imaginaryCircleRadius + _bgCircleDistance / 2)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _survived += Time.deltaTime;
        }

        _timerText.text = _timer.ToString("N2");
        _scoreText.text = _survived.ToString("N2");
        _scoreText2.text = _survived.ToString("N2");

        _newDistanceTimer -= Time.deltaTime;

        if (_newDistanceTimer <= 0)
        {
            _bgCurrCircleSpeed = Random.Range(_bgMinSpeed, _bgMaxSpeed);
            _bgCurrCircleDistance = Random.Range(_bgMinDistance, _bgMaxDistance);
            _newDistanceTimer = _initDistanceTimer;
        }

        _bgCircleDistance = Mathf.MoveTowards(_bgCircleDistance, _bgCurrCircleDistance, Time.deltaTime * 5f);
        _bgCircleSpeed = Mathf.MoveTowards(_bgCircleSpeed, _bgCurrCircleSpeed, Time.deltaTime * 5f);

        _backgroundCircle1.sizeDelta = Vector2.one * (_imaginaryCircleRadius - (_bgCircleDistance / 2));
        _backgroundCircle2.sizeDelta = Vector2.one * (_imaginaryCircleRadius + (_bgCircleDistance / 2));

        _playerCircle.sizeDelta -= Vector2.one * _bgMaxSpeed * 7f * Time.deltaTime;

        var sin = Mathf.Abs(Mathf.Sin(Time.time * Mathf.Deg2Rad * _bgCircleSpeed));
        _imaginaryCircleRadius = Mathf.Lerp(_bgMinSize, _bgMaxSize, sin);
        if (_timer <= 0)
        {
            _gameOverModal.SetActive(true);
            _gameOver = true;
        }
    }
}