using System.Collections;
using UnityEngine;

public class DestoryGameObjectAfterSoundEffect : MonoBehaviour {

	private float totalTimeBeforeDestroy;

	void Start()
	{
		var sound = this.GetComponent<AudioSource>(); //dexetai ws component to audio source 
		totalTimeBeforeDestroy = sound.clip.length; //8a krathsei oso einai o hxos pou balame mesa sto audio source

	}

	void Update()
	{
		totalTimeBeforeDestroy -= Time.deltaTime; //afaireite me bash ton xrono 

		if (totalTimeBeforeDestroy <= 0f) //otan o xronos ginei 0 katastrefetai
			Destroy(this.gameObject);
	}
}
