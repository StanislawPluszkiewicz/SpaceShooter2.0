namespace ESIEE_UNITY_ETS
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.SceneManagement;

	public class LoadNextScene : MonoBehaviour
	{
		void Start()
		{
			Time.timeScale = 1;
			StartCoroutine(LoadNextSceneMethod());
		}

		private IEnumerator LoadNextSceneMethod()
		{
			yield return new WaitForSeconds(2);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}

	}
}