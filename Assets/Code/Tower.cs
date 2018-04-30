using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building {

    public GameObject Bullet;
    public Vector3 Spawn;
    public Collider col;
    public Ninja n;



    private float shootingTimer;
    public float ShootingInterval = 2f;
    private List<GameObject> EnemyInRange = new List<GameObject>();

    protected override void Start()
    {
        base.Start();
        Spawn = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        col = GetComponent<SphereCollider>();
    }

    protected override void BuildingPlaced()
    {
        col.enabled = true;
        Shooting();
    }

    GameObject currentTarget;
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Ninja")
            EnemyInRange.Add(collision.gameObject);
    }

    void OnTriggerExit(Collider collision)
    {
        if (EnemyInRange.Contains(collision.gameObject))
            EnemyInRange.Remove(collision.gameObject);
    }

    void Shooting()
    {

        if (EnemyInRange.Count == 0)
            return;

        currentTarget = EnemyInRange[0];
        n = currentTarget.GetComponent<Ninja>();

        if (currentTarget)
        {
            if(!n.Stealthed)
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
        }
        else shootingTimer = 0;
    }
}
