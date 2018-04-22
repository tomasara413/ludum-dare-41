using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public int Health = 100;

    public int Gold;
    public bool Placed = false;
    public float VisionRange = 10;
    public byte team = 0;

    private BuildingManager bm;

	protected virtual void Start () {
        bm = GameObject.FindGameObjectWithTag("Managers").GetComponent<BuildingManager>();
        if (Placed)
            bm.GetBuildingList().AddGameObjectToList(team, gameObject);
    }
	
	
	private void Update () {
        if (Placed && Health > 0)
            //jestli je budova postavená, tak se rpovede BuildingPlaced();
            BuildingPlaced();
	}

    protected virtual void BuildingPlaced() { }
}
