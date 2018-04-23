using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class Warehouse : Building
    {
        public int foodIncrease = 100;
        public int goldIncrease = 200;
        public bool Increase = false;

        protected override void BuildingPlaced()
        {
            WarehouseProduction();
        }

        void WarehouseProduction()
        {
            if (!Increase)
            {
                rm.GoldMax += goldIncrease;
                rm.FoodMax += foodIncrease;
                Increase = true;
            }
        }
    }
}