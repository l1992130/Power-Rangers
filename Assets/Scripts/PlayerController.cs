using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	[HideInInspector]
	public bool facingRight = true;  //是否面朝右边
	[HideInInspector]
	public bool jump = false;  //能否条约
	public bool run = false;
	public float moveForce = 365f;  //移动给的力
	public float maxSpeed = 5f;  //最大移动速度
	public AudioClip[] jumpClips; //跳跃时候的声音
	public float jumpForce = 1000f; //跳跃时候的力度

	private Transform groundCheck; //玩家着地位置
	private bool grounded = false; //是否着地
	private Animator anim; //引用角色动画

	// Use this for initialization
	void Start () 
	{
	
	}

	void Awake()
	{
		//groundCheck = transform.Find("groundCheck");
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
//		grounded = Physics2D.Linecast(transform.position,groundCheck.position,1 << LayerMask.NameToLayer("Ground"));

		if (Input.GetButtonDown ("Jump"))// && grounded)
					jump = true;
		if (Input.GetButtonDown ("Fire1"))// && grounded)
			run = true;
	}

	void FixedUpdate()
	{
		float h = Input.GetAxis ("Horizontal");
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
			anim.SetTrigger("AttackKick");
//			anim.Play("PGRedAttackKick");
			rigidbody2D.AddForce(new Vector2(0f,jumpForce));
			jump = false;
		}
		if (run)
		{
			anim.SetTrigger("Run");
			//			anim.Play("PGRedAttackKick");
//			rigidbody2D.AddForce(new Vector2(0f,jumpForce));
			run = false;
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
