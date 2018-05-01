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

        public int AttackRange;
        private float shootingTimer;
        public float ShootingInterval = 2f;
        public float Damage;
        private List<TeamObject> EnemyInRange = new List<TeamObject>();
        protected GameObject currentTarget;
        Ninja n;


        protected override void Start()
        {
            base.Start();
            
        }

        protected override void BuildingPlaced()
        {
            base.BuildingPlaced();
            Shooting();
            DetectUnits();
            Spawn = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
        }


        public void DetectUnits()
        {
            EnemyInRange.Clear();

            Collider[] c = Physics.OverlapSphere(transform.position, AttackRange);
            Collider workCollider;
            TeamObject workObject;

            for (int i = 0; i < c.Length; i++)
            {
                workCollider = c[i];

                if (workCollider != GetComponent<Collider>() && (workObject = workCollider.GetComponent<TeamObject>()) && workObject.team != team && ((workObject is Ninja && !(workObject as Ninja).Stealthed) || !(workObject is Ninja)))
                {
                    EnemyInRange.Add(workObject);
                }
            }
        }

        void Shooting()
        {
            if (EnemyInRange.Count == 0)
                return;

            for (int i = 0; i < EnemyInRange.Count - 1; i++)
            {
                if (EnemyInRange[i] == null)
                    EnemyInRange.Remove(EnemyInRange[i]);

                if(EnemyInRange[i].gameObject.GetComponent<Ninja>())
                {
                    currentTarget = EnemyInRange[i].gameObject;
                    break;
                }
            }

            if(currentTarget != null)
            {

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