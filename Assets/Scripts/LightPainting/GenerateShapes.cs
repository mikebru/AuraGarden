using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuOptions {Material, Animation, Scale, Color, Symmetry, Brushed};

public class GenerateShapes : MonoBehaviour {

	private Slider SlideDisplay;

	public float SpawnDistance;

	public GameObject ShapeToSpawn;
	public Material CurrentMaterial;

	public Transform SpawnPoint {get; set;}

	public bool isSpawning;
	public int NumberofShapes;

	public GameObject CurrentParent { get; set;}

	private Vector3 LastSpawnPoint;
	private Vector3 SymmetryLastSpawnPoint;

	//private MovementControl M_Control;

	public Color CurrentColor { get; set;}

	private SteamVR_TrackedObject trackedObj; 
	private SteamVR_Controller.Device Controller;

	private float LastTime;

	public float SizeScale;
	public float DistanceScale { get; set;}

	public Material[] StaffMaterials;
	public GameObject[] StaffBrushes;

	public int SelectedMaterial {get; set;}
	public int SelectedBrush {get; set;}
	public bool isBig {get; set;}
	public bool DynamicColor {get; set;}

	public bool SymmetryDraw;

	public string ColorName { get; set;}
	private SpawnController Spawner;

	private LoadPainting PaintLoader;	

	// Use this for initialization
	void Start () {

		PaintLoader = FindObjectOfType<LoadPainting> ();

		SlideDisplay = GetComponentInChildren<Slider> ();

		SizeScale = 1;
		DistanceScale = 1;

		CurrentColor = new Color (.2f, .5f, .75f, .2f);

		LastSpawnPoint = transform.position;

		trackedObj = this.GetComponent<SteamVR_TrackedObject> ();
		Controller = SteamVR_Controller.Input ((int)trackedObj.index);

		//M_Control = this.GetComponent<MovementControl> ();

		//start with the dot
		SelectedBrush = 3;

		StartCoroutine (DelaySetSpawn ());

		//SetSpawnPoint ();
	}

	void Update()
	{
		//checking distance to determine if we should spawn
		if (Vector3.Distance (LastSpawnPoint, transform.position) >= SpawnDistance) {
			SpawnObject ();
			LastSpawnPoint = transform.position;
		} 
		else if (isSpawning == true) {
			LastTime += Time.deltaTime;
		}

		if (SlideDisplay == null) {
			SlideDisplay = FindObjectOfType<Slider> ();
		}
			
		//change the color based on time to create
		if (DynamicColor == true) {
			SetColor (ColorName, RotationToColor (Controller.velocity));
		}
	}

	IEnumerator DelaySetSpawn()
	{

		yield return new WaitForSeconds (1);

		SetSpawnPoint ();

	}

	//set if scultpture is symmetric
	public void SetSymmetry(bool isSymmetry)
	{
		SymmetryDraw = isSymmetry;
	}

	public void SetSpawnPoint()
	{
		SpawnPoint = transform.GetChild (0).transform;

		//SpawnPoint = GameObject.FindGameObjectWithTag ("Staff").GetComponent<Transform>();

		Spawner = GetComponentInChildren<SpawnController> ();

		//Spawner = FindObjectOfType<SpawnController> ();
	}

	public void SetStaffMesh (Mesh newMesh)
	{
		//Set the display mesh to match the spawn object
		if (GetComponentInChildren<MeshFilter> ().mesh  != newMesh) {
			GetComponentInChildren<MeshFilter> ().mesh = newMesh;
		}

	}

	//set whether the color is based on rotation
	public void SetColorEffect(bool isDynamic)
	{
		DynamicColor = isDynamic;
	}

	//set if animation is happenign or not
	public void SetEffect(bool isAnimating)
	{
		if (GetComponentInChildren<SpawnController> () != null) {
			GetComponentInChildren<SpawnController> ().SetEffect (isAnimating);
		} else {
			FindObjectOfType<SpawnController> ().SetEffect (isAnimating);
		}
	}

	//set which shape we are going to spawn
	public void SetSpawnShape(int Selection)
	{
		//update the shape to spawn
		ShapeToSpawn = StaffBrushes[Selection];
		SelectedBrush = Selection;


		//Set the display mesh to match the spawn object
		if (GetComponentInChildren<MeshFilter> ().mesh  != PaintLoader.BrushMesh[Selection]) {
			GetComponentInChildren<MeshFilter> ().mesh = PaintLoader.BrushMesh[Selection];
		}

	}

	public void SetMaterial(int Selection)
	{
		CurrentMaterial = new Material(StaffMaterials[Selection]);

		GetComponentInChildren<MeshRenderer> ().material = CurrentMaterial;

		SelectedMaterial = Selection;

		//set material name 
		SetMaterialName(PaintLoader.ColorNames [Selection]);
	}

	public void SetMaterialName(string newName)
	{
		ColorName = newName;

		Spawner.SetMaterialName (newName);
	}

	//set scale of mesh
	public void SetSize(float newSize)
	{
		SizeScale = newSize;

		//scale the brush by two, to indicate that the size is larger
		//doesn't necessarily line up with how large the 
		if (newSize > 1) {
			SpawnPoint.localScale = Vector3.one * 2;
			isBig = true;
		} else {
			SpawnPoint.localScale = Vector3.one;
			isBig = false;
		}
	}


