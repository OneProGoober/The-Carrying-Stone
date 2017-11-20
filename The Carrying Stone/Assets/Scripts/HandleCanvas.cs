using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HandleCanvas : MonoBehaviour
{
    private CanvasScaler scaler;

	// Use this for initialization
	void Start ()
    {
        scaler = GetComponent<CanvasScaler>();

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
	}
}
