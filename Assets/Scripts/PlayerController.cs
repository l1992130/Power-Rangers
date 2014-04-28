using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	[HideInInspector]
	public bool facingRight = true;  //是否面朝右边
	[HideInInspector]
	public bool jump = false;  //能否条约
	public bool kick = false;
	public bool sit = false;
	public float moveForce = 365f;  //移动给的力
	public float maxSpeed = 5f;  //最大移动速度
	public AudioClip[] jumpClips; //跳跃时候的声音
	public float jumpForce = 1000f; //跳跃时候的力度

	private Transform groundCheck; //玩家着地位置
	private bool grounded = false; //是否着地
	private Animator anim; //引用角色动画

	private float timeSinceLastKick = 0f;
	private bool isKeepPress = false;

	private AnimatorStateInfo stateInfo;

	// Use this for initialization
	void Start () 
	{
	
	}

	void Awake()
	{
		groundCheck = transform.Find("groundCheck");

		anim = GetComponent<Animator>();
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);
	}
	
	// Update is called once per frame
	void Update () 
	{
//		print("position:"+transform.position);
//		print("groundCheck:"+groundCheck.position);
		grounded = Physics2D.Linecast(transform.position,groundCheck.position,1 << LayerMask.NameToLayer("Ground"));
//		print ("ground" + grounded);
		if (Input.GetButtonDown ("PGJump") && grounded)
					jump = true;
		if (Input.GetButtonDown ("PGKick"))// && grounded)
		{
			kick = true;
		}
		if (Input.GetKey(KeyCode.S))
			sit = true;
		if (Input.GetKeyUp (KeyCode.S))
						sit = false;
		print ("Sit:"+sit);
		if (anim.GetInteger ("Kick") != 0)
			anim.SetInteger ("Kick", 0);

		float h = Input.GetAxis ("PGHorizontal");
		anim.SetFloat ("Speed", Mathf.Abs (h));

		if (h * rigidbody2D.velocity.x < maxSpeed)
			rigidbody2D.AddForce (Vector2.right * h * moveForce);
		if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
			rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();
		if (jump)
		{
			anim.SetTrigger("Jump");
//			anim.Play("PGRedAttackKick");
			rigidbody2D.AddForce(new Vector2(0f,jumpForce));
			jump = false;
		}
		if (kick)
		{	
			timeSinceLastKick = Time.time - timeSinceLastKick;
			Debug.Log("okok:"+timeSinceLastKick);
			if (timeSinceLastKick < 0.5 && timeSinceLastKick > 0)
			{
				isKeepPress = true;
			}
			if (isKeepPress)
			{
				print("keepkick");
				anim.SetInteger("Kick",2);
				isKeepPress = false;
			}
			else
			{
				print("kick");
				anim.SetInteger("Kick",1);
			}
			timeSinceLastKick = Time.time;
			kick = false;
		}
		if (sit) 
		{
			anim.SetBool("Sit",true);
		}
		else
		{
			anim.SetBool("Sit",false);
		}
	}
	
	void Flip()
	{
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
