using Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScript : MonoBehaviour {

    public GameObject text;
    public ResourcesManager rm;
    public float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            if(rm.PopulationAmmount <= 0)
            {
                text.SetActive(true);
            }
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Ninja")
        {
            Debug.Log("YOU LOOOOSE");
            text.SetActive(true);
        }
    }
}
