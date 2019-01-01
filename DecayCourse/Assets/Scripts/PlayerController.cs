using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[SerializeField]
	float BaseSpeed;

	[SerializeField]
	float TurnSpeed;

    [SerializeField]
	AnimationCurve BreakCurve;

    [SerializeField]
	AnimationCurve AccelerationCurve;

    [SerializeField]
	float BreakForce;

    [SerializeField]
	float Acceleration;

    [SerializeField]
	float BreakLiningDecaySpeed;

    public float BreakLining {get; private set;}
    public float BreakTime { get; private set; }
    public float AccelerationTime { get; private set; }
    public float CurrentSpeed { get; private set; }


    // Use this for initialization
    void Start () {
		BreakLining = 100;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButton("Jump") && BreakLining >=0)
		{
			BreakLining -= BreakLiningDecaySpeed * Time.deltaTime;
			AccelerationTime = 0;
			BreakTime += Time.deltaTime;
			CurrentSpeed = Mathf.Lerp(CurrentSpeed, BaseSpeed * BreakCurve.Evaluate(BreakTime), Time.deltaTime*BreakForce);
		}else
		{
			BreakTime = 0;
			AccelerationTime += Time.deltaTime;
			CurrentSpeed = Mathf.Lerp(CurrentSpeed, BaseSpeed * AccelerationCurve.Evaluate(AccelerationTime), Time.deltaTime * Acceleration);
		}
		transform.Translate(-transform.forward * CurrentSpeed * Time.deltaTime);

        var turnSpeed = TurnSpeed + (-90) * Input.GetAxis("Vertical");

		float horizontal = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
		transform.Rotate(0, 0, -horizontal);
	}
}
