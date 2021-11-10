using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallrun : MonoBehaviour
{
    [SerializeField] Transform orientation;
	
	[Header("Camera")]
	[SerializeField] float cameraTilt;
	[SerializeField] float cameraTiltTime;
	
	public float tilt { get; private set; }
	
	[Header("Wall Running")]
	[SerializeField] float wallDist = 0.6f;
	[SerializeField] float minJumpHeight = 0f;
	[SerializeField] float wallrunGravity = 0f;
	[SerializeField] float walljumpForce = 100f;

    public bool wallLeft = false;
	public bool wallRight = false;
	
	public bool wallrunning;
	
	RaycastHit leftHit;
	RaycastHit rightHit;
	
	Rigidbody rb;
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
	
	public bool CanWallRun()
	{
		return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight);
	}
	
	void CheckWall()
	{
		//rotating to get right
		Vector3 planeVel = new Vector3(rb.velocity.z, 0, -rb.velocity.x);
		
		wallLeft = Physics.Raycast(transform.position, -planeVel, out leftHit, wallDist);
		wallRight = Physics.Raycast(transform.position, planeVel, out rightHit, wallDist);
	}
	
	void Update()
    {
        CheckWall();
		
		if(CanWallRun() && rb.velocity.magnitude > 6)
		{
			if(wallLeft)
			{
				StartWallrun();
				wallrunning = true;
			}
			else if(wallRight)
			{
				StartWallrun();
				wallrunning = true;
			}
			else
			{
				StopWallrun();
				wallrunning = false;
			}
		}
		else
		{
			StopWallrun();
		}
		
		Debug.Log(wallrunning);
    }
	
	void StartWallrun()
	{
		rb.useGravity = false;
		rb.AddForce(Vector3.down * wallrunGravity, ForceMode.Acceleration);
		
		if(wallLeft)
		{
			tilt = Mathf.Lerp(tilt, -cameraTilt, cameraTiltTime * Time.deltaTime);
			rb.velocity -= leftHit.normal * Vector3.Dot(rb.velocity, leftHit.normal);
		}
		else if(wallRight)
		{		
			tilt = Mathf.Lerp(tilt, cameraTilt, cameraTiltTime * Time.deltaTime);
			rb.velocity -= rightHit.normal * Vector3.Dot(rb.velocity, rightHit.normal);
		}
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(wallLeft)
			{
				Vector3 walljumpDir = transform.up + leftHit.normal * 0.3f;
				rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
				rb.AddForce(walljumpDir.normalized * walljumpForce, ForceMode.Force);
			}
			
			if(wallRight)
			{
				Vector3 walljumpDir = transform.up + rightHit.normal * 0.3f;
				rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
				rb.AddForce(walljumpDir.normalized * walljumpForce, ForceMode.Force);
			}
		}
	}
	
	void StopWallrun()
	{
		rb.useGravity = true;
		tilt = Mathf.Lerp(tilt, 0, cameraTiltTime * Time.deltaTime);
	}
}