using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {

	public GameObject explosionSound;

	private bool _isDead = false;
	public bool isDead
	{
		get { return _isDead; }
		protected set { _isDead = value; }
	}

	[SerializeField]
	private int maxHealth = 100;

	[SyncVar]//ka8e fora pou allazei mia metablhth einai orath se olous tous client 
	private int currentHealth;

	public float GetHealthPct()
	{
		return (float)currentHealth / maxHealth;
	}

	[SyncVar]
	public string username = "Loading.."; 

	public int kills;
	public int deaths;

	[SerializeField]
	private Behaviour[] disableOnDeath;
	private bool[] wasEnabled;
	//kaleite otan to playersetup einai etoimo dld otan apenergopoih8ei/energopoih8ei kapoio component apo to setup script 8a 3ekinhsei auth h sunarthsh 

	[SerializeField]
	private GameObject[] disableGameObjectsOnDeath;


	[SerializeField]
	private GameObject deathEffect;

	[SerializeField]
	private GameObject spawnEffect;

	private bool firstSetup = true;

	public void SetupPlayer()
	{
		if (isLocalPlayer)
		{
			//allagh twn kamerwn 
			GameManager.instance.SetSceneCameraActive(false);
			GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);

			CmdBroadCastNewPlayerSetup();
		}
	}

	[Command]
	private void CmdBroadCastNewPlayerSetup()
	{
		RpcSetupPlayerOnAllClients();
	}

	[ClientRpc]
	private void RpcSetupPlayerOnAllClients()
	{
		if (firstSetup)
		{
			wasEnabled = new bool[disableOnDeath.Length];
			for (int i = 0; i < wasEnabled.Length; i++)
			{
				wasEnabled[i] = disableOnDeath[i].enabled;
			}
			firstSetup = false;
		}
		SetDefautls();
	}

	//void Update()
	//{
	//	if (!isLocalPlayer)
	//		return;

	//	if (Input.GetKeyDown(KeyCode.K))
	//	{
	//		RpcTakeDamage(9999,username);
	//	}
	//}


	[ClientRpc]
	public void RpcTakeDamage(int _amount,string _sourceID)
	{
		if (isDead)
			return;
		currentHealth -= _amount;
		Debug.Log(transform.name + " now has " + currentHealth + "health");
		if (currentHealth <= 0)
		{
			Die(_sourceID);
		}

	}
	




	private void Die(string _sourceID)
	{
		isDead = true;
		
		Player sourcePlayer = GameManager.GetPlayer(_sourceID);
		if(sourcePlayer != null)
		{
			sourcePlayer.kills++;
			GameManager.instance.onPlayerKilledCallBack.Invoke(username, sourcePlayer.username);
		}

		deaths++;

		//components pou 8eloume na apenergopoihsoume otan 8a eimaste dead
		for (int i = 0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = false;
		}
		//gameobjects pou 8eloume na apenergopoihsoume otan 8a eimaste dead 
		for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
		{
			disableGameObjectsOnDeath[i].SetActive(false);
		}

		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = false;

		//emfanizoume ena death effect
		GameObject _gfxINS = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		GameObject _explosionSound = (GameObject)Instantiate(explosionSound, this.transform.position, this.transform.rotation);
		Destroy(_gfxINS, 3f);

		//allagh twn kamerwn 
		if(isLocalPlayer)
		{
			GameManager.instance.SetSceneCameraActive(true);
			GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
		}

		Debug.Log(transform.name + " IS DEADDD");


		StartCoroutine(Respawn());

	}
	//me8odos gia to respawn dld epanaferei ola ta stoixeia pou eixame apenergopoihsei prin sthn sunarthsh Die
	private IEnumerator Respawn()
	{
		yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);//epistrefei to respawn time pou to orizoume apo to GameManager

		
		Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
		transform.position = _spawnPoint.position;
		transform.rotation = _spawnPoint.rotation;

		yield return new WaitForSeconds(0.1f);

		SetupPlayer();

		Debug.Log(transform.name + " respawned !!");
	}
	//ta default stoixeia pou 8a exei ka8e player sto spawn 
	public void SetDefautls()
	{
		isDead = false;

		currentHealth = maxHealth;
		//energopoioume ta components
		for (int i =0; i < disableOnDeath.Length; i++)
		{
			disableOnDeath[i].enabled = wasEnabled[i];
		}
		//energopoioume ta gameobjects
		for (int i = 0; i < disableGameObjectsOnDeath.Length; i++)
		{
			disableGameObjectsOnDeath[i].SetActive(true);
		}

		//energopoioume to collider 
		Collider _col = GetComponent<Collider>();
		if (_col != null)
			_col.enabled = true;


		//spawn effects 
		GameObject _gfxINS = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
		Destroy(_gfxINS, 3f);

	}

}
