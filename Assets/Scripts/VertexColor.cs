using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertexColor : MonoBehaviour {

	void Start()
	{

	}

	void FixedUpdate()
	{
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		Vector3[] normals = mesh.normals;

		// create new colors array where the colors will be created.
		Color[] colors = new Color[normals.Length];

		for (int i = 0; i < normals.Length; i++) {
			colors [i] = new Color (normals [i].x, normals [i].y, normals [i].z, 1);
		}
		// assign the array of colors to the Mesh.
		mesh.colors = colors;

	}
}
