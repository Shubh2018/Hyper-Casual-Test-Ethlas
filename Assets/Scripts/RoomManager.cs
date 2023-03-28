using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_Text _createRoomName;
    public TMP_Text _joinRoomName;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(_createRoomName.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_joinRoomName.text);
    }
}
