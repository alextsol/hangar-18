//Ypeu8hno gia tis ru8mhseis tou player
//Gia to pws 8a ton prosarmozoume mesa sto network
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;
	[SerializeField]
	string remoteLayerName = "RemotePlayer";

	[SerializeField]
	string dontDrawLayerName = "DontDraw";
	[SerializeField]
	GameObject playerGraphics;

	[SerializeField]
	GameObject playerUIPrefab;
	[HideInInspector]
	public GameObject playerUIInstance;

	

	void Start()
	{
		//elegxei an o player einai local player an den einai tote 
		//apenergopoiei ta stoixeia ta opoia 8a prepei na einai energopoihmena mono ston Local player
		if (!isLocalPlayer)
		{
			DisableComponents();
			AssignRemoteLayer();
		}
		else
		{ 

			//apenergopoiei ta grafika gia tn local player
			SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

			//dhmiourgei to UI(crosshair)
			playerUIInstance = Instantiate(playerUIPrefab);
			playerUIInstance.name = playerUIPrefab.name;

			//Diamorfwsh tou UI 
			PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
			if (ui == null)
				Debug.LogError("No PlayerUI component on PlayerUI prefab.");
			ui.SetPlayer(GetComponent<Player>());


			GetComponent<Player>().SetupPlayer();

			string _username = "Loading..";
			//if (UserAccountManager.isLoggedIn)
			//	_username = UserAccountManager.LoggedIn_data;
			//else
				_username = transform.name;
			CmdSetUsername(transform.name, _username);
		}

	}
	//stoixeia pou einai mesa sto command dinoun thn dunatothta na metaferontai se olous tous client mesa ston server
	[Command]
	void CmdSetUsername(string playerID,string username)
	{
		Player player = GameManager.GetPlayer(playerID);
		if (player != null)
		{
			Debug.Log(username + " has joined !");
			player.username = username;
		}
	}

	void SetLayerRecursively(GameObject obj, int newLayer)
	{
		obj.layer = newLayer;

		foreach (Transform child in obj.transform)
		{
			SetLayerRecursively(child.gameObject, newLayer);
		}
	}

	//xrhsimopoioume thn override giati 8eloume na tre3ei se ena sugkekrimeno gegonos dld otan mpei kapoios client 
	public override void OnStartClient()
	{
		base.OnStartClient();

		string _netId = GetComponent<NetworkIdentity>().netId.ToString();
		Player _player = GetComponent<Player>();

		GameManager.RegisterPlayer(_netId, _player);
	}


	void AssignRemoteLayer()
	{
		gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
	}
	//stoixeia pou 8eloume na apenergopoihsoume ston ispector
	void DisableComponents()
	{
		for (int i = 0; i < componentsToDisable.Length; i++)
		{
			componentsToDisable[i].enabled = false;
		}	
	}


	//katastrwfeas 
	void OnDisable()
	{
	 //apenergopoiei to crosshair apo thn o8onh gia osh wra eimaste dead 
		Destroy(playerUIInstance);
		if (isLocalPlayer)
			GameManager.instance.SetSceneCameraActive(true);

		GameManager.UnregisterPlayer(transform.name);
	}
}
