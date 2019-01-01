using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = new Vector3(CourseBehaviour.Main.GridSize.x/2 - CourseBehaviour.Main.CourseWidth/2, 0, 0);
		Camera.main.orthographicSize = CourseBehaviour.Main.GridSize.magnitude / 5;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
