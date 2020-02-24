using UnityEngine;
using UnityEngine.UI;

public class KillfeedItem : MonoBehaviour
{

	[SerializeField]
	Text text;
	//to text pou 8a grafei sto killfeed panw de3ia sthn o8onh 8a proerxete apo auth th sunarthsh 
	public void Setup(string player, string source)
	{
		text.text = "<b>" + "<color=blue>" + source + "</color>" + "</b>" + " killed " + "<b> " + "<color=red>" + player + "</color>" + "</b>";
	}
}
//<b> <color=blue > Player 1 </color> </b> killed<b> <color=red> Player 2</color> </b> ! 