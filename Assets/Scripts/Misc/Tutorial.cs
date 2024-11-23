using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    //coletaveis
    //armadilhas e agua
    //inimigos e armas
    //pular
    //chefes
    public GameObject[] listaDeTutoriais;
    public GameObject listaDestrutiva;
    int currentTutorial;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tutorial")
        {
            Destroy(other);
            listaDeTutoriais[currentTutorial].SetActive(true);
            currentTutorial++;
        }
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            listaDeTutoriais[currentTutorial - 1].SetActive(false);
        }
    }
}
