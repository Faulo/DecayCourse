using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseSegment : MonoBehaviour {

	public bool Active;
	public bool Reachable;
	public List<CourseSegment> Neighbors;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DrawLines()
	{
		foreach (CourseSegment seg in Neighbors)
		{
			Debug.DrawLine(transform.position, seg.transform.position + 0.5f*Vector3.up, Color.red, 1);
		}
	}

	public void DisappearInstant()
	{
		Active = false;
		gameObject.SetActive(false);
	}

	public void Disappear()
	{
		Active = false;
		StartCoroutine(TurnOff());
	}

	IEnumerator TurnOff()
	{
		var rend = GetComponent<MeshRenderer>();
		for (int i = 0; i < 3; i++)
		{
            var color = rend.material.color;
            rend.material.color = Color.yellow;
			yield return new WaitForSeconds(0.25f);
			rend.material.color = color;
			yield return new WaitForSeconds(0.25f);
		}
		gameObject.SetActive(false);
	}

	public void AddNeighbor(CourseSegment seg)
	{
		if (seg.Active)
		{
			Neighbors.Add(seg);
		}
	}
}
