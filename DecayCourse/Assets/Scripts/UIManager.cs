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
    private TextMeshProUGUI GameOver;
    [SerializeField]
    private TextMeshProUGUI HighScore;

    private PlayerController Player;
    private GameManager Game;


    // Use this for initialization
    void Start () {
        GameOver.gameObject.SetActive(false);
        Player = FindObjectOfType<PlayerController>();
        Game = FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        Timer.text = System.TimeSpan.FromSeconds((int)Game.GameTime).ToString();
        HighScoreData.Update(Game.GameTime, Timer.text);
        HighScore.text = "High score:\n" + HighScoreData.Text;

        if (Game.GameOver)
		{
            GameOver.gameObject.SetActive(true);
            Timer.fontSize = 100;
        }

        if (Input.GetButton("Cancel")) {
            SceneManager.LoadScene(0);
        }
    }
}
