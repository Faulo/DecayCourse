using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour {

    private GameManager Game;

    // Use this for initialization
    void Start()
	{
        Game = FindObjectOfType<GameManager>();
        transform.localScale = new Vector3(CourseBehaviour.Main.GridSize.x * 5, 0, CourseBehaviour.Main.GridSize.y * 5);
    }

	private void OnTriggerEnter(Collider other)
	{
		Game.GameOver = true;
		Camera.main.transform.parent = null;
		Destroy(other.gameObject);
	}

}
