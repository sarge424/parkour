using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
	float mouseX;
	float mouseY;
	
	float xRotation;
	float yRotation;
	
	float sensX = 1f;
	float sensY = 1f;
	float multiplier = 3f;
	
	[SerializeField] Transform cam;
	[SerializeField] Transform orientation;
	
	PlayerMovement movement;
	Wallrun wallrun;
	
	public float fov;
	public float initRot = 0f;
	
    // Start is called before the first frame update
    void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		
		movement = GetComponent<PlayerMovement>();
		wallrun = GetComponent<Wallrun>();
    }

    // Update is called once per frame
    void Update()
    {
		if(movement.isGrounded)
			fov = 100f;
		else
		{
			if(wallrun.wallrunning)
				fov = 110f;
			else
				fov = 105f;
		}
		
        getInput();
		
		cam.localRotation = Quaternion.Euler(xRotation, 0, 0);
		orientation.rotation = Quaternion.Euler(0, yRotation + initRot, 0);
    }
	
	private void getInput()
	{
		mouseX = Input.GetAxisRaw("Mouse X");
		mouseY = Input.GetAxisRaw("Mouse Y");
		
		yRotation += mouseX * sensX * multiplier;
		xRotation -= mouseY * sensY * multiplier;
		
		xRotation = Mathf.Clamp(xRotation, -90f, 90f);
	}
	
	public void ResetRotation()
	{
		xRotation = 0;
		yRotation = 0;
	}
}
