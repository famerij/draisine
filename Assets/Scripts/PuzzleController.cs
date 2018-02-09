using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using UnityEditor;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
	public List<PuzzleBlock> Blocks = new List<PuzzleBlock>();

	private Transform _selectedTransform;
	private PuzzleBlock _selectedBlock;
	
	private void Start()
	{
		Blocks.ForEach(b => b.ActivatedSprites.ForEach(a => a.SetActive(false)));
		
		SelectTransform(Blocks[0].transform);
	}

	private void SelectTransform(Transform _transform)
	{
		// Disable previous block
		if (_selectedBlock != null)
			_selectedBlock.ActivatedSprites.ForEach(s => s.SetActive(false));
		
		_selectedTransform = _transform;
		_selectedBlock = Blocks.Find(block => block.transform == _selectedTransform);
		
		// Enable block
		if (_selectedBlock != null)
			_selectedBlock.ActivatedSprites.ForEach(s => s.SetActive(true));
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (_selectedBlock != null && Blocks.IndexOf(_selectedBlock) > 0)
			{
//				Debug.LogFormat("Selected block {0}", selectedBlock.name);
				SelectTransform(Blocks[Blocks.IndexOf(_selectedBlock) - 1].transform);
			}
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (_selectedBlock != null && Blocks.IndexOf(_selectedBlock) < Blocks.Count - 1)
			{
//				Debug.LogFormat("Selected block {0}", selectedBlock.name);
				SelectTransform(Blocks[Blocks.IndexOf(_selectedBlock) + 1].transform);
			}
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow) && _selectedBlock != null)
		{
			_selectedBlock.ChangeSprite(Direction.Left);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) && _selectedBlock != null)
		{
			_selectedBlock.ChangeSprite(Direction.Right);
		}
	}
}
