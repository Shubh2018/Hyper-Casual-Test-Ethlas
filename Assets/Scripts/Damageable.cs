using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Damageable : MonoBehaviour
{
    private Player _player;
    private Health _health;

    private void Start()
    {
        _player = GetComponent<Player>();
        _health = GetComponent<Health>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag(TagIndex.PROJECTILE))
        {
            if(collision.transform.parent != this.transform)
            {
                _health.UpdateHealth(collision.transform.GetComponent<Projectile>().Damage);
            }

            else
            {
                _player.ProjectileCount += 1;
            }

            Destroy(collision.gameObject);
        }
    }
}
