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

    public int GoldMax;     //Maximum goldů
    public int FoodMax;     //Maximum jídla

	void Start () {
		
	}
	
	void Update () {
	}
}
