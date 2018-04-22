using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : Building
{
    public Rigidbody Unit;
    public Vector3 Spawn = new Vector3(1, 0, 0);

    protected override void BuildingPlaced()
    {
        Recruit();
    }

    void Recruit()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(Unit, Spawn, Unit.rotation);
            rm.GoldAmount -= 10;
        }
    }
}
