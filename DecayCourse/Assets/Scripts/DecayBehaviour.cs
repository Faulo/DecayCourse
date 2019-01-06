using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayBehaviour : MonoBehaviour {
	[SerializeField]
	private float DecayCooldown;
    private float Time;

    private void Update () {
        if (!GameManager.Running) {
            return;
        }

        Time += UnityEngine.Time.deltaTime;
		if (Time >= DecayCooldown)
		{
			Time = 0;
            CourseBehaviour.Instance.RemoveRandomSegment();
        }
	}
}
