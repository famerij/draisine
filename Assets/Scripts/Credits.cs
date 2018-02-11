using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
	public AudioSource AudioSource;
	public AudioClip WhineClip;
	
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

	public void PlayWhine()
	{
		AudioSource.PlayOneShot(WhineClip);
	}
}
