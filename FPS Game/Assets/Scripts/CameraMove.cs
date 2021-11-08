using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform camPos;
	public Transform orientation;
	[SerializeField] Wallrun wallrun;
	[SerializeField] PlayerCamera camScript;
	[SerializeField] Camera cam;

	public bool cutscene = true;
	[SerializeField] float camSpeed = 15f;

	// Update is called once per frame
    void Update()
    {	
		if(cutscene)
		{
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + Time.deltaTime * camSpeed ,transform.rotation.eulerAngles.z);
			
			if(Input.GetKey(KeyCode.Space))
			{
				cutscene = false;
				transform.position = camPos.position;
				transform.rotation = Quaternion.Euler(Vector3.zero);
				cam.transform.localPosition = Vector3.zero;
				cam.transform.rotation = Quaternion.Euler(Vector3.zero);
				camScript.ResetRotation();
			}
		}
		else{		
			transform.position = camPos.position;
			transform.rotation = Quaternion.Euler(camPos.rotation.eulerAngles.x, orientation.rotation.eulerAngles.y, wallrun.tilt);
		
			cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, camScript.fov, 10 * Time.deltaTime);
		}
	}
}
