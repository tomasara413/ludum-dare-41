using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Building {

    float timer = 10;       //doba za kterou se hráči přičte jídlo.

	void Start () {
        Placed = false;     //budova není zezačátku postavená.
    }

	void Update () {
        GeneratingFood();
        print("Food " + Food);
        print("Time " + timer);
	}

    void GeneratingFood()
    {
        if (Placed == true)         //Pokud je budova postavena.
        {
            timer -= Time.deltaTime;    //Začne odečítat čas.
            if (timer <= 0)             //Pokud bude timer menší nebo roven 0.
            {
                Food += 10;             //Vygeneruje 10 jídla.
                timer += 10;            //Opět přičte 10 sec do timer a poté se vše opakuje dokola.
            }
        }
    }
}
