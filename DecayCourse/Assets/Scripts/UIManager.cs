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

    public void ShowStartScreen() {
        StartScreen.gameObject.SetActive(true);
        //FindObjectOfType<PlayerController>().gameObject.SetActive(false);
    }
    public void HideStartScreen() {
        StartScreen.gameObject.SetActive(false);
        //FindObjectOfType<PlayerController>().gameObject.SetActive(true);
    }

    public void ShowGame() {

    }
    public void UpdateGame() {
        Timer.text = System.TimeSpan.FromSeconds((int)GameManager.GameTime).ToString();
        HighScoreData.Update(GameManager.GameTime, Timer.text);
        HighScore.text = "High score:\n" + HighScoreData.Text;
    }
    public void HideGame() {

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
