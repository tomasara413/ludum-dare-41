using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class ResourcesManager : MonoBehaviour
    {


        private int goldAmmount;
        public int GoldAmmount   //Počet goldů, který hráč vlastní.
        {
            get { return goldAmmount; }
            set
            {
                if (value > GoldMax)
                    goldAmmount = GoldMax;
                else
                    goldAmmount = value;
            }
        }

        private int foodAmmount;
        public int FoodAmmount   //Počet jídla, který hráč vlastní.
        {
            get { return foodAmmount; }
            set
            {
                if (value > FoodMax)
                    foodAmmount = FoodMax;
                else
                    foodAmmount = value;
            }
        }
        private int populationAmmount;
        //Počet populace, kterou hráč vlastní.
        public int PopulationAmmount
        {
            get { return populationAmmount; }
            set
            {
                if (value > PopulationMax)
                    populationAmmount = PopulationMax;
                else
                    populationAmmount = value;
            }
        }

        public int GoldMax = 500;     //Maximum goldů
        public int FoodMax = 200;     //Maximum jídla
        public int PopulationMax = 10;  //Maximum Populace
    }
}
