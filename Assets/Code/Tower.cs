using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public GameObject Bullet;
    public Vector3 Spawn = new Vector3(10, 5, 0);

    public bool Spotted = false;

    void Update()
    {
        Shooting();
        print(Spotted);
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Spotted = true;
            print("Spotted");
        }
    }

    void Shooting()
    {        
        if (Spotted && Input.GetButtonDown("Fire1"))
        {
            Instantiate(Bullet, Spawn, Bullet.transform.rotation);
        }
    }
}
