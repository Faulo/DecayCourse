using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static float gameTime;
	public static bool gameOver;

	float time = 0;

	// Use this for initialization
	void Start () {
		time = 0;
		gameTime = 0;
		gameOver = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameOver)
		{
			gameTime += Time.deltaTime;
		}else
		{
			time += Time.deltaTime;
		}
		if (time >=3)
		{
			SceneManager.LoadScene(0);
		}
	}
}
