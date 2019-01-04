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
        CourseBehaviour.Main.RespawnSegments(transform.position, MeshMaterial.color);
        CourseBehaviour.Main.SpawnBalloon();
        Destroy(gameObject);
    }

    void Start() {
        MeshMaterial.color = new Color(Random.Range(0.5f, 1), Random.Range(0.5f, 1), Random.Range(0.5f, 1));
    }
}
