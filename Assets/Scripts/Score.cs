using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
	public GameObject BlockPrefab;
	
	public void UpdateScore(int score)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			Destroy(transform.GetChild(i).gameObject);
		}

		for (int i = 0; i < score; i++)
		{
			var blockObj = Instantiate(BlockPrefab, transform);
		}
	}
}
