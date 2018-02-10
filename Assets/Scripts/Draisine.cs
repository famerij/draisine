using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draisine : MonoBehaviour
{
	public AudioSource EngineAudioSource;
	
	private Animator _animator;

	void Start()
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

	public void PlayStartEngineSound()
	{
		//TODO Start sound
	}
	
	public void EngineStarted()
	{
		EngineAudioSource.Play();
	}
}
