using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI Timer;
    [SerializeField]
    private RectTransform StartScreen;
    [SerializeField]
    private TextMeshProUGUI GameOver;
    [SerializeField]
    private TextMeshProUGUI HighScore;

    private ProgressBar JumpBar {
        get {
            return transform.Find("JumpBar").GetComponent<ProgressBar>();
        }
    }
    private PlayerController Player {
        get {
            return FindObjectOfType<PlayerController>();
        }
    }

    public void ShowStartScreen() {
        StartScreen.gameObject.SetActive(true);
    }
    public void HideStartScreen() {
        StartScreen.gameObject.SetActive(false);
    }

    public void ShowGame() {
        JumpBar.gameObject.SetActive(true);
        GetComponent<AudioSource>().Play();
    }
    public void UpdateGame() {
        Timer.text = System.TimeSpan.FromSeconds((int)GameManager.GameTime).ToString();
        HighScoreData.Update(GameManager.GameTime, Timer.text);
        HighScore.text = "High score:\n" + HighScoreData.Text;
        if (Player != null) {
            JumpBar.Progress = Player.JumpProgress;
        }
    }
    public void HideGame() {
        JumpBar.gameObject.SetActive(false);
    }

    public void ShowGameOver() {
        GameOver.gameObject.SetActive(true);
        Timer.fontSize = 100;
    }
    public void HideGameOver() {
        GameOver.gameObject.SetActive(false);
        Timer.fontSize = 40;
    }
}
