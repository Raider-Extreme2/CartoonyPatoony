using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateHammerHead : MonoBehaviour
{
    public GameObject damageObjectPrefab;
    public GameObject damageObjectPlace;
    private Transform hitArea;

    [Header("SoundFX")]
    [SerializeField] AudioSource playerSpeaker;
    [SerializeField] AudioClip hammerNoise;

    private void Start()
    {
        hitArea = damageObjectPlace.transform;
    }
    public void CreateObject()
    {
        playerSpeaker.PlayOneShot(hammerNoise);
        GameObject prefab = Instantiate(damageObjectPrefab, hitArea.position, hitArea.rotation);
        Destroy(prefab, 0.1f);
    }
}
