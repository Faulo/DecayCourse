using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine {
    public class DelegatedCondition : ICondition {
        private ConditionDelegate Condition;

        public DelegatedCondition(ConditionDelegate condition) {
            Condition = condition;
        }

        public bool Triggered(StateController controller) {
            return Condition(controller);
        }
    }
}