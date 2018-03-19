using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl : MonoBehaviour {

	public float MoveSpeed;

	public float CurrentMoveSpeed {get; set;}

	// Use this for initialization
	void Start () {

		CurrentMoveSpeed = 0;

	}
	
	// Update is called once per frame
	void Update () {

		float xMove = Input.GetAxis ("Horizontal");
		float yMove = Input.GetAxis ("Vertical");

		GetComponent<Rigidbody> ().velocity = new Vector3 (xMove * CurrentMoveSpeed, yMove * CurrentMoveSpeed, 0);

		transform.rotation = Quaternion.LookRotation (this.GetComponent<Rigidbody>().velocity);

		if (GetComponent<Rigidbody> ().velocity.magnitude == 0 && (xMove != 0 || yMove != 0)) {
			StartCoroutine (SpeedUp ());
		}
		else if(xMove == 0 && yMove == 0)
		{
			CurrentMoveSpeed = 0;
		}

	}

	IEnumerator SpeedUp()
	{
		float t = 0;

		while (t < 5) {

			t += Time.deltaTime;

			CurrentMoveSpeed = Mathf.Lerp (0, MoveSpeed, t / 5);


			yield return new WaitForFixedUpdate ();
		}
	}
		

}
