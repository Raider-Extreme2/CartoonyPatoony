using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterKiller : MonoBehaviour
{
    int damage = 99999;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("O objeto " + other + " entrou no trigger");

        var player = other.gameObject.GetComponent<PlayerControler>();
        if (player != null)
        {
            player.healt -= damage;
        }

        var enemy = other.gameObject.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.vida -= damage;
        }
    }
}
