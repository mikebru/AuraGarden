using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour {

	public GameObject MainMenu;

	public GameObject[] Menus;

	private SteamVR_TrackedObject trackedObj; 
	private SteamVR_Controller.Device Controller;

	private float CurrentRot;
	private Vector3 StartingRot;

	private bool CoolDown = false;

	private int CurrentMenu = 0;

	public bool SingleDisplay;

	private float TurnAmount;

	// Use this for initialization
	void Start () {

		trackedObj = this.GetComponent<SteamVR_TrackedObject> ();
		Controller = SteamVR_Controller.Input ((int)trackedObj.index);

		if (MainMenu != null) {
			SetMenus ();
		}

	}
	
	// Update is called once per frame
	void Update () {

		//call trigger press for the current tool
		if (Controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger)) {
			Menus [CurrentMenu].GetComponent<ToolClass> ().TriggerPress ();
		}

		//if the player is touching the thumb pad, pass the finger coordinate positions 
	//	if (Controller.GetTouch (Valve.VR.EVRButtonId.k_EButton_Axis0)) {
			//Vector2 CurrentPosition = new Vector2 ((Controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0).x + 1)/2, (Controller.GetAxis (Valve.VR.EVRButtonId.k_EButton_Axis0).y + 1)/2);
			//Menus [CurrentMenu].GetComponent<ToolClass> ().PadTouch (CurrentPosition);
		//}

		//rotate the menu, next tool 
		if (Controller.GetPressDown (Valve.VR.EVRButtonId.k_EButton_Grip) && CoolDown == false) {
			
			//tell the current tool we are swithing from it 
			Menus [CurrentMenu].GetComponent<ToolClass> ().ChangedTool ();
			RotateMenu ();
		}


	}

	public void SetMenus()
	{
		Menus = new GameObject[MainMenu.transform.childCount];

		for (int i = 0; i < Menus.Length; i++) {
			Menus [i] = MainMenu.transform.GetChild (i).gameObject;
		}

		CurrentRot = MainMenu.transform.localRotation.eulerAngles.z;
		StartingRot = MainMenu.transform.localRotation.eulerAngles;
		//igure out how much we need to turn each time 
		TurnAmount = 360/Menus.Length;

	}


	void RotateMenu()
	{
		//make the current menu invisible
		if (SingleDisplay == true) {
			GraphicsDisplay (false);
		}
			
		StartCoroutine (TurnMenu (TurnAmount));

			//keep track of what menu screen we are currently on. 
		if (CurrentMenu < Menus.Length-1) {
			CurrentMenu += 1;
		} else {
			CurrentMenu = 0;
		}
			
		//Debug.Log (CurrentMenu);

		//make the current menu visible
		if (SingleDisplay == true) {
			GraphicsDisplay (true);
		}

	}

	void GraphicsDisplay(bool TurnOn)
	{
		Menus [CurrentMenu].GetComponent<ToolClass> ().enabled = TurnOn;

		if (TurnOn == true) {
			Menus [CurrentMenu].GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 1, 1, 1f);
		} else {
			Menus [CurrentMenu].GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 1, 1, .2f);
		}

	}


	IEnumerator TurnMenu(float Rotation)
	{
		CoolDown = true;
		float t = 0;
		Vector3 StartRot =  new Vector3 (StartingRot.x,StartingRot.y, CurrentRot);;
		CurrentRot += Rotation;

		Vector3 EndRot = new Vector3 (StartingRot.x,StartingRot.y, CurrentRot);

		while (t < .3f) {
			t += Time.deltaTime;

			MainMenu.transform.localRotation = Quaternion.Euler (Vector3.Lerp (StartRot, EndRot, t/.3f));


			yield return new WaitForFixedUpdate ();
		}


		CoolDown = false;

	}
}
