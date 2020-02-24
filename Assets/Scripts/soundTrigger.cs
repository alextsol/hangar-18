//Ypeu8hno gia to sound effect ths fwtias kai to fade pou kanoume otan apomakrunomaste apo auth 
using UnityEngine;
using System.Collections;
public class soundTrigger : MonoBehaviour {

	public AudioSource source;
	public AudioClip clip;
	private bool isPlayed;

	public void Awake()
	{
		if(source == null)
		source = GetComponent<AudioSource>();

	}

	//kaleite otan mpainoume mesa sthn "perioxh" ths fwtias gia na 3ekinhsei na paizei o hxos 
	public void OnTriggerEnter(Collider other)
	{
		playSound();
	}

	//kaleite otan bgainoume  mesa sthn "perioxh" ths fwtias gia na stamathsei  na paizei o hxos 
	public void OnTriggerExit(Collider other)
	{
		if (source.isPlaying)
		{
			fadeSound();
			StartCoroutine("fadeSound");
		}
	}

	void playSound()
	{
		if (!source.isPlaying)
		{
			source.volume = 0.7f;
			source.Play();
		}
	}

	//dhmiourgoume mia coroutine gia na mas kanei mia "ka8usterimenh epistrofh" wste na kanei fade o hxos kai na mhn kobetai apotoma 
	IEnumerator fadeSound()
	{
		while (source.volume > 0.01f)
		{
			source.volume -= Time.deltaTime / 1f;
			yield return null;
		}
		source.volume = 0f;
		source.Stop();
	}
}
