using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    private Vector3 _mousePosition;
    private float _nextTimeToFire = 0.0f;

    private PhotonView _photonView;

    private bool _hasOpponentWon = false;
    private bool _hasWon = false;
    private int _playerHealth = 20;

    [Header("NickName")]
    [SerializeField] private string _nickName;

    [Header("GFX Color")]
    [SerializeField] private SpriteRenderer _gfx;

    [Header("Weapon Properties")]
    [SerializeField] private Transform _gunHolder;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Transform _projectileSpawnPoint;

    [Header("Fire Rate")]
    [SerializeField] private float _fireRate = 0.25f;

    private const byte WIN_CONDITION_EVENT = 1;
    private const byte LOSE_CONDITION_EVENT = 0;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        PhotonNetwork.NickName = _nickName;
    }

    private void Update()
    {
        if(_photonView.IsMine)
            Aim();
    }

    new private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    new private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag(TagIndex.PROJECTILE))
        {
            if (collision.transform.parent != this.transform)
            {
                _photonView.RPC("UpdateHealth", RpcTarget.AllViaServer, new object[] { collision.transform.GetComponent<Projectile>().Damage });
                //_playerHealth -= collision.transform.GetComponent<Projectile>().Damage;

                if (_playerHealth < 0)
                {
                    _playerHealth = 0;
                    //_photonView.RPC("LoseCheckRPC", RpcTarget.AllViaServer);
                    //_hasLost = true;
                    //NetworkManager.Instance.PlayerLost = this;
                    //NetworkManager.Instance.CheckWinner();
                    /*_hasWon = false;
                    _hasOpponentWon = true;*/
                    HasLost();
                }
            }
        }
    }

    [PunRPC]
    void DestroyGameObject(GameObject go)
    {
        PhotonNetwork.Destroy(go);
    }

    private void Aim()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = (_mousePosition - _gunHolder.transform.position).normalized;
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _gunHolder.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotZ);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot(direction);
        }
    }

    private void Shoot(Vector3 direction)
    {
        if (_nextTimeToFire < Time.time)
        {
            _nextTimeToFire = Time.time + _fireRate;

            GameObject projectile = PhotonNetwork.Instantiate(_projectile.name, _projectileSpawnPoint.transform.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().Velocity = direction * projectile.GetComponent<Projectile>().ProjectileSpeed;
            projectile.transform.parent = this.transform;
        }
    }

    /*public void HasWon()
    {
        _hasWon = true;
        RaiseEventOptions raiseEventOptions;

        if (PhotonNetwork.IsMasterClient)
            raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        else
            raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };

        SendOptions send = new SendOptions { Reliability = true };
        object HasWon = _hasWon;

        PhotonNetwork.RaiseEvent(WIN_CONDITION_EVENT, HasWon, raiseEventOptions, send);
    }*/
    
    public void HasLost()
    {
        _hasWon = false;

        RaiseEventOptions raiseEventOptions;

        if (PhotonNetwork.IsMasterClient)
            raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        else
            raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };

        SendOptions send = new SendOptions { Reliability = true };
        object HasWon = _hasWon;

        PhotonNetwork.RaiseEvent(LOSE_CONDITION_EVENT, HasWon, raiseEventOptions, send);
    }

   /* [PunRPC]
    private void LoseCheckRPC()
    {
        _hasWon = false;
        _hasOpponentWon = true;
    }*/

    [PunRPC]
    private void UpdateHealth(int damage)
    {
        _playerHealth -= damage;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*if (stream.IsReading)
        {
            _hasWon = !(bool)stream.ReceiveNext();
            _hasOpponentWon = !(bool)stream.ReceiveNext();
        }

        else if (stream.IsWriting)
        {
            stream.SendNext(_hasWon);
            stream.SendNext(_hasOpponentWon);
        }*/
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        /*if(obj.Code == WIN_CONDITION_EVENT)
        {
            object data = (object)obj.CustomData;

            this._hasOpponentWon = (bool)data;
        }*/

        if (obj.Code == LOSE_CONDITION_EVENT)
        {
            object data = (object)obj.CustomData;

            this._hasOpponentWon = (bool)data;
            this._hasWon = !(bool)data;
        }
    }
}
