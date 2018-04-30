using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class Farm : Building
    {

        public int FoodProduction = 5;

        public float MaxFoodProductionTimer = 6;       //doba za kterou se hráči přičte jídlo.
        private float currentProductionTimer;

        protected override void Start()
        {
            base.Start();
            currentProductionTimer = MaxFoodProductionTimer;
        }

        protected override void BuildingPlaced()
        {
            base.BuildingPlaced();
            GenerateFood();
        }

        void GenerateFood()
        {
            currentProductionTimer -= Time.deltaTime;    //Začne odečítat čas.
            if (currentProductionTimer <= 0)             //Pokud bude timer menší nebo roven 0 a počet jídla bude menší nebo roven max jídla - 120, tak proveď
            {
                rm.FoodAmmount += FoodProduction;          //Vygeneruje 10 jídla.         
                currentProductionTimer = MaxFoodProductionTimer;                //Opět přičte 10 sec do timer a poté se vše opakuje dokola.                  
            }
        }
    }
}