using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallWalker : MonoBehaviour
{
    Rigidbody body;
    int projectileSpeed = 3;
    public int damage;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    void Start()
    {
        body.velocity = transform.forward * projectileSpeed;
        Destroy(gameObject, 5f);
    }
    public void OnTriggerEnter(Collider other)
    {
        var player = other.gameObject.GetComponent<PlayerLifeManager>();
        if (player != null)
        {
            player.healt -= damage;
        }
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
