using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]

public class PlayerShoot : NetworkBehaviour
{

	private const string PLAYER_TAG = "Player";
	//gia ton hxo 
	public GameObject gunShot;


	private PlayerWeapon currentWeapon;
	private WeaponManager weaponManager;


	[SerializeField]
	private Camera cam;

	[SerializeField]
	private LayerMask mask;


	void Start()
	{

		if (cam == null)
		{
			Debug.LogError("PlayerShoot : No camera referenced");
			this.enabled = false;
		}
		weaponManager = GetComponent<WeaponManager>();
	}

	void Update()
	{
		currentWeapon = weaponManager.GetCurrentWeapon();

		if (PauseMenu.IsOn)
			return;

		if ((currentWeapon.bullets < currentWeapon.maxBullets) && (Input.GetKeyDown(KeyCode.R)))
		{
			weaponManager.Reload();
			return;
		}

		if (currentWeapon.fireRate <= 0f)
		{

			if (Input.GetButtonDown("Fire1"))//ka8e fora pou 8a kanoume click 8a kaleitai h sunarthsh shoot
			{
				Shoot();
			}
		}
		else
		{
			if (Input.GetButtonDown("Fire1"))
			{
				InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate);

			}
			else if (Input.GetButtonUp("Fire1"))
			{
				CancelInvoke("Shoot");
			}
		}
	}
	//kaleitai ston server otan enas paixths purobolei 
	[Command]
	void CmdOnShoot()
	{
		RpcDoShootEffect();
	}
	//metaferetai se olous tous clients otan xrhsimopoioume ena shoot effect
	[ClientRpc]
	void RpcDoShootEffect()
	{
		weaponManager.GetCurrentGraphics().muzzleFlash.Play();
	}
	//kaleitai ston server otan otan xtupame kati kai dexetai tis sunetagmenes sto shmeio pou xtuphsame 
	[Command]
	void CmdOnHit(Vector3 _pos, Vector3 _normal)
	{
		RpcDoHitEffect(_pos, _normal);
	}
	//kaleitai se olous tous client opou  8a emfanizetai to shmeio pou xtupisame me particles
	[ClientRpc]
	void RpcDoHitEffect(Vector3 _pos, Vector3 _normal)
	{
		GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
		Destroy(_hitEffect, 2f);
	}

	[Command]
	void CmdGunShotSync()
	{
		GameObject _gunShot = (GameObject)Instantiate(gunShot, this.transform.position, this.transform.rotation);
		Destroy(_gunShot, 4f);
	}
	[ClientRpc]
	void RpcPlaygunshot()
	{
		CmdGunShotSync();
	}


	[Client] // oles tis plhrofories tis stelnei ston server
	void Shoot()
	{
		if (!isLocalPlayer || weaponManager.isReloading)
		{

			return;
		}


		if (currentWeapon.bullets <= 0)
		{
			weaponManager.Reload();
			return;
		}
		Debug.Log("Remaining Bullets: " + currentWeapon.bullets);

		currentWeapon.bullets--;
		//otan kanoume purobolame kaloume to shoot() ston server 
		CmdOnShoot();

		//Debug.Log("SHOOT!!");
		RaycastHit _hit;

		RpcPlaygunshot();
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, currentWeapon.range, mask))//dhmiourgoume mia dwmh wste o stoxos na einai akribos sto kentro ths o8onhs 
		{
			if (_hit.collider.tag == PLAYER_TAG)//elegxei to antikeimeno pou xtuphsame exei to onoma Player
			{
				CmdPlayerShot(_hit.collider.name, currentWeapon.damage, transform.name);//kalei thn sunarthsh cmdPlayerShot h opoia mas bgazei to onoma tou player pou xtuphsame
			}
			//otan xtupame kati kalei thn OnHit sunarthsh  ston server
			CmdOnHit(_hit.point, _hit.normal);
		}

		if (currentWeapon.bullets <= 0)
		{
			weaponManager.Reload();
		}
	}

	[Command] //dinei plhrofories ston server apo thn meria tou client 
	void CmdPlayerShot(string _playerID, int _damage, string _sourceID)
	{
		Debug.Log(_playerID + " has been shot.");

		Player _player = GameManager.GetPlayer(_playerID);
		_player.RpcTakeDamage(_damage, _sourceID);
	}



}
