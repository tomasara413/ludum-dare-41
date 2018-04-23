using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public GameObject Target;

    public float Speed = 5f;
    public float RotateSpeed = 200f;
    public float Damage;

    Vector3 direction;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
            collision.GetComponent<TeamObject>().TakeDamage(Damage);  
    }

    private void Update()
    {
        if (Target)
        {
            direction = Target.transform.position - transform.position;
            transform.position += direction.normalized * Speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.forward, direction), Time.deltaTime);
        }
    }
}
