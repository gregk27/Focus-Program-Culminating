﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PayloadType{
	Missile,Bomb,Fuel
}


public class PayloadScript : MonoBehaviour {

	public GameObject engine;
	Rigidbody body;
	public float dropForce;
	public bool fired = false;
	public int accel;
	public PayloadType type;
	CapsuleCollider collider;
	[HideInInspector]
	public int lifetime = 0;
	public int maxSpeed;
	PlaneWrapper plane;
	public Explosion explosion;
	bool destroyed = false;
	/// How long the missile is propelled for
	public int maxFuel;


	// Use this for initialization
	void Start () {
		collider = GetComponent<CapsuleCollider> ();
		plane = GetComponentInParent<PlaneWrapper> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetKey(KeyCode.Alpha1))
			Fire();
		if (destroyed) {
			lifetime++;
			if (lifetime == 50)
				gameObject.SetActive (false);
			transform.Find ("Model").gameObject.SetActive (false);
			body.velocity = Vector3.zero;
			body.isKinematic = true;
			collider.enabled = false;
		}
		else if (fired) {
			lifetime++;
			if (lifetime < maxFuel) {
				body.AddForce (transform.right * accel);
			}
			if (lifetime > maxFuel) {
				if(engine != null) engine.GetComponent<ParticleSystem> ().Stop ();
				//Turn towards prograde vector
				transform.Rotate(new Vector3(0,0,(Mathf.DeltaAngle (Mathf.Rad2Deg * Mathf.Atan2 (transform.right.y * 10, transform.right.x * 10), Mathf.Rad2Deg * Mathf.Atan2 (body.velocity.y, body.velocity.x))) / 30), Space.World);
			}
			if (type == PayloadType.Missile) {
				body.AddForce (new Vector3 (-Physics.gravity.x, -Physics.gravity.y, -Physics.gravity.z));
			}
			if (lifetime == 15)	collider.enabled = true;
//			print (transform.InverseTransformDirection (body.velocity).x);
			if (transform.InverseTransformDirection (body.velocity).x > maxSpeed) {
				body.AddForce (-transform.right * accel * 2);
			}

			Debug.DrawLine (transform.position, transform.position + transform.right * 10);
			Debug.DrawLine (transform.position, transform.position + body.velocity * 10);
		}
	}

	void Fire(){
		gameObject.AddComponent<Rigidbody> ();
		body = gameObject.GetComponent<Rigidbody> ();
		body.AddForce (-transform.forward * dropForce);
		if(engine != null) engine.GetComponent<ParticleSystem> ().Play ();
		print (plane.speed);
		body.AddForce (transform.right * (maxSpeed/2+plane.speed*2));
		fired = true;
		body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
	}

	void OnCollisionEnter(Collision other){
		explosion.Detonate ();
		destroyed = true;
		print ("HIT");

		lifetime = 0;
	}
}
