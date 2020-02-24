//script pou einai upeu8hno gia ta settings sto networking allagh cameras k.a
using UnityEngine;
using System.Collections.Generic;
using System.Linq; //to xrhsimopoioume gia na exoume prosbash sthn sunarthsh ToArray()

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public MatchSettings matchSettings;

	[SerializeField]
	private GameObject sceneCamera;

	public delegate void OnPlayerKilledCallBack(string player, string source);
	public OnPlayerKilledCallBack onPlayerKilledCallBack;


	void Awake()//h awake kaleite meta thn start 
	{
		if(instance != null)
		{
			Debug.LogError("More than one GameManager in scene ");
		}
		else
		{
			instance = this;
		}
	}
	//orismos ths scene cameras
	public void SetSceneCameraActive(bool isActive)
	{
		if (sceneCamera == null)
			return;

		sceneCamera.SetActive(isActive);
	}

	#region Player tracking
	private const string PLAYER_ID_PREFIX = "Player";

	private static Dictionary<string, Player> players = new Dictionary<string, Player>();

	public static void RegisterPlayer(string _netID,Player _player)
	{
		string _playerID = PLAYER_ID_PREFIX + _netID;
		players.Add(_playerID, _player);
		_player.transform.name = _playerID;
	}

	public static void UnregisterPlayer(string _playerID)
	{
		players.Remove(_playerID);
	}

	public static Player GetPlayer(string _playerID)
	{
		return players[_playerID];
	}

	public static Player[] GetAllPlayers()
	{
		return players.Values.ToArray();
	}

	//void OnGUI()
	//{
	//	GUILayout.BeginArea(new Rect(200, 200, 200, 500));
	//	GUILayout.BeginVertical();

	//	foreach (string  _playerID in players.Keys)
	//	{
	//		GUILayout.Label(_playerID + "  -  " + players[_playerID].transform.name);
	//	}

	//	GUILayout.EndVertical();
	//	GUILayout.EndArea();

	//}
	#endregion

	

}
