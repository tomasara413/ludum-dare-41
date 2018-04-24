using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScript : MonoBehaviour {

    public GameObject text;

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Ninja")
        {
            Debug.Log("YOU LOOOOSE");
            text.SetActive(true);
        }
    }
}
