using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
		var randomIndices = new int[length];
		for (int i = 0; i < length; i++)
		{
			randomIndices[i] = i;
		}

		string randoms = "";
		for (int i = 0; i < length; i++)
		{
			int randomIndex;
			do
			{
				randomIndex = Random.Range(0, length);
				randomIndices[i] = randomIndex;
			} while (!Validate(randomIndices));

			randoms += randomIndex + " ";
		}

		Debug.LogFormat("Randoms: {0}", randoms);
		
		for (int i = 0; i < randomIndices.Length; i++)
		{
			var index = randomIndices[i];
			_currentPuzzle.Add(Sprites[index]);
		}
		
		UpdateUI();
	}

	private bool Validate(int[] indices)
	{
		int indexCount;
		for (int i = 0; i < indices.Length; i++)
		{
			indexCount = indices.Count(idx => idx == i);
			if (indexCount > 2)
				return false;
		}

		return true;
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
		bool solved = true;
		string text = "Checking Solution\n";
		for (int i = 0; i < solution.Count; i++)
		{
			text += "Try: " + solution[i].name + "\n";
			text += "Solution: " + _currentPuzzle[i].name + "\n";
			if (solution[i] != _currentPuzzle[i])
				solved = false;
		}

		Debug.Log(text);
		return solved;
	}
}
