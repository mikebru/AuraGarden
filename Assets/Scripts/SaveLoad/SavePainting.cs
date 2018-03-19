using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;


[Serializable]
public class PaintingData
{
	public SerializableVector3 ParentLocation;
	public int SelectedBrush;
	public int SelectedMaterial;
	public bool isAnimating;
	public bool isBig;

	public SerializableColor MainColor;

	public SerializableVector3[] ChildPositions;
	public SerializableVector3[] ChildRotations;
}

//constructor for making a vector3 serializable
[System.Serializable]
public struct SerializableVector3
{
	public float x;

	public float y;

	public float z;

	public SerializableVector3(float rX, float rY, float rZ)
	{
		x = rX;
		y = rY;
		z = rZ;
	}
		
	public override string ToString()
	{
		return String.Format("[{0}, {1}, {2}]", x, y, z);
	}
		
	public static implicit operator Vector3(SerializableVector3 rValue)
	{
		return new Vector3(rValue.x, rValue.y, rValue.z);
	}
		
	public static implicit operator SerializableVector3(Vector3 rValue)
	{
		return new SerializableVector3(rValue.x, rValue.y, rValue.z);
	}
}

//constructor for making a color serializable
[System.Serializable]
public struct SerializableColor
{
	public float r;

	public float g;

	public float b;

	public float a;

	public SerializableColor(float rR, float rG, float rB, float rA)
	{
		r = rR;
		g = rG;
		b = rB;
		a = rA;
	}

	public override string ToString()
	{
		return String.Format("[{0}, {1}, {2}, {3}]", r, g, b, a);
	}

	public static implicit operator Color(SerializableColor rValue)
	{
		return new Color(rValue.r, rValue.g, rValue.b, rValue.a);
	}

	public static implicit operator SerializableColor(Color rValue)
	{
		return new SerializableColor(rValue.r, rValue.g, rValue.b, rValue.a);
	}
}


public class SavePainting : MonoBehaviour {

	public PaintingData PaintingInfo;
	public PaintingData LocalCopyOfData {get; set;}

	// Use this for initialization
	void Start () {

		PaintingInfo = new PaintingData ();

	}

	public void SetParentLocation(Vector3 position)
	{
		PaintingInfo.ParentLocation = position;
	}

	public void SetBrush(int BrushType)
	{
		PaintingInfo.SelectedBrush = BrushType;
	}

	public void SetMaterial(int MaterialType)
	{
		PaintingInfo.SelectedMaterial = MaterialType;
	}

	public void SetAnimation(bool isAnimating)
	{
		PaintingInfo.isAnimating = isAnimating;
	}

	public void SetColor(Color newColor)
	{
		PaintingInfo.MainColor = newColor;
	}

	public void SetChildPositions(Vector3[] childPositions)
	{
		PaintingInfo.ChildPositions = new SerializableVector3[childPositions.Length];

		for (int i = 0; i < childPositions.Length; i++) {
			PaintingInfo.ChildPositions [i] = childPositions [i];
		}

		//PaintingInfo.ChildPositions = childPositions;
	}

	public void SetChildScale(bool childScale)
	{
		PaintingInfo.isBig = childScale;
	}

	public void SetChildRotation(Vector3[] childRot)
	{
		PaintingInfo.ChildRotations = new SerializableVector3[childRot.Length];

		for (int i = 0; i < childRot.Length; i++) {
			PaintingInfo.ChildRotations[i] = childRot[i];
		}

	}

}
