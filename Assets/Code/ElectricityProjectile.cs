using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ElectricityProjectile : MonoBehaviour
{
    public float Range;
    public float Damage;

    private bool done = false;

    public void Update()
    {
        if (!done && Range > 0 && Damage > 0)
        {
            Collider[] cldr = Physics.OverlapSphere(transform.position, Range);
            Entity e;
            ElectricityProjectile inst;
            for (int i = 0; i < cldr.Length; i++)
            {
                if (e = cldr[i].GetComponent<Entity>())
                {
                    inst = Instantiate(this, e.transform.position, Quaternion.identity);
                    inst.Damage -= UnityEngine.Random.Range(0, inst.Damage);
                    inst.Range -= UnityEngine.Random.Range(0, inst.Range);
                    e.TakeDamage(Damage);
                }
            }

            done = true;
            //float enemyCount = Mathf.Pow(Time.timeSinceLevelLoad, (1 + Time.timeSinceLevelLoad + Mathf.Pow(Time.timeSinceLevelLoad, 2)) / 2) + Mathf.Pow(Time.timeSinceLevelLoad, 12/20);
        }

        if (done)
            Destroy(gameObject);
    }
}
