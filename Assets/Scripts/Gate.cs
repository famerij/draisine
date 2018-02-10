using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class Gate : MonoBehaviour
{
	public Parallax ParallaxToFollow;
	public float XPositionOnAppear;
	public GameObject GreenSprite;
	public GameObject RedSprite;
	
	private readonly List<GameObject> _children = new List<GameObject>();
	private bool _open;
	private AudioSource _audioSource;

	void Start()
	{
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
		transform.position = new Vector3(XPositionOnAppear, transform.position.y);
		Toggle(open);

		if (open)
		{
			_audioSource.Play();
		}
	}

	public void Disappear()
	{
		_children.ForEach(c => c.SetActive(false));
	}

	public void Toggle(bool open)
	{
		_open = open;

		GreenSprite.SetActive(_open);
		RedSprite.SetActive(!_open);
	}
}
