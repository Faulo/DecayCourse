using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayBehaviour : MonoBehaviour {

	[SerializeField]
	float DecayCooldown;
	float Time;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Time += UnityEngine.Time.deltaTime;
		if (Time >= DecayCooldown)
		{
			Time = 0;
			RemoveRandomSegment();
		}
	}

	public void RemoveAll()
	{
        StartCoroutine(RemoveAllRoutine());
	}
    private IEnumerator RemoveAllRoutine() {
        while (CourseBehaviour.Main.NecessarySegments.Count < CourseBehaviour.Main.ActiveSegments.Count) {
            yield return new WaitForFixedUpdate();
            RemoveRandomSegment();
        }
    }


    public void RemoveRandomSegment()
	{
        if (CourseBehaviour.Main.NecessarySegments.Count < CourseBehaviour.Main.ActiveSegments.Count)
		{
			int r = Random.Range(0, CourseBehaviour.Main.ActiveSegments.Count);
			while (CourseBehaviour.Main.NecessarySegments.Contains(CourseBehaviour.Main.ActiveSegments[r]))
			{
				r = Random.Range(0, CourseBehaviour.Main.ActiveSegments.Count);
			}
			if (!CourseBehaviour.Main.RemoveSegment(CourseBehaviour.Main.ActiveSegments[r]))
			{
				RemoveRandomSegment();
			}
		}
	}
}
