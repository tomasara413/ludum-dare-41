using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Building {

    GameObject ResManager;

    int FoodAmount;
    int FoodMax;

    float timer = 6;       //doba za kterou se hráči přičte jídlo.

	void Start () {
        Placed = false;     //budova není zezačátku postavená.
        ResManager = GameObject.FindGameObjectWithTag("Managers");
    }

	void Update () {
        GeneratingFood();
	}

    void GeneratingFood()
    {
        if (Placed == true)         //Pokud je budova postavena.
        {
            timer -= Time.deltaTime;    //Začne odečítat čas.
            if (timer <= 0 && ResManager.GetComponent<ResourcesManager>().FoodAmount < ResManager.GetComponent<ResourcesManager>().FoodMax - 120)             //Pokud bude timer menší nebo roven 0 a počet jídla bude menší nebo roven max jídla - 120, tak proveď
            {
                ResManager.GetComponent<ResourcesManager>().FoodAmount += 120;          //Vygeneruje 10 jídla.         
                timer += 6;                //Opět přičte 10 sec do timer a poté se vše opakuje dokola.                  
            }
            if (timer <= 0 && ResManager.GetComponent<ResourcesManager>().FoodAmount > ResManager.GetComponent<ResourcesManager>().FoodMax - 120)             //Pokud bude timer menší nebo roven 0 a počet jídla bude větší nebo roven max jídla - 120, tak proveď
            {
                ResManager.GetComponent<ResourcesManager>().FoodAmount += ResManager.GetComponent<ResourcesManager>().FoodMax - ResManager.GetComponent<ResourcesManager>().FoodAmount; ;                   //Přičte se zbytek jídla co chybí do FoodMax
            }
            if (ResManager.GetComponent<ResourcesManager>().FoodAmount == ResManager.GetComponent<ResourcesManager>().FoodMax)             //Pokud bude timer menší nebo roven 0 a počet jídla bude menší nebo roven max jídla, tak proveď
            {
                timer = 6;        
            }

        }
    }                  
}
