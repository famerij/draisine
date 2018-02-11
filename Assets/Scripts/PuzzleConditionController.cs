using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PuzzleConditionController : MonoBehaviour
{
	public List<Sprite> Sprites;
	public GameObject ConditionSpriteParent;
	public GameObject SolvedUI;
	public GameObject UnSolvedUI;

	private readonly List<Sprite> _currentPuzzle = new List<Sprite>();
	[SerializeField]
	private List<ConditionSprite> _conditionSprites;

	private bool _solved;

	private void Awake()
	{
		_conditionSprites = ConditionSpriteParent.GetComponentsInChildren<ConditionSprite>().ToList();
		_conditionSprites.Reverse();
	}
	
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
		StartCoroutine(ScrambleAnimation(() =>
		{
			while (scrambles < 50)
			{
				int i = Random.Range(0, indices.Length);
				int next = i == indices.Length - 1 ? 0 : i + 1;
				int nextValue = indices[next];
				indices[next] = indices[i];
				indices[i] = nextValue;
				scrambles++;
			}
			
			for (int i = 0; i < indices.Length; i++)
			{
				var index = indices[i];
				var randomSpriteIndex = Random.Range(0, blocks[index].SymbolSprites.Count);
				var sprite = blocks[index].SymbolSprites[randomSpriteIndex];
				_currentPuzzle.Add(sprite);
			}
		
			UpdateUI();

			_solved = false;
		}));
	}

	private IEnumerator ScrambleAnimation(Action onDone)
	{
		float timer = 0f;

		while (timer < 1f)
		{
			timer += Time.deltaTime;
			for (int i = 0; i < _conditionSprites.Count; i++)
			{
				_conditionSprites[i].ValidationImage.color = Color.red;
				_conditionSprites[i].SymbolImage.sprite = Sprites[Random.Range(0, Sprites.Count)];
			}
			
			yield return new WaitForEndOfFrame();
		}
		
		
		onDone();
	}

	private void UpdateUI()
	{
		for (int i = 0; i < _conditionSprites.Count; i++)
		{
			if (i >= _currentPuzzle.Count)
				_conditionSprites[i].enabled = false;
			else
			{
				_conditionSprites[i].enabled = true;
				_conditionSprites[i].SymbolImage.sprite = _currentPuzzle[i];
			}
		}
	}

	public bool CheckSolution(List<Sprite> solution)
	{
		if (_currentPuzzle.Count < solution.Count) return false;
		
		bool solved = true;
		string text = "Checking Solution\n";
		for (int i = 0; i < solution.Count; i++)
		{
			text += "Try: " + solution[i].name + "\n";
			text += "Solution: " + _currentPuzzle[i].name + "\n";
			if (solution[i] != _currentPuzzle[i])
			{
				solved = false;
				_conditionSprites[i].ValidationImage.color = Color.red;
			}
			else
			{
				_conditionSprites[i].ValidationImage.color = Color.green;
			}
		}
		
		SolvedUI.SetActive(solved);
		UnSolvedUI.SetActive(!solved);

		if (solved && !_solved)
		{
			GameController.PuzzleSolved();
			_solved = true;
		}
		
		return solved;
	}
}
