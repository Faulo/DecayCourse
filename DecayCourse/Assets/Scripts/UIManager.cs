using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {

	public Image breakVal;
	public TextMeshProUGUI time;


	// Use this for initialization
	void Start () {
		time.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		breakVal.fillAmount = PlayerController.breakLining / 100;
		if (GameManager.gameOver)
		{
			time.gameObject.SetActive(true);
			time.text = System.TimeSpan.FromSeconds((int)GameManager.gameTime).ToString();
		}
	}
}
