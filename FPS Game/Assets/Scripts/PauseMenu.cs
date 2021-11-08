using UnityEngine.UI;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
	[SerializeField] CameraMove cm;
	
	public void PauseGame()
	{
		Time.timeScale = 0f;
		pauseMenu.SetActive(true);
		
		cm.disabled = true;
		
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
	
	public void ResumeGame()
	{
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
		
		cm.disabled = false;
		
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}
