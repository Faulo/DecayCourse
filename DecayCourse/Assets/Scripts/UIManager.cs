using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI Time;
    [SerializeField]
    private TextMeshProUGUI GameOver;

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
        Time.text = System.TimeSpan.FromSeconds((int)Game.GameTime).ToString();

        if (Game.GameOver)
		{
            GameOver.gameObject.SetActive(true);
            if (Input.GetButton("Jump")) {
                SceneManager.LoadScene(0);
            }
        }
	}
}
