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

	
	public void CreateNewPuzzle(List<PuzzleBlock> blocks)
	{
		int length = blocks.Count;
		_currentPuzzle.Clear();
		var indices = new int[length];
		for (int i = 0; i < indices.Length; i++)
		{
			indices[i] = i;
		}
		
		// Scramble the indices
		int scrambles = 0;
		while (scrambles < 50)
		{
			int i = Random.Range(0, indices.Length);
			int next = i == indices.Length - 1 ? 0 : i + 1;
			int nextValue = indices[next];
			indices[next] = indices[i];
			indices[i] = nextValue;
			scrambles++;
		}

		string randoms = "Randoms:\n";
		for (int i = 0; i < indices.Length; i++)
		{
			randoms += indices[i] + "\n";
		}
		Debug.Log(randoms);
		
		for (int i = 0; i < indices.Length; i++)
		{
			var index = indices[i];
			var randomSpriteIndex = Random.Range(0, blocks[index].SymbolSprites.Count);
			var sprite = blocks[index].SymbolSprites[randomSpriteIndex];
			_currentPuzzle.Add(sprite);
		}
		
		UpdateUI();
	}

	private void Swap()
	{
		
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
