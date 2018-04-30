using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class TeamObject : MonoBehaviour
{
    public int Gold;
    public byte team = 0;
    protected float health = 100;
    public bool ProvidesVision = false;
    public float VisionRange = 10;
    public float Health
    {
        get { return health; }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    protected GameObject managers;
    protected ObjectManager om;

    public GameObject WinImage;

    protected virtual void Start()
    {
        managers = GameObject.FindGameObjectWithTag("Managers");
        om = managers.GetComponent<ObjectManager>();
    }

    private void Update()
    {
        if (Health > 0)
            ObjectLiving();
        else
            ObjectDead();
    }

    int visionID = -1;
    protected virtual void ObjectLiving()
    {
        if (ProvidesVision)
        {
            if (visionID < 0)
                visionID = om.GetTeamObjectList().AddVisionObjectToList(0, gameObject);
        }
        else
        {
            if (visionID > -1)
                if (om.GetTeamObjectList().RemoveVisionObject(0, visionID))
                    visionID = -1;
        }
    }

    protected virtual void ObjectDead()
    {
        if (gameObject.tag == "Camp")
            WinImage.SetActive(true);

        Destroy(gameObject);
    }
}