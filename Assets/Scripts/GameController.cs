using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
	public Draisine Draisine;
	public Gate Gate;
	
	private float _startMovementSpeed;
	private bool _stopped;

	private void Start()
	{
		TravelDistance = 0f;
		_startMovementSpeed = MovementSpeed;
		
		StartDraisine();
	}

	private void Update()
	{
		ParallaxBackground.Speed = MovementSpeed;
		
		Wheels.ForEach(t => t.Rotate(-Vector3.forward, 2 * Mathf.PI * WheelSpeedMultiplier * MovementSpeed * Time.deltaTime));
		
		TravelDistance += Time.deltaTime * MovementSpeed;
		
		if (!_stopped)
			MovementSpeed += Time.deltaTime / MovementSpeedIncreaseDivider;
	}

	private bool _gateShown = false;
	
	[ContextMenu("Stop")]
	public void StopDraisine()
	{
		LerpFloat(MovementSpeed, 2f, DecelerationDuration, (value) =>
		{
			MovementSpeed = value;
		}, () =>
		{
			MovementSpeed = 2f;
			Gate.Appear();
			LerpFloat(MovementSpeed, 0f, 2f, (value) => MovementSpeed = value, () => { MovementSpeed = 0f; });
		});
		
		Draisine.StopMovement();
		_stopped = true;
	}

	[ContextMenu("Start")]
	public void StartDraisine()
	{
		LerpFloat(0f, _startMovementSpeed, AccelerationDuration, (value) => MovementSpeed = value, null);
		_stopped = false;
		Draisine.StartMovement();
	}

	private void LerpFloat(float from, float to, float duration, Action<float> progress, Action onDone)
	{
		StartCoroutine(LerpRoutine(from, to, duration, progress, onDone));
	}
	
	private IEnumerator LerpRoutine(float from, float to, float duration, Action<float> progress, Action onDone)
	{
		float t = 0f;

		while (t < 1f)
		{
			progress.Invoke(Mathf.Lerp(from, to, t));
			t += Time.deltaTime/duration;
			yield return new WaitForEndOfFrame();
		}
		
		if (onDone != null) onDone.Invoke();
	}
}
