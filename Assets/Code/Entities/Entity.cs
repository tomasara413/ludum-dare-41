using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Entities
{
    public class Entity : TeamObject
    {
        public NavMeshAgent agent;
        public float speed, MeleeRPublic, MeleeDamagePublic, RangedRangePublic, RangedDamagePublic, AttackDelayPublic, seenRange = 30;
        public float dist;

        protected override void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            meleRange = MeleeRPublic;
            meleDamage = MeleeDamagePublic;
            rangedDamage = RangedDamagePublic;
            rangedRange = RangedRangePublic;
            AttackDelay = AttackDelayPublic;
        }

        protected GameObject attackGameObject = null, follow = null;
        protected Vector3 p1, p2, previousPos;
        protected float meleRange = 10, meleDamage = 10, rangedRange = 0, rangedDamage = 0, AttackDelay = 0;
        protected TeamObject attackObject = null;

        protected List<Vector3> points = new List<Vector3>();
        protected Animator animator;

        protected List<GameObject> enemiesInRange = new List<GameObject>();
        protected List<TeamObject> meleCloseElements = new List<TeamObject>();
        protected List<TeamObject> rangeCloseElements = new List<TeamObject>();

        private float usedDamage = 0;
        protected override void ObjectLiving()
        {
            base.ObjectLiving();
            if (agent == null)
                return;
            dist = agent.remainingDistance;
            Movement();
            DetectUnits();


            EneInRangeCheck();
            if (enemiesInRange.Count != 0)
            {
                Attack(enemiesInRange[0]);
            }
            else
            {
                if (gameObject.tag == "Ninja")
                {
                    Follow(GameObject.FindGameObjectWithTag("Castle"));
                }
                else
                {
                    Attack(GameObject.FindGameObjectWithTag("Camp"));
                }
            }

            if (animator != null)
            {
                if (rangedDamage > meleDamage && agent.remainingDistance > meleRange)
                {
                    if (agent.remainingDistance <= rangedRange)
                    {

                        if (attackObject != null && rangeCloseElements.Count > 0)
                            animator.SetBool("Attack", true);
                        else
                            animator.SetBool("Attack", false);
                    }
                    else
                        animator.SetBool("Attack", false);
                }
                else
                {
                    if (agent.remainingDistance <= meleRange && attackGameObject != null)
                    {

                        animator.SetBool("Attack", true);
                    }
                    else
                        animator.SetBool("Attack", false);
                }
            }
        }

        public void EneInRangeCheck()
        {
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                if (enemiesInRange[i] == null)
                    enemiesInRange.Remove(enemiesInRange[i]);
            }
        }

        protected override void ObjectDead()
        {
            Destroy(gameObject);
        }

        public void CauseDamage()
        {
            attackObject.TakeDamage(usedDamage);
        }

        public void DetectUnits()
        {
            meleCloseElements.Clear();

            Collider[] c = Physics.OverlapSphere(transform.position, meleRange);
            Collider workCollider;
            TeamObject workObject;

            for (int i = 0; i < c.Length; i++)
            {
                workCollider = c[i];

                if (workCollider != GetComponent<Collider>() && (workObject = workCollider.GetComponent<TeamObject>()))
                {
                    meleCloseElements.Add(workObject);
                }
            }
            rangeCloseElements.Clear();

            c = Physics.OverlapSphere(transform.position, rangedRange);

            for (int i = 0; i < c.Length; i++)
            {
                workCollider = c[i];
                if (workCollider != GetComponent<Collider>() && (workObject = workCollider.GetComponent<TeamObject>()))
                {
                    rangeCloseElements.Add(workObject);
                }
            }


        }

        public void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.GetComponent<TeamObject>() == null)
                return;

            if (col.gameObject.GetComponent<TeamObject>().team != team)
            {
                if (col.gameObject.tag == "Ninja" || col.gameObject.tag == "Knight")
                {
                    enemiesInRange.Add(col.gameObject);
                }
            }
        }

        public void OnTriggerExit(Collider col)
        {
            if (enemiesInRange.Contains(col.gameObject))
            {
                enemiesInRange.Remove(col.gameObject);
            }
        }

        public void Movement()
        {
            agent.speed = agent.acceleration = speed;

            if (attackGameObject)
            {
                if (p1 != (p2 = attackGameObject.transform.position))
                {
                    agent.SetDestination(p2);
                    p1 = p2;
                }

                //Debug.Log(agent.remainingDistance + " " + GetBuffedRangedRange() + " " + agent.isStopped);
                //Debug.Log(agent.remainingDistance < GetBuffedRangedRange());
                if (p1 == p2)
                {
                    if (rangedDamage > meleDamage)
                    {
                        usedDamage = rangedDamage;
                        if (agent.remainingDistance < rangedRange)
                        {
                            //Debug.Log(agent.remainingDistance + " " + GetBuffedRangedRange());
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(attackGameObject.transform.position - transform.position), Time.deltaTime * agent.angularSpeed);
                            AttackDelay -= Time.deltaTime;
                            if (AttackDelay <= 0)
                            {
                                CauseDamage();
                                AttackDelay = AttackDelayPublic;
                            }
                            agent.isStopped = true;
                        }
                        else
                            agent.isStopped = false;
                    }
                    else
                    {
                        usedDamage = meleDamage;
                        if (agent.remainingDistance < meleRange)
                        {
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(attackGameObject.transform.position - transform.position), Time.deltaTime * agent.angularSpeed);
                            agent.isStopped = true;
                            AttackDelay -= Time.deltaTime;
                            if (AttackDelay <= 0)
                            {
                                CauseDamage();
                                AttackDelay = AttackDelayPublic;
                            }
                        }
                        else
                            agent.isStopped = false;
                    }
                }
            }
            else
            {
                if (follow)
                {
                    if (p1 != (p2 = follow.transform.position))
                    {
                        agent.SetDestination(p2);
                        p1 = p2;
                        agent.isStopped = false;
                    }
                }
                else
                {
                    if (points.Count > 0)
                    {
                        if (p1 != points[0] && agent.pathStatus != NavMeshPathStatus.PathInvalid)
                        {
                            agent.SetDestination(points[0]);
                            p1 = points[0];
                            agent.isStopped = false;
                        }
                    }
                }

                if (agent.remainingDistance < 1.5f)
                    if (points.Count > 0)
                        points.RemoveAt(0);
            }
        }

        public void Follow(GameObject g)
        {
            if (g != gameObject)
            {
                follow = g;
                attackGameObject = null;
                attackObject = null;
            }
        }

        public void Attack(GameObject g)
        {
            Debug.Log("Attack Set");
            if (g != gameObject)
            {
                if (g == null)
                    return;

                attackObject = g.GetComponent<TeamObject>();
                attackGameObject = g;
                follow = null;
            }
        }

        public void SetDestination(Vector3 dest)
        {
            Debug.Log("Destination Set: " + dest);
            points.Clear();
            points.Add(dest);
            attackGameObject = null;
            follow = null;
            attackObject = null;
        }

        public void AddPathPoint(Vector3 dest)
        {
            //Debug.Log("Path Added");
            points.Add(dest);
            attackGameObject = null;
            follow = null;
        }
    }
}