using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<PlayerController>();
        if (player != null) {
            GetComponent<AudioSource>().Play();
            player.Win();
        }
    }
}
