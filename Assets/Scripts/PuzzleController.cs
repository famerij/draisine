using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
	public Transform Selector;
	
	public List<PuzzleBlock> BlockPrefabs;
	public List<Transform> BlockPlaceholders;
	
	private readonly List<PuzzleBlock> _blocks = new List<PuzzleBlock>();

	private Transform _selectedTransform;
	private PuzzleBlock _selectedBlock;
	
	private void Start()
	{
		InitBlocks();
		SelectTransform(_blocks[0].transform);
	}

	private void InitBlocks()
	{
		for (int i = 0; i < BlockPlaceholders.Count; i++)
		{
			var blockObj = Instantiate(BlockPrefabs[Random.Range(0, BlockPrefabs.Count)], transform);
			var puzzleBlock = blockObj.GetComponent<PuzzleBlock>();
			blockObj.transform.position = BlockPlaceholders[i].transform.position;
			_blocks.Add(puzzleBlock);
			BlockPlaceholders[i].gameObject.SetActive(false);
		}
	}

	private void SelectTransform(Transform _transform)
	{
//		Debug.LogFormat("Selecting transform {0}", transform.name);
		_selectedTransform = _transform;
		_selectedBlock = _blocks.Find(block => block.transform == _selectedTransform);
		Selector.position = _selectedTransform.position;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			for (int i = 0; i < _blocks.Count; i++)
			{
				var blockObj = Instantiate(BlockPrefabs[Random.Range(0, BlockPrefabs.Count)], transform);
				var puzzleBlock = blockObj.GetComponent<PuzzleBlock>();
				blockObj.transform.position = _blocks[i].transform.position;
				Destroy(_blocks[i].gameObject);
				_blocks[i] = puzzleBlock;
			}
		}

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (_selectedBlock != null && _blocks.IndexOf(_selectedBlock) > 0)
			{
//				Debug.LogFormat("Selected block {0}", selectedBlock.name);
				SelectTransform(_blocks[_blocks.IndexOf(_selectedBlock) - 1].transform);
			}
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (_selectedBlock != null && _blocks.IndexOf(_selectedBlock) < _blocks.Count - 1)
			{
//				Debug.LogFormat("Selected block {0}", selectedBlock.name);
				SelectTransform(_blocks[_blocks.IndexOf(_selectedBlock) + 1].transform);
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
