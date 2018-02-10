using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPuzzleController : MonoBehaviour
{
	public PuzzleController PuzzleController;

	public GameObject StartGameConditionSpriteParent;
	public GameObject CreditsConditionSpriteParent;

	public FadeFromToBlack FadeIn;
	
	[SerializeField]
	private List<ConditionSprite> _startGameConditionSprites;
	[SerializeField]
	private List<ConditionSprite> _creditsConditionSprites;
	
	[SerializeField]
	private List<Sprite> _startGamePuzzle;
	[SerializeField]
	private List<Sprite> _creditsPuzzle;

	private void Start()
	{
		_startGameConditionSprites = StartGameConditionSpriteParent.GetComponentsInChildren<ConditionSprite>().ToList();
		_startGameConditionSprites.Reverse();
		_startGamePuzzle = _startGameConditionSprites.Select(condition => condition.SymbolImage.sprite).ToList();
		
		_creditsConditionSprites = CreditsConditionSpriteParent.GetComponentsInChildren<ConditionSprite>().ToList();
		_creditsConditionSprites.Reverse();
		_creditsPuzzle = _creditsConditionSprites.Select(condition => condition.SymbolImage.sprite).ToList();
	}

	private void Update()
	{
		if (CheckSolution(PuzzleController.Blocks.Select(b => b.CurrentSprite)
			.ToList(), _startGamePuzzle, _startGameConditionSprites))
		{
			StartGame();
		}
		
		if (CheckSolution(PuzzleController.Blocks.Select(b => b.CurrentSprite)
			.ToList(), _creditsPuzzle, _creditsConditionSprites))
		{
			Credits();
		}
	}

	public void StartGame()
	{
		FadeIn.FadeIn(() =>
		{
			SceneManager.LoadScene("Game");
		});
	}

	public void Credits()
	{
		FadeIn.FadeIn(() =>
		{
			SceneManager.LoadScene("Credits");
		});
	}

	public bool CheckSolution(List<Sprite> solution, List<Sprite> puzzle, List<ConditionSprite> conditionSprites)
	{
		bool solved = true;
		for (int i = 0; i < solution.Count; i++)
		{
			if (solution[i] != puzzle[i])
			{
				solved = false;
				conditionSprites[i].ValidationImage.color = Color.red;
			}
			else
			{
				conditionSprites[i].ValidationImage.color = Color.green;
			}
		}
		
		return solved;
	}
}
