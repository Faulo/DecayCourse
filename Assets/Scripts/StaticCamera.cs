using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//transform.position = new Vector3(CourseBehaviour.Main.GridSize.x/4, 0, CourseBehaviour.Main.GridSize.y/4);
		Camera.main.orthographicSize = CourseBehaviour.Instance.GridSize.magnitude / 5;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
