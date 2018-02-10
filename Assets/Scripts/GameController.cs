using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public float MovementSpeed = 3f;
	public float MovementSpeedIncreaseDivider = 1000f;
	public float WheelSpeedMultiplier = 5f;
	public float AccelerationDuration = 1f;
	public float DecelerationDuration = 1f;

	public float TravelDistance;
	
	[Header("References")]
	public PuzzleController PuzzleController;
	public Parallax ParallaxBackground;
	public List<Transform> Wheels;
	
	private float _startMovementSpeed;
	private bool _stopped;

	private void Start()
	{
		TravelDistance = 0f;
		_startMovementSpeed = MovementSpeed;
	}

	private void Update()
	{
		ParallaxBackground.Speed = MovementSpeed;
		
		Wheels.ForEach(t => t.Rotate(-Vector3.forward, 2 * Mathf.PI * WheelSpeedMultiplier * MovementSpeed * Time.deltaTime));
		
		TravelDistance += Time.deltaTime * MovementSpeed;
		
		if (!_stopped)
			MovementSpeed += Time.deltaTime / MovementSpeedIncreaseDivider;
	}

	[ContextMenu("Stop")]
	private void StopDraisine()
	{
		LerpFloat(MovementSpeed, 0f, DecelerationDuration, (value) => MovementSpeed = value);
		_stopped = true;
	}
	
	[ContextMenu("Start")]
	private void StartDraisine()
	{
		LerpFloat(0f, _startMovementSpeed, AccelerationDuration, (value) => MovementSpeed = value);
		_stopped = false;
	}

	private void LerpFloat(float from, float to, float duration, Action<float> progress)
	{
		StartCoroutine(LerpRoutine(from, to, duration, progress));
	}
	
	private IEnumerator LerpRoutine(float from, float to, float duration, Action<float> progress)
	{
		float t = 0f;

		while (t < 1f)
		{
			progress.Invoke(Mathf.Lerp(from, to, t));
			t += Time.deltaTime/duration;
			yield return new WaitForEndOfFrame();
		}
		
		progress.Invoke(to);
	}
}
