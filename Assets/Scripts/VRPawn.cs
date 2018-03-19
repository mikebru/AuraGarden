using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

public class VRPawn : NetworkBehaviour {

    public Transform Head;
    public Transform LeftController;
    public Transform RightController;

	public GameObject[] objects;


    void Start () {
        if (isLocalPlayer) { 

			LeftController.gameObject.SetActive (true);
			RightController.gameObject.SetActive (true);

			objects.ToList ().ForEach (x => x.gameObject.SetActive (true));

            GetComponentInChildren<SteamVR_ControllerManager>().enabled = true;
            GetComponentsInChildren<SteamVR_TrackedObject>(true).ToList().ForEach(x => x.enabled = true);
            Head.GetComponentsInChildren<MeshRenderer>(true).ToList().ForEach(x => x.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly);

			LeftController.gameObject.SetActive (true);
			RightController.gameObject.SetActive (true);
        }
	}

    void OnDestroy()
    {
        GetComponentInChildren<SteamVR_ControllerManager>().enabled = false;
        GetComponentsInChildren<SteamVR_TrackedObject>(true).ToList().ForEach(x => x.enabled = false);
    }
}
