using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallInstantiator : MonoBehaviour
{
    public GameObject fireBallPrefab;
    public Transform fireBallSpawnerTransform;

    public void SpawnFireBall() 
    {
        GameObject quebab = Instantiate(fireBallPrefab, fireBallSpawnerTransform.position, fireBallSpawnerTransform.rotation);
    }
}
