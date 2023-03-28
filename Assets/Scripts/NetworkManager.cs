using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Player Prefab")]
    [SerializeField] private Player _playerPrefab;

    [Header("Player Spawn Transforms")]
    [SerializeField] private Transform _playerOneSpawn;
    [SerializeField] private Transform _playerTwoSpawn;

    [Header("Game Network Properties")]
    [SerializeField] private string _gameVersion;
    [SerializeField] private int _maxPlayers;

    [Header("UI Panels")]
    [SerializeField] private Transform _startPanel;
    [SerializeField] private Transform _lobbyPanel;
    [SerializeField] private Transform _roomPanel;

    [Header("Wall Transform")]
    [SerializeField] private Transform _wall;

    private void Awake()
    {
        DontDestroyOnLoad(this);    
    }

    private void Start()
    {
        _startPanel.gameObject.SetActive(true);
        _lobbyPanel.gameObject.SetActive(false);
        _roomPanel.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = _gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        _startPanel.gameObject.SetActive(false);
        _lobbyPanel.gameObject.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        _lobbyPanel.gameObject.SetActive(false);
        _roomPanel.gameObject.SetActive(true);

        if (PhotonNetwork.CurrentRoom.PlayerCount == _maxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.Instantiate(_playerPrefab.name, _playerOneSpawn.position, Quaternion.identity);
        }

        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.Instantiate(_playerPrefab.name, _playerTwoSpawn.position, Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f)));
        }

        _wall.gameObject.SetActive(true);
        _roomPanel.gameObject.SetActive(false);
    }

/*    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        
    }*/
}
