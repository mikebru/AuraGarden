using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveFunction : MonoBehaviour {

	private SavePainting[] CurrentPaintings;
	public PaintingData LocalCopyOfData {get; set;}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.S)) {
			CurrentPaintings = FindObjectsOfType<SavePainting> ();

			Debug.Log (CurrentPaintings.Length);

			if (!Directory.Exists ("Saves")) {
				Directory.CreateDirectory ("Saves");
			}

			for (int i = 0; i < CurrentPaintings.Length; i++) {
				SaveData (i);
			}

		}

	}


	public void SaveData(int index)
	{
		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream saveFile = File.Create("Saves/painting" + index.ToString() + ".binary");

		LocalCopyOfData = CurrentPaintings[index].PaintingInfo;

		formatter.Serialize (saveFile, LocalCopyOfData);

		saveFile.Close ();
	}




}
