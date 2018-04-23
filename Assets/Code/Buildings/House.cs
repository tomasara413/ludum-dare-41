using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class House : Building
    {
        //konzumace na 1 člověka
        public int consumption = 2;
        public int MaxGoldProduction = 20;

        //public int currentHousePopulation = 2;

        private int currentHousePopulation;
        //Počet populace v jednom domě
        public int CurrentHousePopulation
        {
            get { return currentHousePopulation; }
            set
            {
                if (value > MaxHousePopulation)
                    currentHousePopulation = MaxHousePopulation;
                else
                    currentHousePopulation = value;
            }
        }

        public int MaxHousePopulation = 5;

        public float ProductionTimer = 6;
        private float currentProductionTimer;
        //čas po kterym se zrodí nový Peasant.
        public float BornTimer = 5;
        private float currentBornTimer;

        protected override void Start()
        {
            base.Start();
            currentProductionTimer = ProductionTimer;
            currentBornTimer = BornTimer;
            currentHousePopulation = 2;
        }

        protected override void BuildingPlaced()
        {
            /*Debug.Log("population: " + currentHousePopulation);
            Debug.Log("food: " + rm.FoodAmount);
            Debug.Log("gold: " + rm.GoldAmount);*/
            HouseProduction();
        }

        void HouseProduction()
        {
            currentProductionTimer -= Time.deltaTime;
            currentBornTimer -= Time.deltaTime;
            if (currentProductionTimer <= 0)
            {
                /*Debug.Log(currentHousePopulation + "/" + MaxHousePopulation);
                Debug.Log((int)(((float)currentHousePopulation / MaxHousePopulation) * MaxGoldProduction));*/
                rm.GoldAmmount += (int)(((float)currentHousePopulation / MaxHousePopulation) * MaxGoldProduction);
                rm.FoodAmmount -= consumption * currentHousePopulation;
                currentProductionTimer = ProductionTimer;
            }
            if (currentBornTimer <= 0)
            {
                CurrentHousePopulation += 1;
                rm.PopulationAmmount += 1;
                currentBornTimer = BornTimer;
            }
        }
    }
}