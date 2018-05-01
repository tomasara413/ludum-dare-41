using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buildings
{
    public class Camp : Building
    {
        public GameObject Unit, SpawnPoint;
        public float NextSpawn = 5f, delaySpawnEnemyinOneRun = 1f, WaveDelay = 5f;
        int enemyCount = 1;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            
        }

        // Update is called once per frame
        protected override void ObjectLiving()
        {
            base.ObjectLiving();

            //Debug.Log(Health);

            NextSpawn -= Time.deltaTime;
            if (NextSpawn < 0)
            {
                //float enemyCount = (Mathf.Pow(Time.timeSinceLevelLoad, (1 + Time.timeSinceLevelLoad + Mathf.Pow(Time.timeSinceLevelLoad, 2)) / 2) + Mathf.Pow(Time.timeSinceLevelLoad, 12 / 20));
                

                StartCoroutine(Spawn(enemyCount));
                enemyCount++;
                NextSpawn = WaveDelay;
                //print("spawn!");
            }
        }

        IEnumerator Spawn(int count)
        {
            for (int i = 0; i <= count; i++)
            {
                yield return new WaitForSeconds(delaySpawnEnemyinOneRun);
                Instantiate(Unit, SpawnPoint.transform.position, Unit.transform.rotation);
            }
        }

        protected override void ObjectDead()
        {
            WinImage.SetActive(true);
            Destroy(gameObject);
        }
    }
}
