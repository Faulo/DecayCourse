using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	float baseSpeed;
	[SerializeField]
	float turnSpeed;
	[SerializeField]
	AnimationCurve breakCurve;
	[SerializeField]
	AnimationCurve accelerationCurve;
	[SerializeField]
	float breakForce;
	[SerializeField]
	float acceleration;
	[SerializeField]
	float breakLiningDecaySpeed;

	public static float breakLining;
	float breakTime;
	float accelerationTime;
	float curSpeed;


	// Use this for initialization
	void Start () {
		breakLining = 100;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButton("Jump") && breakLining >=0)
		{
			breakLining -= breakLiningDecaySpeed * Time.deltaTime;
			accelerationTime = 0;
			breakTime += Time.deltaTime;
			curSpeed = Mathf.Lerp(curSpeed, baseSpeed * breakCurve.Evaluate(breakTime), Time.deltaTime*breakForce);
		}else
		{
			breakTime = 0;
			accelerationTime += Time.deltaTime;
			curSpeed = Mathf.Lerp(curSpeed, baseSpeed * accelerationCurve.Evaluate(accelerationTime), Time.deltaTime * acceleration);
		}
		transform.Translate(-transform.forward * curSpeed * Time.deltaTime);

		float horizontal = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
		transform.Rotate(0, 0, -horizontal);
	}
}
