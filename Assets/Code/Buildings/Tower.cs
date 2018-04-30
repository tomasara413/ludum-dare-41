using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class Tower : Building
    {
        public GameObject ProjectilePrefab;
        public Vector3 Spawn;


        private float shootingTimer;
        public float ShootingInterval = 2f;
        public float Damage;
        private List<GameObject> EnemyInRange = new List<GameObject>();

        protected override void Start()
        {
            base.Start();
            Spawn = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        }

        protected override void BuildingPlaced()
        {
            base.BuildingPlaced();
            Shooting();
        }

        protected GameObject currentTarget;
        Ninja n;
        void OnTriggerEnter(Collider collision)
        {
            if ((n = collision.gameObject.GetComponent<Ninja>()) && n.Stealthed)
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

            if (n && !n.Stealthed)
            {
                shootingTimer -= Time.deltaTime;
                if (shootingTimer <= 0)
                {
                    Shoot();
                }
            }
            else shootingTimer = 0;
        }

        protected virtual void Shoot()
        {
            Projectile proj = Instantiate(ProjectilePrefab, Spawn, ProjectilePrefab.transform.rotation).GetComponent<Projectile>();
            proj.Target = currentTarget;
            proj.gameObject.SetActive(true);
            proj.Damage = Damage;
            shootingTimer = ShootingInterval;
        }
    }
}