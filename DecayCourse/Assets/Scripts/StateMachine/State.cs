using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine {
    [Serializable]
    public class State {
        public string Id { get; private set; }
        public IAction Action { get; private set; }

        internal State(string id, IAction action) {
            Id = id;
            Action = action;
        }

        public override string ToString() {
            return Id;
        }
    }
}