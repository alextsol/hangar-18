using UnityEngine;

[System.Serializable]
public class PlayerWeapon {

	public string name = "Ak 47";

	public int damage = 10;
	public int  range = 100;
	public float fireRate = 0f;
	public int maxBullets = 20;

	[HideInInspector]
	public int bullets;

	public float reloadTime = 3f;

	public GameObject graphics;
	//dhmiourgia constructor pou bazei ths metablhtes ka8e fora pou auth h classh xrhsimopoieitai 
	public PlayerWeapon()
	{
		bullets = maxBullets;
	}

}
