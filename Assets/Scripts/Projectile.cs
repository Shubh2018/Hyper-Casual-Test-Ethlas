using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviourPunCallbacks
{
    [Header("Projectile Properties")]
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _lifeTimeInSeconds;
    [SerializeField] private int _damage = 10;

    #region Properties

    public float ProjectileSpeed { get { return _projectileSpeed; } }

    public Vector3 Velocity { set { GetComponent<Rigidbody2D>().velocity = value; } }

    public int Damage { get { return _damage; } }

    #endregion

    new private void OnEnable()
    {
        StartCoroutine(DestroyAfterSeconds(_lifeTimeInSeconds));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag(TagIndex.PLAYER))
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    private IEnumerator DestroyAfterSeconds(float _time)
    {
        yield return new WaitForSeconds(_time);

        PhotonNetwork.Destroy(this.gameObject);
    }
}
