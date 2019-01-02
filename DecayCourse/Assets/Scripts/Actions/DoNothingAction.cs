using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothingAction : IAction {
    public IEnumerator OnEnter(StateController controller) {
        yield return null;
    }

    public IEnumerator OnExit(StateController controller) {
        yield return null;
    }

    public IEnumerator OnUpdate(StateController controller) {
        yield return null;
    }
}
