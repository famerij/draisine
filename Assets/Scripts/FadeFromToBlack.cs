using System.Collections;
using System.Collections.Generic;
using SimpleEasing;
using UnityEngine;

public class FadeFromToBlack : MonoBehaviour
{
	public CanvasGroup CanvasGroup;
	public float Duration;
	public bool In;
	
	private void Start()
	{
		if (!In)
		{
			CanvasGroup.FadeTo(0f, Duration, EasingTypes.Linear);
		}
	}

	public void FadeIn()
	{
		CanvasGroup.FadeTo(1f, Duration, EasingTypes.Linear);
	}
}
