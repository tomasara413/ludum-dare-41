using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class House : Building
    {
        //konzumace na 1 člověka
        public int consumption = 2, MaxGoldProduction = 20;

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

        public float ProductionTimer = 6, BornTimer = 5, DeathTimer = 10;
        private float currentProductionTimer, currentBornTimer, currentDeathTimer;

        public bool FirstPlacedRun = true;

        protected override void Start()
        {
            base.Start();
            currentProductionTimer = ProductionTimer;
            currentBornTimer = BornTimer;
            currentDeathTimer = DeathTimer;
            currentHousePopulation = 2;

        }

        protected override void BuildingPlaced()
        {
            base.BuildingPlaced();
            /*Debug.Log("population: " + currentHousePopulation);
            Debug.Log("food: " + rm.FoodAmount);
            Debug.Log("gold: " + rm.GoldAmount);*/

            HouseProduction();
        }

        void HouseProduction()
        {
            if (FirstPlacedRun)
            {
                rm.PopulationAmmount += 2;
                rm.PopulationMax += 5;
                FirstPlacedRun = false;
            }

            currentProductionTimer -= Time.deltaTime;

            if (currentHousePopulation < MaxHousePopulation && rm.FoodAmmount > 0 && CurrentHousePopulation > 1)
            {
                currentBornTimer -= Time.deltaTime;

                if (currentBornTimer <= 0)
                {
                    CurrentHousePopulation++;
                    rm.PopulationAmmount++;
                    currentBornTimer = BornTimer;
                }
            }

            if (currentProductionTimer <= 0)
            {
                /*Debug.Log(currentHousePopulation + "/" + MaxHousePopulation);
                Debug.Log((int)(((float)currentHousePopulation / MaxHousePopulation) * MaxGoldProduction));*/
                if (rm.GoldAmmount < rm.GoldMax)
                    rm.GoldAmmount += (int)(((float)currentHousePopulation / MaxHousePopulation) * MaxGoldProduction);

                rm.FoodAmmount -= consumption * currentHousePopulation;
                currentProductionTimer = ProductionTimer;
            }

            if (rm.FoodAmmount < 0 && CurrentHousePopulation > 0)
            {
                currentDeathTimer -= Time.deltaTime;

                if (currentDeathTimer < 0)
                {
                    currentHousePopulation--;
                    rm.PopulationAmmount--;
                    currentDeathTimer = DeathTimer;
                }
            }

        }
    }
}