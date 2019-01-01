using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public float GameTime { get; private set; }
    public bool GameStarted { get; private set; }
    public bool GameOver { get; set; }

    // Use this for initialization
    void Start () {
		GameTime = 0;
        GameStarted = false;
        GameOver = false;
        GameStarted = true;
    }
	
	void Update () {
        if (GameStarted) {
            GameTime += Time.deltaTime;
        }
    }
}
