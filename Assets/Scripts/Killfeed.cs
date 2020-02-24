using UnityEngine;
using System.Collections;

public class Killfeed : MonoBehaviour
{

	[SerializeField]
	GameObject KillfeedItemPrefab;

	void Start()
	{
		GameManager.instance.onPlayerKilledCallBack += OnKill;
	}

	public void OnKill(string player, string source)
	{
		GameObject go = (GameObject)Instantiate(KillfeedItemPrefab, this.transform);//bazoume ta grafika sto panel tou killfeed
		go.GetComponent<KillfeedItem>().Setup(player, source);
		go.transform.SetAsFirstSibling();//h seira pou 8a emfanizontai ta onomata sto killfeed 8a einai apo to neotero pros to palaiotero 
		
		Destroy(go, 5f);//to text 8a e3afanizete meta apo 5s
	}
}
