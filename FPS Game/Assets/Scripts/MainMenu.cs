using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel(int index)
	{
		SceneManager.LoadScene("Level" + index.ToString());
	}
	
	public void ExitGame()
	{
		Debug.Log("Exit");
		Application.Quit();
	}
}
