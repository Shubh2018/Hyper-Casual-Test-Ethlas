using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    private Player[] _playersInScene;

    private Player _playerWon;
    private Player _playerLost;

    #region Properties

    public Player PlayerLost { set { _playerLost = value; } }

    #endregion

    private static GameManager _instance;

    public static GameManager Instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }

        else if(_instance != this)
        {
            Destroy(_instance.gameObject);
            _instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    new private void OnEnable()
    {
        _playersInScene = new Player[2];
        _playersInScene = FindObjectsOfType<Player>();
    }

    new private void OnDisable()
    {
        _playersInScene = null;
    }

    public void CheckWinner()
    {
        for(int i = 0; i < _playersInScene.Length; i++)
        {
            if (_playersInScene[i] == _playerLost)
                break;
            else
                _playerWon = _playersInScene[i];
        }

        Debug.Log("Winner: " + _playerWon.name);
        Debug.Log("Loser: " + _playerLost.name);
    }
}
