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
        //transform.localScale = new Vector3(CourseBehaviour.Main.GridSize.x * 10, 0, CourseBehaviour.Main.GridSize.y * 10);
    }

	private void OnTriggerEnter(Collider other)
	{
        var player = other.GetComponent<PlayerController>();
        if (player != null) {
            player.Die();
        }
	}

}
