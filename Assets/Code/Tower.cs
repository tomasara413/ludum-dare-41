using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public GameObject Bullet;
    public Vector3 Spawn = new Vector3(10, 5, 0);

    void Update()
    {
        Shooting();
    }

    GameObject currentTarget;
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
            currentTarget = collision.gameObject;
    }

    void Shooting()
    {
        if (currentTarget && Input.GetButtonDown("Fire1"))
        {
            Projectile proj = Instantiate(Bullet, Spawn, Bullet.transform.rotation).GetComponent<Projectile>();
            proj.Target = currentTarget;
            proj.gameObject.SetActive(true);
        }
    }
}
