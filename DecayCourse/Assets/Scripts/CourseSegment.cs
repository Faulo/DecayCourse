using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseSegment : MonoBehaviour {

	public bool active;
	public bool reachable;
	public List<CourseSegment> neighbors;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DrawLines()
	{
		foreach (CourseSegment seg in neighbors)
		{
			Debug.DrawLine(transform.position, seg.transform.position + 0.5f*Vector3.up, Color.red, 1);
		}
	}

	public void DisappearInstant()
	{
		active = false;
		gameObject.SetActive(false);
	}

	public void Disappear()
	{
		active = false;
		StartCoroutine(TurnOff());
	}

	IEnumerator TurnOff()
	{
		MeshRenderer rend = GetComponent<MeshRenderer>();
		for (int i = 0; i < 6; i++)
		{
			rend.material.color = Color.yellow;
			yield return new WaitForSeconds(0.25f);
			rend.material.color = Color.white;
			yield return new WaitForSeconds(0.25f);
		}
		gameObject.SetActive(false);
	}

	public void AddNeighbor(CourseSegment seg)
	{
		if (seg.active)
		{
			neighbors.Add(seg);
		}
	}
}
