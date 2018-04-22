using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : Building {

    ResourcesManager ResManager;

    public int foodIncrease = 100;
    public int goldIncrease = 200;
    public bool Increase = false;

    protected override void Start ()
    {
        ResManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ResourcesManager>();
    }

    protected override void BuildingPlaced()
    {
        WarehouseProduction();
    }

    void WarehouseProduction()
    {        
        if (!Increase)
        {
            ResManager.GoldMax += goldIncrease;
            ResManager.FoodMax += foodIncrease;
            Increase = true;
        }       
    }
}
