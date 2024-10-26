using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEnemiesHammer : MonoBehaviour
{
    public int damage;
    [SerializeField] CapsuleCollider capsuleCollider;

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.vida -= damage;
        }
    }

    //public void EnableTriggerBox() 
    //{
    //    capsuleCollider.enabled = true;
    //}

    //public void DisableTriggerBox()
    //{
    //    capsuleCollider.enabled = false;
    //}
}
