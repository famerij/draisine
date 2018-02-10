using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	public float MovementSpeed = 1f;
	public float WheelSpeedMultiplier = 5f; 
	
	[Header("References")]
	public PuzzleController PuzzleController;
	public Parallax ParallaxBackground;
	public List<Transform> Wheels;

	private void Update()
	{
		ParallaxBackground.Speed = MovementSpeed;
		
		Wheels.ForEach(t => t.Rotate(-Vector3.forward, 2 * Mathf.PI * WheelSpeedMultiplier * MovementSpeed * Time.deltaTime));
	}
}
