﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eject : MonoBehaviour {

	Rigidbody body;
	bool fired = false;
	int maxTimer = 20;
	int timer;

	void Start(){
		timer = maxTimer;
	}

	// Update is called once per frame
	void Update () {
		if (fired) {
			timer--;
			if (timer == maxTimer) {
				gameObject.AddComponent<Rigidbody> ();
				body = GetComponent<Rigidbody> ();
				body.AddRelativeForce (new Vector3 (0, 0, 500));
			}
			if (timer < maxTimer) {
				if (timer > 0)
					body.AddRelativeForce (new Vector3 (0, 0, (Mathf.Abs (timer - maxTimer)+10)*20));
				//Wait until canopy is cleared
				if (timer == maxTimer - 10)
					GetComponent<MeshCollider> ().enabled = true;
			}
			if (timer == 0) {
				body.drag = 2;
				body.angularDrag = 5;
			}
		}
		
	}

	public void Fire(int fireDelay){
		fired = true;
		timer += fireDelay +1;
	}
}
