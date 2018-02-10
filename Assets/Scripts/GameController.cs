using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static Action StartEngine;
	
	public float MovementSpeed = 3f;
	public float MovementSpeedIncreaseDivider = 1000f;
	public float MovementSpeedBoost = .5f;
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
	private int _stoppedCount;
	private bool _newPuzzle;

	private void Start()
	{
		TravelDistance = 0f;
		_startMovementSpeed = MovementSpeed;
		MovementSpeed = 0f;

		StartEngine += StartDraisine;

		_stopped = false;
		Draisine.StartMovement();
	}

	private void OnDisable()
	{
		StartEngine -= StartDraisine;
	}

	private void Update()
	{
		ParallaxBackground.Speed = MovementSpeed;
		
		Wheels.ForEach(t => t.Rotate(-Vector3.forward, 2 * Mathf.PI * WheelSpeedMultiplier * MovementSpeed * Time.deltaTime));
		
		TravelDistance += Time.deltaTime * MovementSpeed;

		if (!_stopped)
		{
			if (TravelDistance % 50f < 1f)
			{
				TravelDistance += 1f;
				_stoppedCount++;

				if (_stoppedCount > 1)
				{
					if (PuzzleController.ValidatePuzzle())
					{
						// TODO Turn gate green
						Gate.Appear(true);
						MovementSpeed += MovementSpeedBoost;
						DelayedCall(() => PuzzleController.CreateNewPuzzle(), 2f);
					}
					else
					{
						StopDraisine();
					}
				}
			}
			
			MovementSpeed += Time.deltaTime / MovementSpeedIncreaseDivider;
		}
		else
		{
			if (PuzzleController.ValidatePuzzle() && !_newPuzzle)
			{
				Gate.Toggle(true);
				Draisine.StartMovement();
				_newPuzzle = true;
				DelayedCall(() =>
				{
					PuzzleController.CreateNewPuzzle();
					_newPuzzle = false;
				}, 2f);
			}
		}
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
			Gate.Appear(false);
			LerpFloat(MovementSpeed, 0f, 2f, (value) => MovementSpeed = value, () =>
			{
				MovementSpeed = 0f;
			});
		});
		
		Draisine.StopMovement();
		PuzzleController.JumpBlocks();
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
	
	private void DelayedCall(Action action, float delay)
	{
		StartCoroutine(DelayedCallRoutine(action, delay));
	}

	private IEnumerator DelayedCallRoutine(Action action, float delay)
	{
		yield return new WaitForSeconds(delay);
		action();
	}
}
