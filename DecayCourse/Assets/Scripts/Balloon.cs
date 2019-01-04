using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    Material MeshMaterial {
        get {
            return GetComponent<MeshRenderer>().material;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<PlayerController>() != null) {
            CourseBehaviour.Main.RespawnSegments(transform.position, MeshMaterial.color);
            CourseBehaviour.Main.SpawnBalloon();
            Destroy(gameObject);
        }
    }

    void Start() {
        MeshMaterial.color = new Color(Random.Range(0.25f, 0.75f), Random.Range(0.25f, 0.75f), Random.Range(0.25f, 0.75f));
    }
}
