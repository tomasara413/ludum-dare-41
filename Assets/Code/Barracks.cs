using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : Building
{
    public GameObject Spawn;

    public void Recruit(GameObject Unit)
    {
        Instantiate(Unit, Spawn.transform.position, Unit.transform.rotation);
        rm.GoldAmmount -= 10;
    }
}
