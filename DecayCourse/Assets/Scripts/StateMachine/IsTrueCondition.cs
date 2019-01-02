using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine {

    public class IsTrueCondition : ICondition
    {
        public bool Triggered(StateController controller) {
            return true;
        }
    }

}