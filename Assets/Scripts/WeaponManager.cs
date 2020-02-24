//script upeu8hno gia tis ru8miseis sto weapon 
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public class WeaponManager : NetworkBehaviour {


	[SerializeField]
	private string weaponLayerName = "Weapon";
	
	//dhmiourgoume ena placeholder mesa sto opoio 8a "spawnaroume " to weapon
	[SerializeField]
	private Transform weaponHolder;

	[SerializeField]
	private PlayerWeapon primaryWeapon;

	private PlayerWeapon currentWeapon;
	private WeaponGraphics currentGraphics;

	public GameObject reloadSound;

	//sunarthsh gia to reload elegxei an einai true h false
	public bool isReloading = false;

	void Start () {
		//ka8e fora pou 3ekina to game o player 8a exei equipped to primaryweapon
		EquipWeapon(primaryWeapon);	
	}
	//sunarthsh pou 8a dexete an to weapon einai to primary h to secondary kai 8a to epistrefei 
	public PlayerWeapon GetCurrentWeapon()
	{
		return currentWeapon;
	}

	public WeaponGraphics GetCurrentGraphics()
	{
		return currentGraphics;
	}

	void EquipWeapon (PlayerWeapon _weapon)
	{
		currentWeapon = _weapon; //dexetai to orisma ths sunarths _weapon kai to bazei sto current weapon 
		GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);//spawn ta grafika 
		_weaponIns.transform.SetParent(weaponHolder);//to emafinizei sthn 8esh tou weapon holder

		currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
		if(currentGraphics == null)
		{
			Debug.LogError("No WeaponGraphics component on the weapon object:" + _weaponIns.name);
		}

		//elegxos gia an o client einai local dld emeis kai metaferei ta grafika 
		if (isLocalPlayer)
			Util.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));

	}

	public void Reload()
	{
		if (isReloading)
			return;

		StartCoroutine(Reload_Coroutine());

	}
	//to Ienumerator einai mia coroutine h opoio 8a mas epistrepsei mia metablhth meta apo mia ka8usterish 
	private IEnumerator Reload_Coroutine()
	{
		Debug.Log("Reloading..");
		isReloading = true;
		//CmdOnReload();
		//metafora tou hxou gia to reload 
		GameObject _reloadSound = (GameObject)Instantiate(reloadSound, this.transform.position, this.transform.rotation);
		yield return new WaitForSeconds(currentWeapon.reloadTime);
		currentWeapon.bullets = currentWeapon.maxBullets;

		isReloading = false;
	}

	//[Command]
	//void CmdOnReload()
	//{
	//	RpcOnReload();
	//}

	//[ClientRpc]
	//void RpcOnReload()
	//{

	//}

}
