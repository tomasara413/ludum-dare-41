using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour {

    public int MaxUse = 10;
    public int CurrentUse;
    public float Damage;

    void Start()
    {
        CurrentUse = Random.Range(1, MaxUse);
    }

    void Update()
    {
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (CurrentUse > 1)
            {
                collision.GetComponent<TeamObject>().TakeDamage(Damage);
                CurrentUse -= 1;
            }
            else
            {
                collision.GetComponent<TeamObject>().TakeDamage(Damage);
                Destroy(collision.gameObject);
            }
        }
    }
}
