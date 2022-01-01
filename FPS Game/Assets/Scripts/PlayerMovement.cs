using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("References")]
	[SerializeField] public Transform orientation;
	[SerializeField] Transform groundCheck;
	[SerializeField] LayerMask groundMask;
	
	[Header("Movement")]
	[SerializeField] float moveSpeed = 20f;
	[SerializeField] float maxSpeed = 10f;
	[SerializeField] float dragValue = 10f;
	
	[Header("Aerial")]
	[SerializeField] float jumpForce = 10f;
	[SerializeField] float airSpeed = 1f;
	[SerializeField] float maxAirSpeed = 15f;
	
	public Rigidbody rb;
	Vector3 inputVec;
	RaycastHit slopeHit;
	
	public bool isGrounded;
	bool isSliding;
	
	bool jumpPressed;
	bool pressingJump;
	
	bool noDrag;
	
	public string state;
	
	float oldYvel = 0f;
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		//get ukb input
		GetInput();
		
		//check if grounded
		isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
		
		//jump
		if(isGrounded && jumpPressed)
		{
			rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
			Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
			float upVel = rb.velocity.y;
			rb.velocity = orientation.transform.forward * flatVel.magnitude + upVel * Vector3.up;
		}
		
	}
	
	void FixedUpdate()
	{	
		Debug.Log(rb.velocity.y - oldYvel);
		oldYvel = rb.velocity.y;
	
		//set drag values
		if(inputVec.magnitude == 0 && isGrounded && !isSliding && !pressingJump)
			rb.drag = dragValue;
		else
			rb.drag = 0f;
		
		//stomp
		//if(!isGrounded && isSliding)
		//	rb.AddForce(-transform.up * stompForce, ForceMode.Impulse);
		
		//movement force
		if(isGrounded && !isSliding)
		{
			if(!OnSlope())
				rb.AddForce(inputVec * moveSpeed, ForceMode.Force);
			else
				rb.AddForce(Vector3.ProjectOnPlane(inputVec, slopeHit.normal) * moveSpeed, ForceMode.Force);				
		}
		//air movement
		else if(!isGrounded)
			rb.AddForce(inputVec * airSpeed, ForceMode.Force);
		
		
		//limit speed
		CapVelocity();
	}
	
	private void CapVelocity()
	{
		if(rb.velocity.magnitude > maxSpeed && isGrounded && !isSliding)
			rb.velocity = Vector3.Lerp(rb.velocity, rb.velocity.normalized * maxSpeed, 0.1f);
		if(rb.velocity.magnitude > maxAirSpeed && !isGrounded)
			rb.velocity = Vector3.Lerp(rb.velocity, rb.velocity.normalized * maxAirSpeed, 0.1f);
		
		if(rb.velocity.y > jumpForce)
		{
			rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
		}
	}
	
	private void GetInput()
	{
		float vertInput = Input.GetAxisRaw("Vertical");
		float horizInput = Input.GetAxisRaw("Horizontal");
		inputVec = orientation.transform.forward * vertInput + orientation.transform.right * horizInput;
		
		isSliding = Input.GetKey(KeyCode.LeftShift);
		jumpPressed = Input.GetKeyDown(KeyCode.Space);
		pressingJump = Input.GetKey(KeyCode.Space);
	}
	
	private bool OnSlope()
	{
		if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, 1.05f))
		{
			if(slopeHit.normal != Vector3.up)
				return true;
		}	
		return false;
	}
	
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Goal"))
		{
			state = "goal";
		}
		
		if(other.gameObject.layer == LayerMask.NameToLayer("Lava"))
		{
			state = "lava";
		}
	}
	
	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Spawn"))
		{
			state = "spawn";
		}
	}
}
