﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Left, Right }

public class PuzzleBlock : MonoBehaviour
{
	public SpriteRenderer SymbolRenderer;
	public List<GameObject> ActivatedSprites;
	public List<Sprite> SymbolSprites;
	public AudioClip CollisionSound;

	private AudioSource _audioSource;

	public Sprite CurrentSprite
	{
		get { return SymbolSprites[_currentSpriteIndex]; }
	}

	private int _currentSpriteIndex;
	
	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		_currentSpriteIndex = Random.Range(0, SymbolSprites.Count);
		UpdateSprite();
	}

	private void UpdateSprite()
	{
		SymbolRenderer.sprite = SymbolSprites[_currentSpriteIndex];
	}
	
	public void ChangeSprite(Direction direction)
	{
		switch (direction)
		{
			case Direction.Left:
				if (_currentSpriteIndex != 0)
				{
					_currentSpriteIndex--;
					UpdateSprite();
					_audioSource.volume = 1f;
					_audioSource.pitch = .95f;
					_audioSource.Play();
				}
				else
				{
					// TODO: Fail effect
				}
				break;
			case Direction.Right:
				if (_currentSpriteIndex < SymbolSprites.Count - 1)
				{
					_currentSpriteIndex++;
					UpdateSprite();
					_audioSource.volume = 1f;
					_audioSource.pitch = 1.05f;
					_audioSource.Play();
				}
				else
				{
					// TODO: Fail effect
				}
				break;
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.relativeVelocity.magnitude > 5f && other.gameObject.CompareTag("Base"))
		{
			_audioSource.volume = .3f;
			_audioSource.PlayOneShot(CollisionSound);
		}
	}
}
