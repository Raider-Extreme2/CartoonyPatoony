using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalFake : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Application.Quit();
    }
}
