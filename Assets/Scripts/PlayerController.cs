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
	public bool boxing = false;
	public float moveForce = 365f;  //移动给的力
	public float maxSpeed = 2f;  //最大移动速度
	public AudioClip[] jumpClips; //跳跃时候的声音
	public float jumpForce = 1000f; //跳跃时候的力度

	private Transform groundCheck; //玩家着地位置
	private bool grounded = false; //是否着地
	private Animator anim; //引用角色动画

	//腿部攻击相关参数
	private float timeSinceLastKick = 0f;
	private bool isKickKeepPress = false;

	//下蹲系列动作相关

	private AnimatorStateInfo stateInfo;

	//手部攻击相关参数
	private float timeSinceLastBoxing = 0f;
	private bool isBoxingKeepPress = false;
	// Use this for initialization
	void Start () 
	{
	
	}

	void Awake()
	{
		groundCheck = transform.Find("groundCheck");

		anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		//跳
		grounded = Physics2D.Linecast(transform.position,groundCheck.position,1 << LayerMask.NameToLayer("Ground"));
		if (Input.GetButtonDown("PGJump") && grounded) 
		{
			print("jumpbutton"+ Time.time);
			jump = true;
			print(anim.GetLayerName(0));
		}
		//踢腿
		if (Input.GetButtonDown ("PGKick"))
		{
			kick = true;
		}
		if (anim.GetInteger ("Kick") != 0)
			anim.SetInteger ("Kick", 0);
		//下蹲
		if (Input.GetAxis ("PGVertical") < 0 && Input.GetButton ("PGVertical")) 
		{
			sit = true;
		}
		if (Input.GetButtonUp ("PGVertical")) 
		{
			sit = false;
		}
		//打拳
		if (Input.GetButtonDown ("PGBoxing"))
		{
			boxing = true;
		}
		if (anim.GetInteger ("Boxing") != 0)
			anim.SetInteger ("Boxing", 0);

		//除了跳和正常跑，其余速度降低
		if (getButton("PGHorizontal") && !stateInfo.IsName ("PGRedRun") && !stateInfo.IsName("PGRedJump"))
			maxSpeed = 0.1f;
		if (stateInfo.IsName("PGRedRun"))
			maxSpeed = 2f;

		//左右跑动
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
	}

	void FixedUpdate()
	{
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
				isKickKeepPress = true;
			}
			if (isKickKeepPress)
			{
				anim.SetInteger("Kick",2);
				isKickKeepPress = false;
			}
			else
			{
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
		if (boxing)
		{	
			timeSinceLastBoxing = Time.time - timeSinceLastBoxing;
			Debug.Log("okok:"+timeSinceLastBoxing);
			if (timeSinceLastBoxing < 0.5 && timeSinceLastBoxing > 0)
			{
				isBoxingKeepPress = true;
			}
			if (isBoxingKeepPress)
			{
				anim.SetInteger("Boxing",2);
				isBoxingKeepPress = false;
			}
			else
			{
				anim.SetInteger("Boxing",1);
			}
			timeSinceLastBoxing = Time.time;
			boxing = false;
		}
//		if (boxing)
//		{
//			anim.SetInteger("Boxing",1);
//			boxing = false;
//		}
//		else
//			anim.SetInteger("Boxing",0);
	}

	void Flip()
	{
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	bool getButton(string buttonName)
	{
		return Input.GetButton (buttonName) || Input.GetButtonDown (buttonName);
	}
}
