using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	public float Speed;
	
	public Transform Far;
	public Transform Near;
	public Transform Nearest;
	public Transform Front;
	
	public float FarSpeedMultiplier;
	public float NearSpeedMultiplier;
	public float NearestSpeedMultiplier;
	public float FrontSpeedMultiplier; 
	
	void Update()
	{
		if (Far != null)
			Far.transform.position += Vector3.left * Time.deltaTime * Speed * FarSpeedMultiplier;
		
		if (Near != null)
			Near.transform.position += Vector3.left * Time.deltaTime * Speed * NearSpeedMultiplier;
		
		if (Nearest != null)
			Nearest.transform.position += Vector3.left * Time.deltaTime * Speed * NearestSpeedMultiplier;
		
		if (Front != null)
			Front.transform.position += Vector3.left * Time.deltaTime * Speed * FrontSpeedMultiplier;
	}
}
