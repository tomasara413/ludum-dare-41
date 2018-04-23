using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revealer : Building {

    public bool revealed = false;

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Ninja")
            revealed = true;        
    }

    void Revealed()
    {
        GameObject theNinja = GameObject.Find("Ninja");
        Ninja ninja = theNinja.GetComponent<Ninja>();
        if (revealed == true)
        {
            ninja.Stealthed = false;            
        }
        else ninja.Stealthed = true;

    }
}
