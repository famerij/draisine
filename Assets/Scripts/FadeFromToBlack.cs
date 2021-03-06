﻿using System;
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
		if (In)
		{
			CanvasGroup.alpha = 1f;
			CanvasGroup.FadeTo(0f, Duration, EasingTypes.Linear);
		}
	}

	public void FadeIn(Action onComplete)
	{
		CanvasGroup.FadeTo(1f, Duration, EasingTypes.Linear, false, TweenRepeat.Once, onComplete);
	}
}
