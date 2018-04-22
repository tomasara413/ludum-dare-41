using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building {

    ResourcesManager ResManager;

    public int consumption = 2;     //konzumace na 1 člověka
    public int MaxGoldProduction = 20;
    public bool IncreaseMaxPopulation = false;

    //public int currentHousePopulation = 2;

    private int currentHousePopulation;
    public int CurrentHousePopulation     //Počet populace v jednom domě
    {
        get { return currentHousePopulation; }
        set
        {
            if (currentHousePopulation + value > maxHousePopulation)
                currentHousePopulation = maxHousePopulation;
            else
                currentHousePopulation += value;
        }
    }

    public int maxHousePopulation = 5;

    public float ProductionTimer = 6;       
    private float CurrentProductionTimer;
    public float BornTimer = 5;     //čas po kterym se zrodí nový Peasant.
    private float CurrentBornTimer;

    protected override void Start () {
        CurrentProductionTimer = ProductionTimer;
        CurrentBornTimer = BornTimer;
        ResManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<ResourcesManager>();
        currentHousePopulation = 2;
    }

    protected override void BuildingPlaced()
    {
        HouseProduction();
        Debug.Log("population " + currentHousePopulation);
        Debug.Log("gold " + ResManager.GoldAmount);
        
    }

    void HouseProduction()
    {
        if (!IncreaseMaxPopulation)
        {
            ResManager.PopulationMax += maxHousePopulation;
            IncreaseMaxPopulation = true;
        }
        CurrentProductionTimer -= Time.deltaTime;
        CurrentBornTimer -= Time.deltaTime;
        if (CurrentProductionTimer <= 0)
        {
            ResManager.GoldAmount += (int)(((float)currentHousePopulation / maxHousePopulation) * MaxGoldProduction);
            ResManager.FoodAmount -= consumption * currentHousePopulation;
            CurrentProductionTimer += ProductionTimer;
            
        }
        if (CurrentBornTimer <= 0)
        {
            CurrentHousePopulation += 1;
            CurrentBornTimer += BornTimer;
        }
    }
}
