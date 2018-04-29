using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : TeamObject {
    public int Gold;
    public bool Placed = false;

    protected BuildingManager bm;
    protected ResourcesManager rm;

	protected override void Start () {
        base.Start();
        bm = managers.GetComponent<BuildingManager>();
        rm = managers.GetComponent<ResourcesManager>();
        if (Placed)
            om.GetTeamObjectList().AddGameObjectToList(team, gameObject);
    }

    protected override void ObjectLiving()
    {
        if (Placed)
        {
            base.ObjectLiving();
            BuildingPlaced();
        }
    }

    protected virtual void BuildingPlaced() {

    }
}
