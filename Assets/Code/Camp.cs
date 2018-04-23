using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp : MonoBehaviour {

    public GameObject Unit;
    public float NextSpawn = 5f;
    int enemyCount = 1;
    public Vector3 spawnPosEnemy;

    // Use this for initialization
    void Start () {
        spawnPosEnemy = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.timeSinceLevelLoad >= NextSpawn)
        {
            //float enemyCount = (Mathf.Pow(Time.timeSinceLevelLoad, (1 + Time.timeSinceLevelLoad + Mathf.Pow(Time.timeSinceLevelLoad, 2)) / 2) + Mathf.Pow(Time.timeSinceLevelLoad, 12 / 20));
            Spawn(enemyCount);
            enemyCount++;
            NextSpawn += 5f;
            print("spawn!");
        }
	}

    void Spawn(int count)
    {
        for(int i = 0;i <= count; i++ )
        {
            Instantiate(Unit, spawnPosEnemy, Unit.transform.rotation);
            print("i");
        }
    }
}
