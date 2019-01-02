using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAirborneCondition : ICondition {
    public bool Triggered(StateController controller) {
        return !Mathf.Approximately(controller.GetComponent<Rigidbody>().velocity.y, 0);
    }
}
