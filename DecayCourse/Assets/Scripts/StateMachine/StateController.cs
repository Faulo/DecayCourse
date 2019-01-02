using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachine {
    public class StateController : MonoBehaviour {
        private State CurrentState;
        private Coroutine CurrentRoutine;

        private Dictionary<string, State> RegisteredStates = new Dictionary<string, State>();
        private HashSet<Transition> RegisteredTransitions = new HashSet<Transition>();

        private Dictionary<string, object> Args = new Dictionary<string, object>();
        public T GetArgument<T>(string key) {
            if (Args.ContainsKey(key)) {
                var val = (T)Args[key];
                if (val != null && val.Equals(null) == false) {
                    return val;
                }
            }
            return default(T);
        }
        public void SetArgument(string key, object val) {
            Args[key] = val;
        }

        private Dictionary<Type, ICondition> Conditions = new Dictionary<Type, ICondition>();
        private T GetCondition<T>() where T : ICondition, new() {
            var key = typeof(T);
            if (!Conditions.ContainsKey(key)) {
                Conditions[key] = new T();
            }
            return (T)Conditions[key];
        }

        private Dictionary<Type, IAction> Actions = new Dictionary<Type, IAction>();
        private T GetAction<T>() where T : IAction, new() {
            var key = typeof(T);
            if (!Actions.ContainsKey(key)) {
                Actions[key] = new T();
            }
            return (T)Actions[key];
        }

        public void AddState<T>(string id) where T : IAction, new() {
            RegisteredStates[id] = new State(id, GetAction<T>());
            if (CurrentState == null) {
                CurrentState = RegisteredStates[id];
                CurrentRoutine = StartCoroutine(EnterStateRoutine());
            }
        }

        private IEnumerator EnterStateRoutine() {
            yield return CurrentState.Action.OnEnter(this);
            CurrentRoutine = null;
        }

        public void AddTransition<T>(string oldStateId, string newStateId) where T : ICondition, new() {
            var oldState = RegisteredStates[oldStateId];
            var newState = RegisteredStates[newStateId];
            RegisteredTransitions.Add(new Transition(oldState, newState, GetCondition<T>()));
        }
        public void AddTransition(string oldStateId, string newStateId) {
            AddTransition<IsTrueCondition>(oldStateId, newStateId);
        }

        public bool InState(string id) {
            return (CurrentState != null && CurrentState.Id == id);
        }

        public void TransitionToState(string id) {
            if (CurrentRoutine != null) {
                StopCoroutine(CurrentRoutine);
            }
            CurrentRoutine = StartCoroutine(TransitionToStateRoutine(RegisteredStates[id]));
        }

        private IEnumerator TransitionToStateRoutine(State newState) {
            yield return CurrentState.Action.OnExit(this);
            CurrentState = newState;
            yield return CurrentState.Action.OnEnter(this);
            CurrentRoutine = null;
        }

        private void Update() {
            if (CurrentState != null) {
                if (CurrentRoutine == null) {
                    foreach (Transition transition in RegisteredTransitions) {
                        if (transition.OldState == CurrentState) {
                            if (transition.Condition.Triggered(this)) {
                                TransitionToState(transition.NewState.Id);
                                return;
                            }
                        }
                    }
                }
                
                CurrentRoutine = StartCoroutine(UpdateStateRoutine());
            }
        }

        private IEnumerator UpdateStateRoutine() {
            yield return CurrentState.Action.OnUpdate(this);
            CurrentRoutine = null;
        }
    }
}
