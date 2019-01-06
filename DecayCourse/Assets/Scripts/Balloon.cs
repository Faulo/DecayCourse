using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {
    Material MeshMaterial {
        get {
            return GetComponent<MeshRenderer>().material;
        }
    }

    [SerializeField]
    private AnimationCurve SizeToScaleCurve;

    private int Size {
        get {
            return SanitizedSize;
        }
        set {
            float scale = SizeToScaleCurve.Evaluate(value);
            transform.localScale = new Vector3(scale, scale, scale);
            SanitizedSize = value;
        }
    }
    private int SanitizedSize;

    private void OnTriggerEnter(Collider other) {
        var player = other.GetComponent<PlayerController>();
        if (player != null) {
            CourseBehaviour.Main.RespawnSegments(transform.position, MeshMaterial.color, Size);
            CourseBehaviour.Main.SpawnBalloon();
            Destroy(gameObject);
        }
    }

    void Start() {
        MeshMaterial.color = new Color(Random.Range(0.25f, 0.75f), Random.Range(0.25f, 0.75f), Random.Range(0.25f, 0.75f));
        Size = Random.Range(1, 4);
    }
}
