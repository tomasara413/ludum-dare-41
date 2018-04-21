using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    public int Health;

    public int Gold;
    public bool Placed = false; 
	protected virtual void Start () {
		
	}
	
	
	private void Update () {
        if (Placed)
            //jestli je budova postavená, tak se rpovede BuildingPlaced();
            BuildingPlaced();
	}

    protected virtual void BuildingPlaced() { }
}
