using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayBehaviour : MonoBehaviour {

	[SerializeField]
	float decayCooldown;
	float time;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time >= decayCooldown)
		{
			time = 0;
			RemoveRandomSegment();
		}
	}

	public void RemoveAll()
	{
		while (CourseBehaviour.main.necessarySegments.Count < CourseBehaviour.main.activeSegments.Count)
		{
			RemoveRandomSegment();
		}
	}

	public void RemoveRandomSegment()
	{
		if (CourseBehaviour.main.necessarySegments.Count < CourseBehaviour.main.activeSegments.Count)
		{
			int r = Random.Range(0, CourseBehaviour.main.activeSegments.Count);
			while (CourseBehaviour.main.necessarySegments.Contains(CourseBehaviour.main.activeSegments[r]))
			{
				r = Random.Range(0, CourseBehaviour.main.activeSegments.Count);
			}
			if (!CourseBehaviour.main.RemoveSegment(CourseBehaviour.main.activeSegments[r]))
			{
				RemoveRandomSegment();
			}
		}
	}
}
