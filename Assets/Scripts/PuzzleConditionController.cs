using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleConditionController : MonoBehaviour
{
	public List<Sprite> Sprites;
	public List<Image> UISprites;

	private readonly List<Sprite> _currentPuzzle = new List<Sprite>();

	public void CreateNewPuzzle(int length)
	{
		_currentPuzzle.Clear();
		for (int i = 0; i < length; i++)
		{
			_currentPuzzle.Add(Sprites[Random.Range(0, Sprites.Count)]);
		}
		
		UpdateUI();
	}

	private void UpdateUI()
	{
		for (int i = 0; i < UISprites.Count; i++)
		{
			if (i >= _currentPuzzle.Count)
				UISprites[i].enabled = false;
			else
			{
				UISprites[i].enabled = true;
				UISprites[i].sprite = _currentPuzzle[i];
			}
		}
	}

	public bool CheckSolution(List<Sprite> solution)
	{
		for (int i = 0; i < solution.Count; i++)
		{
			if (solution[i] != _currentPuzzle[i])
				return false;
		}
		
		CreateNewPuzzle(_currentPuzzle.Count);
		return true;
	}
}
