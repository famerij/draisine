using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
	public List<PuzzleBlock> BlockPrefabs;

	private readonly List<PuzzleBlock> _blocks = new List<PuzzleBlock>();
	private GameObject _blockParent;
	
	private void Start()
	{
		InitBlocks();
	}

	private void InitBlocks()
	{
		_blockParent = new GameObject("BlockParent");
		_blockParent.transform.SetParent(transform, false);
		for (int i = 0; i < 8; i++)
		{
			var blockObj = Instantiate(BlockPrefabs[Random.Range(0, BlockPrefabs.Count)], _blockParent.transform);
			var puzzleBlock = blockObj.GetComponent<PuzzleBlock>();
			var y = -i * puzzleBlock.Renderer.bounds.size.y;
			blockObj.transform.localPosition = new Vector3(0f, y);
			_blocks.Add(puzzleBlock);
		}
	}

	private void Update()
	{
		if (Input.anyKeyDown)
		{
			Destroy(_blockParent);
			_blocks.Clear();
			InitBlocks();
		}
	}
}
