using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class Barracks : Building
    {
        public GameObject Spawn;

        public void Recruit(GameObject Unit)
        {
            Instantiate(Unit, Spawn.transform.position, Unit.transform.rotation);
            rm.SoldierAmount += 1;
            rm.GoldAmmount -= Unit.GetComponent<TeamObject>().Gold;
        }
    }
}