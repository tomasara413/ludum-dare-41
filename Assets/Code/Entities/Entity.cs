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
        public float speed, MeleeRange, MeleeDamage, RangedRange, RangedDamage, AttackDelay, seenRange = 30;
        public float dist;

        protected override void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            meleRange = MeleeRange;
            meleDamage = MeleeDamage;
            rangedDamage = RangedDamage;
            rangedRange = RangedRange;
            attackDelay = AttackDelay;
        }

        protected GameObject attackGameObject = null, follow = null;
        protected Vector3 p1, p2, previousPos;
        protected float meleRange = 10, meleDamage = 10, rangedRange = 0, rangedDamage = 0, attackDelay = 0;
        protected TeamObject attackObject = null;

        protected List<Vector3> points = new List<Vector3>();
        protected Animator animator;

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


            //EneInRangeCheck();
            if (meleCloseElements.Count != 0)
                Attack(meleCloseElements[0].gameObject);

            else if (rangeCloseElements.Count != 0)
                Attack(rangeCloseElements[0].gameObject);

            else
            {
                if (gameObject.GetComponent<TeamObject>().team > 0)
                    Follow(GameObject.FindGameObjectWithTag("Castle"));
                else
                    Attack(GameObject.FindGameObjectWithTag("Camp"));
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

        protected override void ObjectDead()
        {
            Destroy(gameObject);
        }

        public void CauseDamage()
        {
            if(attackObject)
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

                if (workCollider != GetComponent<Collider>() && (workObject = workCollider.GetComponent<TeamObject>()) && workObject.team != team && ((workObject is Ninja && !(workObject as Ninja).Stealthed) || !(workObject is Ninja)))
                {
                    meleCloseElements.Add(workObject);
                }
            }
            rangeCloseElements.Clear();

            c = Physics.OverlapSphere(transform.position, rangedRange);

            for (int i = 0; i < c.Length; i++)
            {
                workCollider = c[i];
                if (workCollider != GetComponent<Collider>() && (workObject = workCollider.GetComponent<TeamObject>()) && workObject.team != team && ((workObject is Ninja && !(workObject as Ninja).Stealthed) || !(workObject is Ninja)))
                {
                    rangeCloseElements.Add(workObject);
                }
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
                            /*attackDelay -= Time.deltaTime;
                            if (attackDelay <= 0)
                            {
                                CauseDamage();
                                attackDelay = AttackDelay;
                            }*/
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
                            /*attackDelay -= Time.deltaTime;
                            if (attackDelay <= 0)
                            {
                                CauseDamage();
                                attackDelay = AttackDelay;
                            }*/
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