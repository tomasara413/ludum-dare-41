using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public GameObject Target;

    public float Speed = 5f, Distance, RotateSpeed = 200f, Damage;

    Vector3 direction;

    private void Update()
    {
        if (Target)
        {
            direction = Target.transform.position - transform.position;
            Distance = (Target.transform.position - transform.position).sqrMagnitude;
            transform.position += direction.normalized * Speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.forward, direction), Time.deltaTime);

            if(Distance < 1)
            {
                Target.GetComponent<Entity>().TakeDamage(Damage);
                Destroy(gameObject);
            }
        }
    }
}
