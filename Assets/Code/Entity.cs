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
    protected override void ObjectLiving()
    {
        base.ObjectLiving();
        Movement();
    }

    protected override void ObjectDead()
    {
        agent.isStopped = true;
        return;
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

    public void TakeDamage(float damage)
    {
        health -= damage;
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
