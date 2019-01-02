using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateController))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	float BaseSpeed;

	[SerializeField]
	float TurnSpeed;

    [SerializeField]
	AnimationCurve AccelerationCurve;

    [SerializeField]
	float Acceleration;
    
    public float AccelerationTime { get; private set; }
    public float CurrentSpeed { get; private set; }

    private StateController StateController {
        get {
            return GetComponent<StateController>();
        }
    }


    // Use this for initialization
    void Start () {
        StateController.AddState<DoNothingAction>("Idle");
        StateController.AddState<DoNothingAction>("Airborne");

        StateController.AddTransition<IsAirborneCondition>("Idle", "Airborne");
        StateController.AddTransition<IsGroundedCondition>("Airborne", "Idle");
    }
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButton("Jump") && StateController.InState("Idle"))
		{
            Vector3 atas = new Vector3(0, 2, 0);
            GetComponent<Rigidbody>().AddForce(atas, ForceMode.Impulse);
        }
        
		AccelerationTime += Time.deltaTime;
		CurrentSpeed = Mathf.Lerp(CurrentSpeed, BaseSpeed * AccelerationCurve.Evaluate(AccelerationTime), Time.deltaTime * Acceleration);
		
		transform.Translate(-transform.forward * CurrentSpeed * Time.deltaTime);

        var turnSpeed = TurnSpeed + (-90) * Input.GetAxis("Vertical");

		float horizontal = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
		transform.Rotate(0, 0, -horizontal);
	}
}
