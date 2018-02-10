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
	public List<PuzzleBlock> Blocks = new List<PuzzleBlock>();

	private Transform _activeTransform;
	private PuzzleBlock _activeBlock;
	private PuzzleBlock _selectedBlock;
	private bool _selection;
	
	private void Start()
	{
		Blocks.ForEach(b => b.ActivatedSprites.ForEach(a => a.SetActive(false)));
		
		ActivateTransform(Blocks[0].transform);
	}

	private void ActivateTransform(Transform _transform)
	{
		if (_selection && _activeBlock != null)
		{
			Vector3 currentPos = _activeTransform.position;
			_activeTransform.position = _transform.position;
			_transform.position = currentPos;
			
			// Rearrange list
			Blocks.Sort((b1, b2) => b1.transform.position.y.CompareTo(b2.transform.position.y));
			
			SpreadBlocks();
			
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

	private void SpreadBlocks()
	{
		float yPos = Base.transform.position.y + Base.bounds.extents.y;
		for (int i = 0; i < Blocks.Count; i++)
		{
			Blocks[i].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
			var blockBounds = Blocks[i].GetComponent<Collider2D>().bounds;
			float y = SpreadHeight + yPos + blockBounds.extents.y;
			Blocks[i].transform.MoveTo(Blocks[i].transform.position, new Vector3(Blocks[i].transform.position.x, y), .3f,
				EasingTypes.BackOut);
			yPos = y + blockBounds.extents.y;
		}
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
		
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (_activeBlock != null && Blocks.IndexOf(_activeBlock) > 0)
			{
//				Debug.LogFormat("Selected block {0}", selectedBlock.name);
				ActivateTransform(Blocks[Blocks.IndexOf(_activeBlock) - 1].transform);
			}
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow))
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
	}
}
