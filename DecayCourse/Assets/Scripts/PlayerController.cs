using StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    enum PlayerState {
        Grounded,
        Jumping,
        Airborne
    }
    private PlayerState State = PlayerState.Airborne;
    [SerializeField]
    private AnimationCurve JumpStrengthCurve;
    [SerializeField]
    private float JumpStrengthMultiplier = 1;

    public float JumpProgress { get; private set; }
    [SerializeField]
    private float JumpProgressMultiplier = 1;

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


    // Use this for initialization
    void Start () {
        transform.position = new Vector3(CourseBehaviour.Main.GridSize.x / 4, 1, CourseBehaviour.Main.GridSize.y / 4);
    }
	
	// Update is called once per frame
	void Update () {
        if (!GameManager.Running) {
            return;
        }

        switch (State) {
            case PlayerState.Grounded:
                if (Input.GetButton("Jump")) {
                    PrepareJump();
                }
                break;
            case PlayerState.Jumping:
                if (Input.GetButton("Jump")) {
                    PrepareJump();
                } else {
                    Jump();
                }
                break;
            case PlayerState.Airborne:
                if (Mathf.Approximately(GetComponent<Rigidbody>().velocity.y, 0)) {
                    Land();
                }
                break;
        }
        
		AccelerationTime += Time.deltaTime;
		CurrentSpeed = Mathf.Lerp(CurrentSpeed, BaseSpeed * AccelerationCurve.Evaluate(AccelerationTime), Time.deltaTime * Acceleration);
		
		transform.Translate(-transform.forward * CurrentSpeed * Time.deltaTime);

        var turnSpeed = TurnSpeed + (-90) * Mathf.Clamp(Input.GetAxis("Vertical"), -1, 0);

		float horizontal = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
		transform.Rotate(0, 0, -horizontal);
	}

    private void Land() {
        State = PlayerState.Grounded;
        JumpProgress = 0;
    }
    private void PrepareJump() {
        State = PlayerState.Jumping;
        JumpProgress = Mathf.Clamp(JumpProgress + Time.deltaTime * JumpProgressMultiplier, 0, 1);
    }
    private void Jump() {
        State = PlayerState.Airborne;
        Vector3 atas = new Vector3(0, JumpStrengthMultiplier * JumpStrengthCurve.Evaluate(JumpProgress), 0);
        GetComponent<Rigidbody>().AddForce(atas, ForceMode.Impulse);
    }

    public void Die() {
        Camera.main.transform.parent = null;
        Destroy(gameObject);
    }
}
