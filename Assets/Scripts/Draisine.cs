using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draisine : MonoBehaviour
{
	public AudioSource EngineAudioSource;
	public AudioSource SwitchAudioSource;
	public AudioSource TrackAudioSource;

	public AudioClip EngineStopSound;
	public AudioClip TrainStopSound;
	
	private Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	public void StartMovement()
	{
		_animator.SetBool("Drive", true);
	}
	
	public void StopMovement()
	{
		_animator.SetBool("Drive", false);
	}

	public void EngineStopped()
	{
		EngineAudioSource.Stop();
	}

	public void EngineBreak()
	{
		SwitchAudioSource.PlayOneShot(EngineStopSound);
	}

	public void PlayStartEngineSound()
	{
		SwitchAudioSource.Play();
	}
	
	public void PlayTrainStopSound()
	{
		TrackAudioSource.Stop();
		TrackAudioSource.PlayOneShot(TrainStopSound);
	}
	
	public void EngineStarted()
	{
		EngineAudioSource.Play();
		TrackAudioSource.Play();
		if (GameController.StartEngine != null)
			GameController.StartEngine.Invoke();
	}
}
