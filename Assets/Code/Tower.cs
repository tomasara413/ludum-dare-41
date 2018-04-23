using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public GameObject Bullet;
    public Vector3 Spawn = new Vector3(10, 5, 0);

    private float shootingTimer;
    public float ShootingInterval = 2f;

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

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
            currentTarget = null;
    }

    void Shooting()
    {
        if (currentTarget)
        {
            shootingTimer -= Time.deltaTime;
            if (shootingTimer <= 0)
            {
                Projectile proj = Instantiate(Bullet, Spawn, Bullet.transform.rotation).GetComponent<Projectile>();
                proj.Target = currentTarget;
                proj.gameObject.SetActive(true);
                shootingTimer = ShootingInterval;
            }
        }
        else shootingTimer = 0;
    }
}
