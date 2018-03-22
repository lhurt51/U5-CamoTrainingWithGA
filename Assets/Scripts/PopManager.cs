using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopManager : MonoBehaviour {

	public GameObject personPrefab;
	// How many people we want to spawn at each interval
	public int popSize = 10;
	// A lits to store all the people prefabs
	List<GameObject> population = new List<GameObject>();
	// How long has passed in this interval
	public static float elapsed = 0.0f;
	// How long each interval will last
	public int trialTime = 10;
	// Which interval we are currently on
	int generation = 0;

	// Our GUI's style
	GUIStyle guiStyle = new GUIStyle();
	void OnGUI() {
		guiStyle.fontSize = 20;
		guiStyle.normal.textColor = Color.white;
		GUI.Label (new Rect (10, 10, 100, 20), "Generation: " + generation, guiStyle);
		GUI.Label (new Rect (10, 30, 100, 20), "Trial Time: " + (int)elapsed, guiStyle);
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < popSize; i++) {
			Vector3 pos = new Vector3(Random.Range(-9.0f, 9.0f), Random.Range(-4.5f, 4.5f), 0.0f);
			GameObject person = Instantiate(personPrefab, pos, Quaternion.identity);
			person.GetComponent<DNA>().r = Random.Range(0.0f, 1.0f);
			person.GetComponent<DNA>().g = Random.Range(0.0f, 1.0f);
			person.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);
			person.GetComponent<DNA>().height = Random.Range(4.0f, 15.0f);
			population.Add(person);
		}
	}
	
	// Update is called once per frame
	void Update () {
		elapsed += Time.deltaTime;
		if (elapsed > trialTime) {
			BreedNewPop();
			elapsed = 0.0f;
		}
	}

	GameObject Breed(GameObject parent1, GameObject parent2) {
		Vector3 pos = new Vector3(Random.Range(-9.0f, 9.0f), Random.Range(-4.5f, 4.5f), 0.0f);
		GameObject offSpring = Instantiate (personPrefab, pos, Quaternion.identity);
		DNA par1DNA = parent1.GetComponent<DNA> ();
		DNA par2DNA = parent2.GetComponent<DNA> ();

		// Random chance of mutation
		if (Random.Range (0.0f, 100.0f) < 95.0f) {
			// Generate random colors based on parents DNA
			offSpring.GetComponent<DNA> ().r = Random.Range (0.0f, 100.0f) < 50.0f ? par1DNA.r : par2DNA.r;
			offSpring.GetComponent<DNA> ().g = Random.Range (0.0f, 100.0f) < 50.0f ? par1DNA.g : par2DNA.g;
			offSpring.GetComponent<DNA> ().b = Random.Range (0.0f, 100.0f) < 50.0f ? par1DNA.b : par2DNA.b;
			offSpring.GetComponent<DNA> ().height = Random.Range (0.0f, 100.0f) < 50.0f ? par1DNA.height : par2DNA.height;
		} else {
			offSpring.GetComponent<DNA>().r = Random.Range(0.0f, 1.0f);
			offSpring.GetComponent<DNA>().g = Random.Range(0.0f, 1.0f);
			offSpring.GetComponent<DNA>().b = Random.Range(0.0f, 1.0f);
			offSpring.GetComponent<DNA>().height = Random.Range(4.0f, 15.0f);
		}
		return offSpring;
	}

	void BreedNewPop() {
		List<GameObject> newPop = new List<GameObject> ();
		// Culling unfit people
		List<GameObject> sortedList = population.OrderByDescending(o => o.GetComponent<DNA>().timeToClicked).ToList();

		population.Clear ();
		// Breed the better half of the population
		for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++) {
			population.Add (Breed (sortedList [i], sortedList [i + 1]));
			population.Add (Breed (sortedList [i + 1], sortedList [i]));
		}

		// Destroy all previous population
		for (int i = 0; i < sortedList.Count; i++) {
			Destroy (sortedList[i]);
		}
		generation++;
	}
}
