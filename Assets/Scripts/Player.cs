using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 mousePosition;
    private float _nextTimeToFire = 0.0f;

    [Header("Weapon Properties")]
    [SerializeField] private Transform _gunHolder;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private int _projectileCount = 10;

    [Header("Fire Rate")]
    [SerializeField] private float _fireRate = 0.25f;

    #region Properties

    public int ProjectileCount { get { return _projectileCount; } set { _projectileCount = value; } }

    #endregion

    private void Update()
    {
        Aim();
    }

    private void Aim()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = (mousePosition - _gunHolder.transform.position).normalized;
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
            _projectileCount -= 1;

            Projectile projectile = Instantiate(_projectile, _projectileSpawnPoint.transform.position, Quaternion.identity);
            projectile.Velocity = direction * projectile.ProjectileSpeed;
            projectile.transform.parent = this.transform;
        }
    }
}
