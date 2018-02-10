using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using SimpleEasing;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class PuzzleController : MonoBehaviour
{
	public Collider2D Base;
	public float SpreadHeight = .5f;
	public float SwapDuration = .2f;
	public float SpreadDuration = .2f;
	public List<PuzzleBlock> Blocks = new List<PuzzleBlock>();
	public PuzzleConditionController ConditionController;

	private Transform _activeTransform;
	private PuzzleBlock _activeBlock;
	private PuzzleBlock _selectedBlock;
	private bool _selection;
	private bool _swapping;
	private float _timer;
	
	private void Start()
	{
		Blocks.ForEach(b =>
		{
			b.ActivatedSprites.ForEach(a => a.SetActive(false));
			b.transform.SetParent(transform, true);
		});
		
		ActivateTransform(Blocks[0].transform);
		
		if (ConditionController)
			ConditionController.CreateNewPuzzle(Blocks);
	}

	private void ActivateTransform(Transform _transform)
	{
		if (_selection && _activeBlock != null)
		{
			StartCoroutine(SwitchBlocks(_transform));
			
			return;
		}
		
		// Activation switch
		//
		// Disable previous block
		if (_activeBlock != null)
			_activeBlock.ActivatedSprites.ForEach(s => s.SetActive(false));
		
		_activeTransform = _transform;
		_activeBlock = Blocks.Find(block => block.transform == _activeTransform);
		
		// Enable block
		if (_activeBlock != null)
			_activeBlock.ActivatedSprites.ForEach(s => s.SetActive(true));
	}

	private IEnumerator SwitchBlocks(Transform _other)
	{
		Vector3 currentPos = _activeTransform.position;
//			_activeTransform.position = _transform.position;
//			_transform.position = currentPos;
		_activeTransform.MoveTo(_activeTransform.position, _other.position, SwapDuration, EasingTypes.BackOut);
		_other.MoveTo(_other.position, currentPos, SwapDuration, EasingTypes.BackOut);
		_swapping = true;
		
		yield return new WaitForSeconds(SwapDuration);
		
		// Rearrange list
		Blocks.Sort((b1, b2) => b1.transform.position.y.CompareTo(b2.transform.position.y));
		
		ValidatePuzzle();
		
		if (!_selection)
		{
			Blocks.ForEach(b => b.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic);
			_swapping = false;
			yield break;
		}
		
		SpreadBlocks();
	}

	private void SpreadBlocks()
	{
		float yPos = Base.transform.position.y + Base.bounds.extents.y;
		for (int i = 0; i < Blocks.Count; i++)
		{
			Blocks[i].transform.SetSiblingIndex(i);
			Blocks[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
			var blockBounds = Blocks[i].GetComponent<Collider2D>().bounds;
			float y = SpreadHeight + yPos + blockBounds.extents.y;
			Blocks[i].transform.MoveTo(Blocks[i].transform.position, new Vector3(Blocks[i].transform.position.x, y),
				SpreadDuration,EasingTypes.BackOut);
			yPos = y + blockBounds.extents.y;
		}

		_swapping = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (!_selection)
				SpreadBlocks();
			_selection = true;
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			Blocks.ForEach(b => b.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic);
			_selection = false;
		}
		
		if (Input.GetKeyDown(KeyCode.DownArrow) && !_swapping)
		{
			if (_activeBlock != null && Blocks.IndexOf(_activeBlock) > 0)
			{
//				Debug.LogFormat("Selected block {0}", selectedBlock.name);
				ActivateTransform(Blocks[Blocks.IndexOf(_activeBlock) - 1].transform);
			}
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow) && !_swapping)
		{
			if (_activeBlock != null && Blocks.IndexOf(_activeBlock) < Blocks.Count - 1)
			{
//				Debug.LogFormat("Selected block {0}", selectedBlock.name);
				ActivateTransform(Blocks[Blocks.IndexOf(_activeBlock) + 1].transform);
			}
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow) && _activeBlock != null)
		{
			_activeBlock.ChangeSprite(Direction.Left);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) && _activeBlock != null)
		{
			_activeBlock.ChangeSprite(Direction.Right);
		}
		else if (Input.GetKeyDown(KeyCode.Return))
		{
			if (ValidatePuzzle())
				ConditionController.CreateNewPuzzle(Blocks);
		}
		
		if (Input.anyKeyDown)
		{
			ValidatePuzzle();
		}

		if (_timer < 0f && !_selection && !_swapping)
		{
			for (int i = 0; i < Blocks.Count; i++)
			{
				Blocks[i].GetComponent<Rigidbody2D>().AddForce(Vector3.up, ForceMode2D.Impulse);
			}
			_timer = Random.Range(.8f, 1.2f);
		}

		_timer -= Time.deltaTime;
	}

	private bool ValidatePuzzle()
	{
		if (ConditionController == null) return false;
		
		var blockSprites = Blocks
			.Where(b => b.gameObject.activeInHierarchy)
			.Select(b => b.CurrentSprite)
			.ToList();
		var solved = ConditionController.CheckSolution(blockSprites);
		Debug.LogFormat("Puzzle solved? {0}", solved);

		return solved;
	}
}
