using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(ConfigurableJoint))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 5f;
	[SerializeField]
	private float lookSensitivity = 3f;
	[SerializeField]
	private float thrusterForce = 2000f;




	[Header("Spring Settings:")]
	[SerializeField]
	private float jointSpring= 20f;
	[SerializeField]
	private float jointMaxForce = 40f;


	//components
	private PlayerMotor motor;
	private ConfigurableJoint joint;
	private Animator animator;

	void Start()
	{
		motor = GetComponent<PlayerMotor>();
		joint = GetComponent<ConfigurableJoint>();
		animator = GetComponent<Animator>();

		SetJointSettings(jointSpring);
	}

	void Update()
	{
		if (PauseMenu.IsOn)
		{
			if (Cursor.lockState != CursorLockMode.None)
			{
				Cursor.lockState = CursorLockMode.None;

				motor.Move(Vector3.zero);
				motor.Rotate(Vector3.zero);
				motor.RotateCamera(0f);
			}
			return;
		}

		if (Cursor.lockState != CursorLockMode.Locked)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		//ru8mish 8eshs stoxou 
		//epishs kai energopoihsh twn physics gia na energopoihsoume thn baruthta 

		float _xMov = Input.GetAxis("Horizontal");
		float _zMov = Input.GetAxis("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov;
		Vector3 _movVertical = transform.forward * _zMov;

		//teliko dianusma ths kinhshs
		Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

		//kinhsh tou animation
		animator.SetFloat("ForwardVelocity", _zMov);

		//efarmogh kinhshs
		motor.Move(_velocity);

		//upologismos tou rotation san 3d dianusma 
		float _yRot = Input.GetAxis("Mouse X");

		Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

		//efarmogh tou rotation
		motor.Rotate(_rotation);

		//upologismos to rotation ths kameras  san 3d dianusma 
		float _xRot = Input.GetAxis("Mouse Y");

		float  _cameraRotationX = _xRot * lookSensitivity;
		//efarmogh tou rotation ths kameras 
		motor.RotateCamera(_cameraRotationX);

		//upologismos dunamhs w8hshs 
		Vector3 _thrusterForce = Vector3.zero;
		if (Input.GetButton("Jump"))
		{
			_thrusterForce = Vector3.up * thrusterForce;
			SetJointSettings(0f);
		}else
		{
			SetJointSettings(jointSpring);
		}
		//efarmogh ths w8hshs
		motor.ApplyThruster(_thrusterForce);
	}

	private void SetJointSettings(float _jointSpring)
	{
		joint.yDrive = new JointDrive { positionSpring = jointSpring, maximumForce = jointMaxForce};
	}

}
