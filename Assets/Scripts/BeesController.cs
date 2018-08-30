using System;
using System.Collections.Generic;
using UnityEngine;

public class BeesController : MonoBehaviour {
	
	// Private
	private GameObject[] bees;
	private GameObject hunter;
	private GameObject prey;
	private float time;

	// Public
	public Transform master;
	public float radio;

	// Use this for initialization
	void Start () {
		updateBees();
		time = 0;
	}

    // Update is called once per frame
    void Update () {

		if ( existsNeutralBee() ) {
			selectHunter();
		}

		if ( hunter != null && prey != null) {
			hunt();
		} 

		beesDefenders();

	}

    private void updateBees() {
        bees = GameObject.FindGameObjectsWithTag("Bee");
    }

	private bool existsNeutralBee() {
        return GameObject.FindGameObjectWithTag("Neutral") != null;
    }

    private void selectHunter() {

		/* Determinamos la presa y el cazador */
        prey = GameObject.FindGameObjectWithTag("Neutral");
		prey.tag = "Prey";

		hunter = bees[0];
		hunter.tag = "Hunter";

		updateBees();

    }

 	private void hunt() {
        /* Iniciamos la caza */
		if(Vector3.Distance(hunter.transform.position, prey.transform.position) > 2f){

			Vector3 dir = prey.transform.position - hunter.transform.position;
			hunter.transform.Translate(dir.normalized * 5f * Time.deltaTime);

		} else {

			if (time > 3) {
				
				// All are Bee
				hunter.tag = "Bee";
				prey.tag = "Bee";

				// Set to null
				hunter = null;
				prey = null;

				time = 0;

				updateBees();

			} else {
				time += Time.deltaTime;
			}

		}
    }

    private void beesDefenders(){
		foreach (var bee in bees) {

			// Se tiene que sustituir este if por uno que compruebe que ya no puede moverse mas si se encuentra en su posicion
			if(Vector3.Distance(bee.transform.position, master.position) > radio){

				Vector3 dir = new Vector3(master.position.x + radio , master.position.y, master.position.z);

				dir = dir - bee.transform.position;
				bee.transform.Translate(dir.normalized * 2f * Time.deltaTime);

			} else {
				// The Bees rotate around Master
				bee.transform.RotateAround(master.position, Vector3.down, 50f * Time.deltaTime);
			}
		}
	}

}

