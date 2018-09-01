using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeesController : MonoBehaviour {
	
	// Private
	private GameObject[] bees;

	private ArrayList prey;
	private ArrayList hunter;
	private float time;

	// Public
	public GameObject prefab;
	public Transform master;
	public float radio;

	// Use this for initialization
	void Start () {

		prey = new ArrayList();
		hunter = new ArrayList();
		time = 0;

		updateBees();
	}

    // Update is called once per frame
    void Update () {

		if ( existsNeutralBee() ) {
			selectHunter();
		}


		// Esto del try catch me parece una cutrada... Aunque funciona se deberia cambiar.
		// El problema es que las listas no funcionan. Se deberia hacer con el capacity pero por alguna razon, cuando no hay objetos adquiere como valor 4 cuando deberia
		// ser 0 :/

		try	{
			if ( hunter[0] != null ) {
				hunt();
			}
		}
		catch (System.Exception) {
		}

		

		beesDefenders();

	}

	// Funciones auxiliares
    private void updateBees() {
        bees = GameObject.FindGameObjectsWithTag("Bee");
    }

	private bool existsNeutralBee() {
        return GameObject.FindGameObjectWithTag("Neutral") != null;
    }

	private void updateLists(){
		hunter.TrimToSize();
		prey.TrimToSize();
	}

	// Funciones motoras
    private void selectHunter() {

		GameObject cazador, neutralBee;
		GameObject[] neutrals = GameObject.FindGameObjectsWithTag("Neutral");

		for (int i = 0; i < bees.Length; i++) {

			try {
			
				neutralBee = neutrals[i];

				// Presa
				neutralBee.tag = "Prey";
				prey.Add(neutralBee);

				// Cazador
				cazador = bees[0];
				cazador.tag = "Hunter";
				hunter.Add(cazador);
			}
			catch (System.Exception) {
				
			}
			
			updateBees();

		}

    }

 	private void hunt() {

		GameObject cazador, presa;

		for (int i = 0; i < this.hunter.Capacity; i++) {

			cazador = (GameObject) hunter[i];
			presa = (GameObject) prey[i];

			/* Iniciamos la caza */
			if(Vector3.Distance(cazador.transform.position, presa.transform.position) > 2f){

				Vector3 dir = presa.transform.position - cazador.transform.position;
				cazador.transform.Translate(dir.normalized * 5f * Time.deltaTime);

			} else {

				if (time > 3) {
					
					// All are Bee
					cazador.tag = "Bee";
					Instantiate(prefab, presa.transform.position, Quaternion.identity); 
					GameObject.Destroy(presa);

					// Set to null
					hunter.Remove(cazador);
					prey.Remove(presa);

					time = 0;

					updateBees();

				} else {
					time += Time.deltaTime;
				}

			}

			updateLists();
			
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
				if (Vector3.Distance(bee.transform.position, master.position) < (radio - 0.2f) ) {
					Vector3 dir = new Vector3(master.position.x + (radio - 0.2f) , master.position.y, master.position.z);

					dir = dir - bee.transform.position;
					bee.transform.Translate(dir.normalized * 2f * Time.deltaTime);
				} else {
					// The Bees rotate around Master
					bee.transform.RotateAround(master.position, Vector3.down, 50f * Time.deltaTime);
				}
			}
		}
	}

}

