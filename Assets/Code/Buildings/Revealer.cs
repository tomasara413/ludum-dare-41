using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revealer : Building {

    void OnTriggerEnter(Collider collision)
    {
        if(Placed)
        {
            if (collision.gameObject.tag == "Ninja") {
                collision.gameObject.GetComponent<Ninja>().Stealthed = false;
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (Placed)
        {
            if (collision.gameObject.tag == "Ninja")
            {
                collision.gameObject.GetComponent<Ninja>().Stealthed = true;
            }
        }
    }
}
