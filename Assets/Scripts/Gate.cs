﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class Gate : MonoBehaviour
{
	public Action GatePassed;
	
	public Parallax ParallaxToFollow;
	public float XPositionOnAppear;
	public GameObject GreenSprite;
	public GameObject RedSprite;
	public Animator GreenGateAnimator;
	
	private readonly List<GameObject> _children = new List<GameObject>();
	private bool _open;
	private AudioSource _audioSource;
	private Collider2D _collider;

	void Start()
	{
		_collider = GetComponent<Collider2D>();
		_audioSource = GetComponent<AudioSource>();
		
		for (int i = 0; i < transform.childCount; i++)
		{
			_children.Add(transform.GetChild(i).gameObject);
		}
		
		Disappear();
	}

	void Update()
	{
		if (ParallaxToFollow)
		{
			transform.position +=
				Vector3.left * Time.deltaTime * ParallaxToFollow.Speed * ParallaxToFollow.NearestSpeedMultiplier;
		}
	}

	public void Appear(bool open)
	{
		_children.ForEach(c => c.SetActive(true));
		_collider.enabled = true;
		transform.position = new Vector3(XPositionOnAppear, transform.position.y);
		Toggle(open);
	}

	public void Disappear()
	{
		_children.ForEach(c => c.SetActive(false));
		_collider.enabled = false;
	}

	public void Toggle(bool open)
	{
		_open = open;

		GreenSprite.SetActive(_open);
		RedSprite.SetActive(!_open);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			_audioSource.Play();
			GreenGateAnimator.SetTrigger("Pass");
			if (GatePassed != null)
				GatePassed();
		}
	}
}
