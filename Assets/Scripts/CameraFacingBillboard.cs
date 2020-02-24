//script to opoio allazei ta angle ths camera na blepoun ena sugkerkimeno shmeio apo oles tis pleures 
//emeis to xreiazomaste gia na blepoume to healthbar kai to name tou player xwris na exei gonies 
using UnityEngine;
using System.Collections;

public class CameraFacingBillboard : MonoBehaviour {

	void Update()
	{
		Camera cam = Camera.main;

		transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward,
			cam.transform.rotation * Vector3.up);
	}
}
