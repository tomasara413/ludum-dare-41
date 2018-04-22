using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public int Health = 100;

    public int Gold;
    public bool Placed = false;
    public bool ProvidesVision = false;
    public float VisionRange = 10;
    public byte team = 0;

    protected BuildingManager bm;
    protected ResourcesManager rm;

	protected virtual void Start () {
        GameObject managers = GameObject.FindGameObjectWithTag("Managers");
        bm = managers.GetComponent<BuildingManager>();
        rm = managers.GetComponent<ResourcesManager>();
        if (Placed)
            bm.GetBuildingList().AddGameObjectToList(team, gameObject);
    }

    int visionID = -1;
    private void Update()
    {
        if (Placed && Health > 0)
        {
            BuildingPlaced();
            if (ProvidesVision)
            {
                if (visionID < 0)
                    visionID = bm.GetBuildingList().AddVisionObjectToList(0, gameObject);
            }
            else
            {
                if (visionID > -1)
                    if (bm.GetBuildingList().RemoveVisionObject(0, visionID))
                        visionID = -1;
            }
        }
    }

    protected virtual void BuildingPlaced() { }
}
