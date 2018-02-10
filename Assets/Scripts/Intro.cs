using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
	public FadeFromToBlack FadeIn;
	
	private IEnumerator Start()
	{
		yield return new WaitForSeconds(6f);
		
		FadeIn.FadeIn(() =>
		{
			SceneManager.LoadScene("Tutorial");
		});
	}
}
