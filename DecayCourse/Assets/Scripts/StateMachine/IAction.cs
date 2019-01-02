using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine {
    public interface IAction {
        IEnumerator OnEnter(StateController controller);
        IEnumerator OnExit(StateController controller);
        IEnumerator OnUpdate(StateController controller);
    }
}