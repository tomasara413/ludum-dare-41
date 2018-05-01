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
        public float speed, MeleeRange, MeleeDamage, RangedRange, RangedDamage;

        private float powMelee = 0, powRanged = 0, powVisionRange;
        protected override void Start()
        {
            base.Start();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            meleeRange = MeleeRange;
            meleeDamage = MeleeDamage;
            rangedDamage = RangedDamage;
            rangedRange = RangedRange;
            //attackDelay = AttackDelay;
            powMelee = Mathf.Pow(MeleeRange, 2);
            powRanged = Mathf.Pow(RangedRange, 2);
            powVisionRange = Mathf.Pow(powVisionRange, 2);
            castle = GameObject.FindGameObjectWithTag("Castle");
            camp = GameObject.FindGameObjectWithTag("Camp");
        }
        private float dist;

        protected GameObject attackGameObject = null, follow = null;
        protected Vector3 p1, p2, previousPos;
        protected float meleeRange = 10, meleeDamage = 10, rangedRange = 0, rangedDamage = 0, attackDelay = 0;
        protected TeamObject attackObject = null;

        protected List<Vector3> points = new List<Vector3>();
        protected Animator animator;

        protected List<TeamObject> meleCloseElements = new List<TeamObject>();
        protected List<TeamObject> rangeCloseElements = new List<TeamObject>();
        protected List<TeamObject> elementsInAgroRange = new List<TeamObject>();

        private float usedDamage = 0;
        private float sqrDst = 0;

        private GameObject camp, castle;

        protected override void ObjectLiving()
        {
            base.ObjectLiving();
            if (agent == null)
                return;
            dist = agent.remainingDistance;
            Movement();
            DetectUnits();

            if (attackObject != null)
                sqrDst = (transform.position - attackObject.transform.position).sqrMagnitude;

            AgressiveRangeCheck();
            /*if (meleCloseElements.Count != 0)
                Attack(meleCloseElements[0].gameObject);
            else if (rangeCloseElements.Count != 0)
                Attack(rangeCloseElements[0].gameObject);*/
            if (elementsInAgroRange.Count > 0 && (attackObject == null || (attackObject != null && agent.remainingDistance > VisionRange && sqrDst > powVisionRange)))
                Attack(elementsInAgroRange[0].gameObject);
            else
            {
                if (!attackObject)
                {
                    if (team > 0)
                    {
                        Follow(castle);
                    }
                    else
                    {
                        Attack(camp);
                        sqrDst = (transform.position - attackObject.transform.position).sqrMagnitude;
                    }
                }
            }

            if (animator != null && attackObject != null)
            {
                if (attackObject is Ninja && (attackObject as Ninja).Stealthed)
                {
                    attackObject = null;
                    attackGameObject = null;
                }

                if (rangedDamage > meleeDamage && sqrDst > powMelee && agent.remainingDistance > meleeRange)
                {
                    if (sqrDst <= powRanged && agent.remainingDistance <= rangedRange)
                    {
                        if (rangeCloseElements.Count > 0)
                        {
                            Debug.Log("First true");
                            animator.SetBool("Attack", true);
                        }
                        else
                            animator.SetBool("Attack", false);
                    }
                    else
                        animator.SetBool("Attack", false);
                }
                else
                {
                    if (sqrDst <= powMelee && agent.remainingDistance <= meleeRange)
                    {
                        Debug.Log("Second true");
                        animator.SetBool("Attack", true);
                    }
                    else
                        animator.SetBool("Attack", false);
                }
            }
        }

        protected virtual void AgressiveRangeCheck()
        {
            Collider[] c = Physics.OverlapSphere(transform.position, VisionRange);
            Collider workCollider;
            TeamObject workObject;
            elementsInAgroRange.Clear();

            for (int i = 0; i < c.Length; i++)
            {
                workCollider = c[i];
                
                if ((workObject = workCollider.GetComponent<TeamObject>()) && workObject.team != team && ((workObject is Ninja && !(workObject as Ninja).Stealthed) || !(workObject is Ninja)))
                {
                    //Debug.Log(name + ", Possible target: " + workCollider.name);
                    elementsInAgroRange.Add(workObject);
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

            Collider[] c = Physics.OverlapSphere(transform.position, meleeRange);
            Collider workCollider;
            TeamObject workObject;

            for (int i = 0; i < c.Length; i++)
            {
                workCollider = c[i];

                if ((workObject = workCollider.GetComponent<TeamObject>()) && workObject.team != team && ((workObject is Ninja && !(workObject as Ninja).Stealthed) || !(workObject is Ninja)))
                    meleCloseElements.Add(workObject);
            }
            rangeCloseElements.Clear();

            c = Physics.OverlapSphere(transform.position, rangedRange);

            for (int i = 0; i < c.Length; i++)
            {
                workCollider = c[i];
                if ((workObject = workCollider.GetComponent<TeamObject>()) && workObject.team != team && ((workObject is Ninja && !(workObject as Ninja).Stealthed) || !(workObject is Ninja)))
                    rangeCloseElements.Add(workObject);
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
                    if (rangedDamage > meleeDamage)
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
                        usedDamage = meleeDamage;
                        if (agent.remainingDistance < meleeRange)
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
            //Debug.Log("Attack Set");
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
            //Debug.Log("Destination Set: " + dest);
            //for now, it can be used to set destination but we do not want that for this build
            return; 
            points.Clear();
            points.Add(dest);
            attackGameObject = null;
            follow = null;
            attackObject = null;
        }

        public void AddPathPoint(Vector3 dest)
        {
            //Debug.Log("Path Added");
            //for now, it can be used to set path but we do not want that for this build
            return; 
            points.Add(dest);
            attackGameObject = null;
            follow = null;
        }
    }
}