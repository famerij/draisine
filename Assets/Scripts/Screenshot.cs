using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			ScreenCapture.CaptureScreenshot(string.Format("screen_{0}.png", 
				DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")));
		}
	}
}
