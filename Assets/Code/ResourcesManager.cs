using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour {

    public int GoldAmount;  //Počet goldů, který hráč vlastní.
    public int FoodAmount;  //Počet jídla, který hráč vlastní.

    public int GoldMax;     //Maximum goldů
    public int FoodMax;     //Maximum jídla

	void Start () {
		
	}
	
	void Update () {
        //Je-li přebytek některé suroviny, budeme dělat, že není
        if (FoodAmount > FoodMax)
            FoodAmount = FoodMax;
        if (GoldAmount > GoldMax)
            GoldAmount = GoldMax;
	}
}
