using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemiesHammer : MonoBehaviour
{
    public int damage;
    [SerializeField] Collider objCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyTag") 
        {
            var enemy = other.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.vida -= damage;
            }
        }
    }
}
