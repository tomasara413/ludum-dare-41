using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour {

    public int MaxUse = 10;
    public int CurrentUse;
    public float Damage;

    void Start()
    {
        CurrentUse = MaxUse;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Ninja")
        {
            if (CurrentUse > 1)
            {
                collision.GetComponent<Ninja>().TakeDamage(Damage);
                CurrentUse -= 1;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
