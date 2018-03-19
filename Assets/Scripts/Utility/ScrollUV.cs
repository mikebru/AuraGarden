using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollUV : MonoBehaviour {

	public Material WaterEffect;
	private float CurrentTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		CurrentTime += Time.deltaTime* .1f;

		WaterEffect.SetTextureOffset ("_BumpMap", new Vector2(0, CurrentTime));

	}
}
