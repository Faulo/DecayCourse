﻿using System.Collections;
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
        yield return GetComponent<Animator>().PlayAndWait("Blink");
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
