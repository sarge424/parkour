using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
	[SerializeField] GameObject spawn;
	[SerializeField] GameObject goal;
	[SerializeField] GameObject player;
	[SerializeField] PlayerMovement movement;
	[SerializeField] PlayerCamera cam;
	[SerializeField] CameraMove camMove;
	[SerializeField] TextMeshProUGUI timerTextPro;
	[SerializeField] GameObject pauseMenu;
	
	public float timer = 0;
	public bool finished = false;
	public bool paused = false;
	
    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = 144;
		
		player.transform.position = spawn.transform.position;
		cam.initRot = spawn.transform.rotation.eulerAngles.y;
	}

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
		}
		
		
		if(!finished)
			timer += Time.deltaTime;
			
		string timeText = "";
		
		if(timer < 0.05)
			timeText = "0.00";
		else
			timeText = timer.ToString("0.00");
		
		if(camMove.cutscene)
			timeText = "Get from the orange zone to the green zone as fast as possible.\nAlso the floor is lava.\nPress <space> to begin.";
		
		timerTextPro.SetText(timeText);
		
		if(string.Compare(movement.state, "goal") == 0){
			movement.state = "";
			finished = true;
		}
		else if(string.Compare(movement.state, "spawn") == 0){
			movement.state = "";
			finished = false;
			timer = 0.0f;
		}
		else if(string.Compare(movement.state, "lava") == 0){
			movement.state = "";
			finished = false;
			player.transform.position = spawn.transform.position;
			cam.ResetRotation();
			movement.rb.velocity = Vector3.zero;
		}
    }
	
	public void PauseGame()
	{
		Time.timeScale = 0f;
		pauseMenu.SetActive(true);
		
		camMove.disabled = true;
		
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
	
	public void ResumeGame()
	{
		pauseMenu.SetActive(false);
		Time.timeScale = 1f;
		
		camMove.disabled = false;
		
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}
