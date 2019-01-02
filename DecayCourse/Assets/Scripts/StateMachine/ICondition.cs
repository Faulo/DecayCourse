using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine {
    public interface ICondition {
        bool Triggered(StateController controller);
    }
}