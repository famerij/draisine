using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
	public FadeFromToBlack FadeIn;

	private bool _fading;
	
	public void Update()
	{
		if (Input.anyKeyDown && !_fading)
		{
			_fading = true;
			FadeIn.FadeIn(() =>
			{
				SceneManager.LoadScene("Tutorial");
			});
		}
	}
}
