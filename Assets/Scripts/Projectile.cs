using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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

    private void OnEnable()
    {
        Destroy(this.gameObject, _lifeTimeInSeconds);
    }
}
