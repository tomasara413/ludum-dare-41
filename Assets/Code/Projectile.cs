using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public Rigidbody Rb;

    public Transform Target;

    public float Speed = 5f;
    public float RotateSpeed = 200f;

    // Use this for initialization
    void Start () {
        Target = GameObject.FindGameObjectWithTag("Enemy").transform;
        Rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector3 direction = Target.position - Rb.position;

        direction.Normalize();

        Vector3 rotateAmount = Vector3.Cross(direction, transform.up);

        Rb.angularVelocity = -rotateAmount * RotateSpeed;

        Rb.velocity = transform.up * Speed;
    }
}
