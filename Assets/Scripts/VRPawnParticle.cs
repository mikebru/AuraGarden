using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Linq;

public class VRPawnParticle : NetworkBehaviour {

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


	[Command]
	public void CmdSetAuth(NetworkInstanceId objectId, NetworkIdentity player)
	{
		GameObject iObject = NetworkServer.FindLocalObject (objectId);
		NetworkIdentity N_Identity = iObject.GetComponent<NetworkIdentity> ();
		NetworkConnection otherOwner = N_Identity.clientAuthorityOwner;

		if (otherOwner == player.connectionToClient) {
			return;
		} else {

			if (otherOwner != null) {
				N_Identity.RemoveClientAuthority (otherOwner);
			}

			N_Identity.AssignClientAuthority (player.connectionToClient);
		}


	}
}
