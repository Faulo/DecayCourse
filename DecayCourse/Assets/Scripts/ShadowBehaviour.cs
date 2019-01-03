using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBehaviour : MonoBehaviour
{
    float SurfacePosition;

    // Start is called before the first frame update
    void Start()
    {
        SurfacePosition = 0.51f;
    }

    // Update is called once per frame
    void Update()
    {
        float offset = transform.parent.position.y - SurfacePosition;
        transform.localPosition = new Vector3(0, 0, offset);
    }
}
