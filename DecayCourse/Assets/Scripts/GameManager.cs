using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public enum GameState {
        FirstStart,
        Running,
        GameOver,
        GameWon
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
        set {
            if (value) {
                Instance.TransitionToState(GameState.GameOver);
            }
        }
    }
    public static bool GameWon {
        get {
            return State == GameState.GameWon;
        }
        set {
            if (value) {
                Instance.TransitionToState(GameState.GameWon);
            }
        }
    }

    public static float GameTime { get; private set; }

    private static GameManager Instance {
        get {
            return FindObjectOfType<GameManager>();
        }
    }

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
        UI.HideGameWon();

        EnterCurrentState();
    }

    void Update() {
        switch (State) {
            case GameState.FirstStart:
                break;
            case GameState.Running:
                UI.UpdateGame();
                GameTime += Time.deltaTime;
                if (Player == null) {
                    TransitionToState(GameState.GameOver);
                } else {
                    
                }
                break;
            case GameState.GameOver:
                break;
        }

        if (Input.GetButton("Cancel")) {
            TransitionToState(GameState.Running);
            SceneManager.LoadScene(0);
        }
        if (Input.GetKey(KeyCode.F4)) {
            Application.Quit();
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
            case GameState.GameWon:
                UI.ShowGameWon();
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
            case GameState.GameWon:
                UI.HideGameWon();
                break;
        }
    }
}
