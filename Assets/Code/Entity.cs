using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;


public class Entity : TeamObject
{
    public NavMeshAgent agent;
    public float speed;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
    }

    protected GameObject attack = null, follow = null;
    protected Vector3 p1, p2, previousPos;
    protected float meleRange = 10;
    protected float meleDamage = 10;
    protected float rangedRange = 0;
    protected float rangedDamage = 0;
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

        Movement();
        DetectUnits();

        if (animator != null)
        {
            if (rangedDamage > meleDamage && agent.remainingDistance > meleRange)
            {
                if (agent.remainingDistance <= rangedRange)
                {
                    usedDamage = rangedDamage;
                    if (attackObject != null && rangeCloseElements.Count > 0)
                        animator.SetBool(1, true);
                    else
                        animator.SetBool(1, false);
                }
                else
                    animator.SetBool(1, false);
            }
            else
            {
                if (agent.remainingDistance <= meleRange)
                {
                    usedDamage = meleDamage;
                    if (attackObject != null && meleCloseElements.Count > 0)
                        animator.SetBool(0, true);
                    else
                        animator.SetBool(0, false);
                }
                else
                    animator.SetBool(0, false);
            }
        }
    }

    protected override void ObjectDead()
    {
        agent.isStopped = true;
        return;
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

    public void Movement()
    {
        agent.speed = agent.acceleration = speed;

        if (attack)
        {
            if (p1 != (p2 = attack.transform.position))
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
                    if (agent.remainingDistance < rangedRange)
                    {
                        //Debug.Log(agent.remainingDistance + " " + GetBuffedRangedRange());
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(attack.transform.position - transform.position), Time.deltaTime * agent.angularSpeed);
                        agent.isStopped = true;
                    }
                    else
                        agent.isStopped = false;
                }
                else
                {
                    if (agent.remainingDistance < meleRange)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(attack.transform.position - transform.position), Time.deltaTime * agent.angularSpeed);
                        agent.isStopped = true;
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
            attack = null;
            attackObject = null;
        }
    }

    public void Attack(GameObject g)
    {
        Debug.Log("Attack Set");
        if (g != gameObject)
        {
            attackObject = g.GetComponent<TeamObject>();
            attack = g;
            follow = null;
        }
    }

    public void SetDestination(Vector3 dest)
    {
        Debug.Log("Destination Set: " + dest);
        points.Clear();
        points.Add(dest);
        attack = null;
        follow = null;
        attackObject = null;
    }

    public void AddPathPoint(Vector3 dest)
    {
        //Debug.Log("Path Added");
        points.Add(dest);
        attack = null;
        follow = null;
    }
}
