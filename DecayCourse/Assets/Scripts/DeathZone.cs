using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
		transform.localScale = new Vector3(CourseBehaviour.main.gridSize.x * 5, 0, CourseBehaviour.main.gridSize.y * 5);
	}

	private void OnTriggerEnter(Collider other)
	{
		GameManager.gameOver = true;
		Camera.main.transform.parent = null;
		Destroy(other.gameObject);
	}

}
