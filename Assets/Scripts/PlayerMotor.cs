using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

	[SerializeField]
	private Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private float cameraRotationX = 0f;
	private float currentCameraRotatioX = 0f;
	private Vector3 thrusterForce = Vector3.zero;

	[SerializeField]
	private float cameraRotationLimit = 85f;

	private Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody>();

	}
	//dexetai ena dianusma gia thn kinhsh
	public void Move(Vector3 _velocity)
	{
		velocity = _velocity;
	}
	//dexetai ena dianusma gia to rotate ston a3ona y
	public void Rotate(Vector3 _rotation)
	{
		rotation = _rotation;
	}
	//dexetai ena dianusma gia to rotate ths kameras
	public void RotateCamera(float _cameraRotationX)
	{
		cameraRotationX = _cameraRotationX;
	}
	//dexetai ena dianusma gia thn w8hsh tou player
	public void ApplyThruster(Vector3 _thrusterForce)
	{
		thrusterForce = _thrusterForce;
	}
	//trexei se ka8e fusikh epanalhpsh 
	void FixedUpdate()
	{
		PerformMovement();
		PerformRotation();
	}

	//kanei kinhsh me bash to velocity
	void PerformMovement()
	{
		if (velocity != Vector3.zero)
		{
			rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
		}

		if (thrusterForce != Vector3.zero)
		{
			rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);//to forcemode.acceleration au3anei thn epitaxunsh tou player agnoontas thn maza tou 
		}

	}

	void PerformRotation()
	{
		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		if(cam != null)
		{
			//ypologismos tou rotation ths cameras kai to mechanism gia na mhn gurizei 360 moires ston a3ona y
			currentCameraRotatioX -= cameraRotationX;
			currentCameraRotatioX = Mathf.Clamp(currentCameraRotatioX, -cameraRotationLimit, cameraRotationLimit);
			//efarmogh tou rotation kai to bazoume san transform mesa sthn camera
			cam.transform.localEulerAngles = new Vector3(currentCameraRotatioX, 0f, 0f);
		}
	}
}
