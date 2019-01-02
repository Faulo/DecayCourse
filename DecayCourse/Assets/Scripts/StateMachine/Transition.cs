using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine {
    public class Transition {
        public State OldState;
        public State NewState;
        public ICondition Condition;

        internal Transition(State oldState, State newState, ICondition condition) {
            OldState = oldState;
            NewState = newState;
            Condition = condition;
        }

        public override string ToString() {
            return OldState + " => " + Condition + " => " + NewState;
        }
    }
}