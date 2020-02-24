using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour {
	//to delegate dexete sunarthseis , 8a mporei na parei plhrofories apo oles tis sunarthseis pou 8a to "fortwsoume"
	public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
	private JoinRoomDelegate joinRoomCallBack; 

	[SerializeField]
	private Text roomNameText;

	private MatchInfoSnapshot match;

	public void Setup (MatchInfoSnapshot _match,JoinRoomDelegate _joinRoomCallBack)
	{
		match = _match;
		joinRoomCallBack = _joinRoomCallBack;

		roomNameText.text = match.name + "(" + match.currentSize + "/" + match.maxSize + ")";

	}

	public void JoinRoom()
	{
		joinRoomCallBack.Invoke(match);
		

	}
}