	public void SetColor(string ColorName, Color newColor)
	{
		CurrentMaterial = new Material(CurrentMaterial);

		CurrentMaterial.SetColor (ColorName, newColor);

		GetComponentInChildren<MeshRenderer> ().material = CurrentMaterial;

		CurrentColor = newColor;
	}

	public void SetVertex(Vector2 newDirections)
	{
		CurrentMaterial = new Material(CurrentMaterial);

		CurrentMaterial.SetFloat ("_Value1", newDirections.x * .3f);
		CurrentMaterial.SetFloat ("_Value4", newDirections.y * .3f);

		GetComponentInChildren<MeshRenderer> ().material = CurrentMaterial;
	}
		
	//recount how many shapes are spawned, called after deleting occurs 
	public IEnumerator UpdateNumberofShapes()
	{
		yield return new WaitForFixedUpdate ();

		GameObject[] shapes = GameObject.FindGameObjectsWithTag ("SpawnShape");

		//update the slider display
		NumberofShapes = shapes.Length;

		SlideDisplay.value = 1 - (NumberofShapes/2000.0f);
	}

	void SpawnObject()
	{
		if (isSpawning == true) {

			SpawnSlice (false);

			if (SymmetryDraw == true) {
				SpawnSlice (true);

			}

			//SetColor (SpawnedShape.GetComponentInChildren<MeshRenderer>().material);

			//increase the count of created shapes
			NumberofShapes += 1;

			//update the slider display
			SlideDisplay.value = 1 - (NumberofShapes/2000.0f);

			//stop spawning when we exceed 1000 objects, for framerate reasons 
			if (NumberofShapes > 2000) {
				FindObjectOfType<SpawnController> ().StopSpawning();
			}

			LastTime = 0;

		}

	}


	void SpawnSlice(bool isSymmetry)
	{
		GameObject SpawnedShape = Instantiate (ShapeToSpawn, CurrentParent.transform);

		//set the amount of the spawned object is on screen
		SpawnedShape.GetComponent<ShapeAttributes> ().DisplayTime = LastTime;

		SpawnedShape.transform.rotation = SpawnPoint.rotation; 

		//set the scale of the object
		SpawnedShape.transform.localScale = SpawnedShape.transform.localScale * SizeScale;

		if (isSymmetry == false) {
			SpawnedShape.transform.position = SpawnPoint.position; 

			//Orient the shape to face in the direction of motion
			SpawnedShape.transform.GetChild (0).transform.forward = (transform.position - LastSpawnPoint);
			SpawnedShape.transform.GetChild (0).transform.localRotation = Quaternion.Euler (new Vector3 (SpawnedShape.transform.GetChild (0).transform.localEulerAngles.x, 0 , 0));
		} 
		//creates symmetry geometry 
		else {
			//positioning
			SpawnedShape.transform.position = SpawnPoint.position; 
			SpawnedShape.transform.localPosition = new Vector3 (SpawnedShape.transform.localPosition.x, SpawnedShape.transform.localPosition.y, -SpawnedShape.transform.localPosition.z);


			//float RotationAmount = 0;
			//RotationAmount = (90 - (SpawnedShape.transform.rotation.eulerAngles.z)) * 2;
			//SpawnedShape.transform.Rotate(new Vector3(RotationAmount,0,0), Space.World);

			//rotation
			SpawnedShape.transform.GetChild (0).transform.forward = (SpawnedShape.transform.position - SymmetryLastSpawnPoint);
			SpawnedShape.transform.GetChild (0).transform.localRotation = Quaternion.Euler (new Vector3 (SpawnedShape.transform.GetChild (0).transform.localEulerAngles.x, 0 , 0));

			//naming
			SpawnedShape.name += "Symmetry";

			SymmetryLastSpawnPoint = SpawnedShape.transform.position;
		}
			

		//Set the spawned shapes material
		SpawnedShape.GetComponentInChildren<MeshRenderer>().material = CurrentMaterial;

		//change the color based on time to create
		if (DynamicColor == true) {
			SpawnedShape.GetComponentInChildren<MeshRenderer> ().material.SetColor (ColorName, RotationToColor (Controller.velocity));
		}

	}

		
	Color RotationToColor(Vector3 CurrentVector)
	{
		float R = ((CurrentVector.x * Controller.velocity.x/5) + .1f);
		float G = ((CurrentVector.y * Controller.velocity.y/5) + .1f);
		float B = ((CurrentVector.z  * Controller.velocity.z/5) + .1f);

		Color DirectionColor = new Color (R, G, B, 1);

		return DirectionColor;
	}

	void SetColor(Material SpawnMat)
	{
		if (SpawnMat.HasProperty("_EmissionColor") == true) {

			Color StartColor = SpawnMat.GetColor ("_EmissionColor");
			SpawnMat.SetColor ("_EmissionColor", StartColor * ((LastTime * 10) + .2f));
		}

	}


	//spawning with a time delay
	IEnumerator SpawnTimer()
	{

		yield return new WaitForSeconds (.01f);

		SpawnObject ();

		StartCoroutine (SpawnTimer ());
		
	}



}
