using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    [Header("Player Prefab")]
    [SerializeField] private Player _playerPrefab;

    [Header("Player Spawn Transforms")]
    [SerializeField] private Transform _playerOneSpawn;
    [SerializeField] private Transform _playerTwoSpawn;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        
    }
}
