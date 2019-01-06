using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    public float Progress {
        get {
            return Foreground.fillAmount;
        }
        set {
            Foreground.fillAmount = value;
        }
    }

    private Image Foreground {
        get {
            return transform.Find("Foreground").GetComponent<Image>();
        }
    }
}
