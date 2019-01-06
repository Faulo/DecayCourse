using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public enum GameState {
        FirstStart,
        Running,
        GameOver
    }
    public static GameState State = GameState.FirstStart;
    public static bool Running {
        get {
            return State == GameState.Running;
        }
    }
    public static bool GameOver {
        get {
            return State == GameState.GameOver;
        }
    }

    public static float GameTime { get; private set; }

    private PlayerController Player;
    private UIManager UI;

    // Use this for initialization
    void Start() {
        GameTime = 0;
        Player = FindObjectOfType<PlayerController>();
        UI = FindObjectOfType<UIManager>();

        UI.HideStartScreen();
        UI.HideGame();
        UI.HideGameOver();

        EnterCurrentState();
    }

    void Update() {
        switch (State) {
            case GameState.FirstStart:
                break;
            case GameState.Running:
                UI.UpdateGame();
                if (Player == null) {
                    TransitionToState(GameState.GameOver);
                } else {
                    GameTime += Time.deltaTime;
                }
                break;
            case GameState.GameOver:
                break;
        }

        if (Input.GetButton("Cancel")) {
            TransitionToState(GameState.Running);
            SceneManager.LoadScene(0);
        }
    }

    private void TransitionToState(GameState newState) {
        ExitCurrentState();
        State = newState;
        EnterCurrentState();
    }

    private void EnterCurrentState() {
        switch (State) {
            case GameState.FirstStart:
                UI.ShowStartScreen();
                break;
            case GameState.Running:
                UI.ShowGame();
                break;
            case GameState.GameOver:
                UI.ShowGameOver();
                break;
        }
    }
    private void ExitCurrentState() {
        switch (State) {
            case GameState.FirstStart:
                UI.HideStartScreen();
                break;
            case GameState.Running:
                UI.HideGame();
                break;
            case GameState.GameOver:
                UI.HideGameOver();
                break;
        }
    }
}
