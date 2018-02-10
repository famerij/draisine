using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class Gate : MonoBehaviour
{
	public Parallax ParallaxToFollow;
	public float XPositionOnAppear;

	private readonly List<GameObject> _children = new List<GameObject>();

	void Start()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			_children.Add(transform.GetChild(i).gameObject);
		}
		
		Disappear();
	}

	void Update()
	{
		transform.position +=
			Vector3.left * Time.deltaTime * ParallaxToFollow.Speed * ParallaxToFollow.NearestSpeedMultiplier;
	}

	public void Appear()
	{
		_children.ForEach(c => c.SetActive(true));
		transform.position = new Vector3(XPositionOnAppear, transform.position.y);
	}

	public void Disappear()
	{
		_children.ForEach(c => c.SetActive(false));
	}
}
