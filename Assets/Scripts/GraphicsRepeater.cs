using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphicsRepeater : MonoBehaviour
{
	public SpriteRenderer Graphics;

	public float YMin;
	public float YMax;
	public float SpacingMin;
	public float SpacingMax;
	public int MaxGraphicInstances = 50;
	public float DestroyGraphicsDistance = 50f;

	private readonly List<SpriteRenderer> _graphics = new List<SpriteRenderer>();
	private Vector3 _originalPosition;

	void Start()
	{
		_originalPosition = Graphics.transform.position;
		var newGraphics = Instantiate(Graphics.gameObject);
		newGraphics.transform.SetParent(transform, false);
		newGraphics.transform.position = Graphics.transform.position;
		_graphics.Add(newGraphics.GetComponent<SpriteRenderer>());
		Graphics.gameObject.SetActive(false);

		for (int i = 0; i < MaxGraphicInstances; i++)
		{
			AddGraphic();
		}
	}
	
	void Update()
	{
		if (_graphics.Last() != null && _graphics.Last().transform.position.x < DestroyGraphicsDistance)
		{
			AddGraphic();
		}
		
		if (_graphics[0].transform.position.x < -DestroyGraphicsDistance)
		{
			Destroy(_graphics[0].gameObject);
			_graphics.RemoveAt(0);
		}	
	}

	void AddGraphic()
	{
		var newGraphics = Instantiate(Graphics.gameObject);
		newGraphics.SetActive(true);
		newGraphics.transform.SetParent(transform, false);
		float x = _graphics.Last().transform.position.x + (_graphics.Last().bounds.size.x - .1f) + 
		          (Mathf.Approximately(SpacingMax, 0f) ? 0f : Random.Range(SpacingMin, SpacingMax));
		float y = _originalPosition.y + (Mathf.Approximately(YMax, 0f) ? 0f : Random.Range(YMin, YMax));
		newGraphics.transform.position = new Vector3(x, y);
		_graphics.Add(newGraphics.GetComponent<SpriteRenderer>());
	}
}
