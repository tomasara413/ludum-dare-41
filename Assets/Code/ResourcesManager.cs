using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour {


    private int goldAmount;
    public int GoldAmount   //Počet goldů, který hráč vlastní.
    {
        get { return goldAmount; }
        set {
            if (goldAmount + value > GoldMax)
                goldAmount = GoldMax;
            else
                goldAmount += value;
        }
    }

    private int foodAmount;
    public int FoodAmount   //Počet jídla, který hráč vlastní.
    {
        get { return foodAmount; }
        set
        {
            if (foodAmount + value > FoodMax)
                foodAmount = FoodMax;
            else
                foodAmount += value;
        }
    }
    private int populationAmount;
    public int PopulationAmount     //Počet populace, kterou hráč vlastní.
    {
        get { return populationAmount; }
        set
        {
            if (populationAmount + value > PopulationMax)
                populationAmount = PopulationMax;
            else
                populationAmount += value;
        }
    }

    public int GoldMax = 500;     //Maximum goldů
    public int FoodMax = 200;     //Maximum jídla
    public int PopulationMax = 10;  //Maximum Populace

	void Start () {		
	}
	
	void Update () {
	}
}
