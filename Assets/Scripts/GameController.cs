using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public static Action StartEngine;
	public static Action PuzzleSolved;
	
	public float MovementSpeed = 3f;
	public float WheelSpeedMultiplier = 5f;
	public float AccelerationDuration = 1f;
	public float DecelerationDuration = 1f;
	
	public float TravelDistance;
	
	[Header("Game Balance")]
	public float MovementSpeedIncreaseDivider = 1000f;
	public float MovementSpeedBoost = .5f;
	public float GateDistanceThreshold = 150f;
	
	
	[Header("References")]
	public PuzzleController PuzzleController;
	public Parallax ParallaxBackground;
	public List<Transform> Wheels;
	public Draisine Draisine;
	public Gate Gate;
	public Score Score;
	
	private float _startMovementSpeed;
	private bool _stopped;
	private int _stoppedCount;
	private bool _newPuzzle;
	private int _score;
	private float _modulo;

	private void Start()
	{
		TravelDistance = 0f;
		_startMovementSpeed = MovementSpeed;
		MovementSpeed = 0f;

		StartEngine += StartDraisine;
		PuzzleSolved += OnPuzzleSolved;

		_stopped = false;
		Draisine.StartMovement();
	}

	private void OnDisable()
	{
		StartEngine -= StartDraisine;
		PuzzleSolved -= OnPuzzleSolved;
	}

	private void Update()
	{
		ParallaxBackground.Speed = MovementSpeed;
		
		Wheels.ForEach(t => t.Rotate(-Vector3.forward, 2 * Mathf.PI * WheelSpeedMultiplier * MovementSpeed * Time.deltaTime));

		float diminisher = 1f;
		if (_score > 0)
			diminisher = Mathf.Clamp(1f - (_score * 0.1f), .4f, 1f);
		TravelDistance += Time.deltaTime * MovementSpeed * diminisher;

		if (!_stopped)
		{
			if (TravelDistance > GateDistanceThreshold && TravelDistance % GateDistanceThreshold < 1f)
			{
				TravelDistance += 1f;
				_stoppedCount++;
				
				if (PuzzleController.ValidatePuzzle())
				{
					Gate.Appear(true);
					MovementSpeed += MovementSpeedBoost;
					_score++;
					Score.UpdateScore(_score);
					DelayedCall(() =>
					{
						PuzzleController.CreateNewPuzzle();
						PuzzleController.ToggleInput(true);
					}, 2f);
				}
				else
				{
					StopDraisine();
					_score = 0;
					Score.UpdateScore(_score);
				}
			}
			
			MovementSpeed += Time.deltaTime / MovementSpeedIncreaseDivider;
		}
		else
		{
			if (PuzzleController.ValidatePuzzle())
			{
				Gate.Toggle(true);
				if (!_newPuzzle)
				{
					Draisine.StartMovement();
					_newPuzzle = true;
					DelayedCall(() =>
					{
						PuzzleController.CreateNewPuzzle();
						_newPuzzle = false;
						PuzzleController.ToggleInput(true);
					}, 2f);
				}
			}
		}
		_modulo = TravelDistance % GateDistanceThreshold;
	}

	private void OnPuzzleSolved()
	{
		float modulo = TravelDistance % GateDistanceThreshold;
		if (modulo < 10f) return;
		float distanceToJumpTo = GateDistanceThreshold - 10f;
		Debug.LogFormat("Puzzle soled at distance {0} % {1} = {2}", TravelDistance, GateDistanceThreshold, modulo);
		if (modulo < distanceToJumpTo)
		{
			TravelDistance = TravelDistance - modulo + distanceToJumpTo;
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
