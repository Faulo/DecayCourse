using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private Image BreakVal;
    [SerializeField]
    private TextMeshProUGUI Time;

    private PlayerController Player;
    private GameManager Game;


    // Use this for initialization
    void Start () {
		Time.gameObject.SetActive(false);
        Player = FindObjectOfType<PlayerController>();
        Game = FindObjectOfType<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		BreakVal.fillAmount = Player.BreakLining / 100;

		if (Game.GameOver)
		{
			Time.gameObject.SetActive(true);
			Time.text = System.TimeSpan.FromSeconds((int)Game.GameTime).ToString();
            if (Input.GetButton("Jump")) {
                SceneManager.LoadScene(0);
            }
        }
	}
}
