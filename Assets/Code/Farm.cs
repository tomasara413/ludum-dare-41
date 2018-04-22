using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Building {

    ResourcesManager ResManager;

    public int FoodProduction = 5;

    public float MaxFoodProductionTimer = 6;       //doba za kterou se hráči přičte jídlo.
    private float currentProductionTimer;

	protected override void Start () {
        currentProductionTimer = MaxFoodProductionTimer;
        ResManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ResourcesManager>();
    }

    protected override void BuildingPlaced () {
        GenerateFood();
        
	}

    void GenerateFood()
    {
        currentProductionTimer -= Time.deltaTime;    //Začne odečítat čas.
        if (currentProductionTimer <= 0)             //Pokud bude timer menší nebo roven 0 a počet jídla bude menší nebo roven max jídla - 120, tak proveď
        {
            ResManager.FoodAmount += FoodProduction;          //Vygeneruje 10 jídla.         
            currentProductionTimer += MaxFoodProductionTimer;                //Opět přičte 10 sec do timer a poté se vše opakuje dokola.                  
        }
    }                  
}