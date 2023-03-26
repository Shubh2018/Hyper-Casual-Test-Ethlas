using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int _playerHealth;

    public void UpdateHealth(int damage)
    {
        _playerHealth -= damage;
    }
}
