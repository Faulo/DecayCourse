using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour {
	private void OnTriggerEnter(Collider other)
	{
        var player = other.GetComponent<PlayerController>();
        if (player != null) {
            GetComponent<AudioSource>().Play();
            player.Die();
        }
	}
}
