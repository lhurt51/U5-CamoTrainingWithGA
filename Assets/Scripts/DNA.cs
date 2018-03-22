using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour {

	// Color gene
	public float r;
	public float g;
	public float b;

	// height gene **normal height (9.48)**
	public float height;

	// Has been clicked on **AKA Dead**
	bool clicked = false;
	// How long was the person left unclicked **AKA Undead**
	public float timeToClicked = 0.0f;

	SpriteRenderer sRend;
	Collider2D sCollider;

	// Use this for initialization
	void Start () {
		sRend = GetComponent<SpriteRenderer>();
		sCollider = GetComponent<Collider2D>();
		sRend.color = new Color(r, g, b);
		sRend.size = new Vector2(sRend.size.x, height);
	}

	void OnMouseDown() {
		clicked = true;
		timeToClicked = PopManager.elapsed;
		Debug.Log("Died At: " + timeToClicked);
		sRend.enabled = false;
		sCollider.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
