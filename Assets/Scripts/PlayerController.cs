using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	//按钮系列
	private static readonly string JumpButton = "PGJump";
	private static readonly string KickButton = "PGKick";
	private static readonly string VerticalButton = "PGVertical";
	private static readonly string HorizontalButton = "PGHorizontal";
	private static readonly string BoxingButton = "PGBoxing";
	private static readonly string SwordButton = "PGSword";
	//动画系列
	//private static readonly string SwordButton = "PGSword";
	//动画参数
	private static readonly string JumpPrm = "Jump";
	private static readonly string SpeedPrm = "Speed";
	private static readonly string BoxingPrm = "Boxing";
	private static readonly string KickPrm = "Kick";
	private static readonly string SitPrm = "Sit";
	private static readonly string JumpWallPrm = "JumpWall";
	private static readonly string SwordPrm = "Sword";
	private static readonly string GroundedPrm = "Grounded";
	private static readonly string GroundedSwordPrm = "GroundedSword";

	[HideInInspector]
	public bool facingRight = true;  //是否面朝右边
	[HideInInspector]
	public bool jump = false;  //能否跳跃
	[HideInInspector]
	public bool kick = false;  //踢腿
	[HideInInspector]
	public bool sit = false;  //下蹲
	[HideInInspector]
	public bool boxing = false;  //打拳
	[HideInInspector]
	public bool sword = false;  //挥剑

	public float moveForce = 365f;  //移动给的力
	public float maxSpeed = 2f;  //最大移动速度
	public AudioClip[] jumpClips;  //跳跃时候的声音
	public float jumpForce = 1000f;  //跳跃时候的力度

	private Transform groundCheck;  //玩家着地检测
	private float groundCheckVariable = 0.16f;  //着地检测范围变量
	private bool grounded = false;  //是否着地
	private int groundCount;
	private Transform wallCheck;  //贴墙检测
	private float wallCheckVariable = 0.225f;  //贴墙检测范围变量
	private bool wallTouched = false;  //是否贴墙
	private int wallCount;

	SpriteRenderer sr;

	//animator相关
	private AnimatorStateInfo stateInfo;
	private Animator anim;  //引用角色动画

	//连招相关参数
	private float timeSinceLastPress = 0f;
	private bool isKeepPress = false;
	private int curSkill = 1;
	private float pressInterval = 0.4f;
	
	void Awake()
	{
		groundCheck = transform.Find("groundCheck");
		wallCheck = transform.Find ("wallCheck");
		anim = GetComponent<Animator>();
		BoxCollider2D box = GetComponent<BoxCollider2D>();
//		box.size = new Vector2 (0.3f,1);
		sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		groundCount = 0;
		wallCount = 0;
		//print ("size:"+sr.sprite.bounds.size);
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		//跳
		for (int i = -1; i < 2; i++)
		{
			groundCheck.localPosition = new Vector3(groundCheckVariable * i,groundCheck.localPosition.y,groundCheck.position.z);
			grounded = Physics2D.Linecast(transform.position,groundCheck.position,1 << LayerMask.NameToLayer("Ground"));

			wallCheck.localPosition = new Vector3(wallCheck.localPosition.x,wallCheckVariable * i,wallCheck.localPosition.z);
			wallTouched = Physics2D.Linecast (transform.position, wallCheck.position, 1 << LayerMask.NameToLayer ("Ground"));

			if (!grounded)
				groundCount += i + 2;
			if (!wallTouched) 
				wallCount += i + 2;
		}

		if (groundCount == 6) {
			anim.SetBool (GroundedPrm, false);
			grounded = false;
		} 
		else
		{
			anim.SetBool (GroundedPrm, true);
			grounded = true;
		}

		if (wallCount == 6) {
			anim.SetBool (JumpWallPrm, false);
			wallTouched = false;
		} 
		else
		{
			anim.SetBool (JumpWallPrm, true);
			wallTouched = true;
		}

//		wallTouched = Physics2D.Linecast (transform.position, wallCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
//		if (wallTouched)
//			anim.SetBool (JumpWallPrm, true);
//		else
//			anim.SetBool (JumpWallPrm, false);

//		print ("ground:" + grounded);
//		print ("wall:" + wallTouched);
		if (Input.GetButtonDown(JumpButton) && 
		    (grounded || (stateInfo.IsName("PGClimbWallEnd") || stateInfo.IsName ("PGClimbWall"))
		 )
		    ) 
		{
			//print("jumpbutton"+ Time.time);
			jump = true;
			anim.SetBool(JumpWallPrm,false);
			//print(anim.GetLayerName(0));
		}
		anim.SetFloat ("JumpWallVelocity", Mathf.Abs (rigidbody2D.velocity.x));
		//踢腿
		if (Input.GetButtonDown (KickButton))
		{
			kick = true;
		}
		if (anim.GetInteger (KickPrm) != 0)
			anim.SetInteger (KickPrm, 0);
		//下蹲
		if (Input.GetAxis (VerticalButton) < 0 && Input.GetButton (VerticalButton)) 
		{
			sit = true;
		}
		if (Input.GetButtonUp (VerticalButton)) 
		{
			sit = false;
		}
		//打拳
		if (Input.GetButtonDown (BoxingButton))
		{
			boxing = true;
		}
		if (anim.GetInteger (BoxingPrm) != 0)
			anim.SetInteger (BoxingPrm, 0);
		//挥剑
		if (Input.GetButtonDown (SwordButton))
						sword = true;	
		if (anim.GetInteger (SwordPrm) != 0)
			anim.SetInteger (SwordPrm, 0);
		if (anim.GetInteger (GroundedSwordPrm) != 0)
			anim.SetInteger (GroundedSwordPrm, 0);
		//除了跳和正常跑，其余速度降低
//		if (getButton("PGHorizontal") && !stateInfo.IsName ("PGRun") && !stateInfo.IsName("PGJump"))
//			maxSpeed = 0.1f;
//		if (stateInfo.IsName("PGRun"))
//			maxSpeed = 2f;

		if (stateInfo.IsName ("PGRunPunch") || stateInfo.IsName ("PGBackKick") || stateInfo.IsName ("PGBoxing") || stateInfo.IsName ("PGKeepBoxing")
		    || stateInfo.IsName ("PGAttackKick") || stateInfo.IsName ("PGKeepKick") || stateInfo.IsTag("150") || stateInfo.IsName("PGSweep")
		    ) 
			maxSpeed = 0.1f;
		else 
			maxSpeed = 2f;

		//左右跑动
		float h = Input.GetAxis (HorizontalButton);
		anim.SetFloat (SpeedPrm, Mathf.Abs (h));

		if (grounded || stateInfo.IsName ("PGClimbWallEnd") || stateInfo.IsName ("PGClimbWall") || (!grounded && !wallTouched)) {
						if (h * rigidbody2D.velocity.x < maxSpeed)
								rigidbody2D.AddForce (Vector2.right * h * moveForce);
				}
		if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)
			rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();
	}

	void FixedUpdate()
	{
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		if (jump)
		{
			anim.SetTrigger(JumpPrm);
			//			anim.Play("PGRedAttackKick");
			rigidbody2D.AddForce(new Vector2(0f,jumpForce));
			jump = false;
		}
		if (kick)
		{	
//			timeSinceLastKick = Time.time - timeSinceLastKick;
//			Debug.Log("okok:"+timeSinceLastKick);
//			if (timeSinceLastKick < 0.5 && timeSinceLastKick > 0)
//			{
//				isKickKeepPress = true;
//			}
//			if (isKickKeepPress)
//			{
//				anim.SetInteger(KickPrm,2);
//				isKickKeepPress = false;
//			}
//			else
//			{
//				anim.SetInteger(KickPrm,1);
//			}
//			timeSinceLastKick = Time.time;
			isKeepPressing(KickPrm,2);
			kick = false;
		}
		if (sit) 
		{
			anim.SetBool(SitPrm,true);
		}
		else
		{
			anim.SetBool(SitPrm,false);
		}
		if (boxing)
		{	
			isKeepPressing(BoxingPrm,2);
			boxing = false;
		}
		if (sword)
		{
			if (!sit)
				isKeepPressing(SwordPrm,6);
			else
				isKeepPressing(GroundedSwordPrm,2);
			sword = false;
		}
	}

	void isKeepPressing(string animName,int skillCount)
	{
		timeSinceLastPress = Time.time - timeSinceLastPress;
		if (timeSinceLastPress < pressInterval && timeSinceLastPress > 0)
		{
			isKeepPress = true;
		}
		if (isKeepPress)
		{
			if (stateInfo.normalizedTime > 0.9 && !stateInfo.IsName("PGIdle"))
			{
			if (curSkill < skillCount)
				curSkill++;
			else
				curSkill = 1;
			anim.SetInteger(animName,curSkill);
			}
			else if (stateInfo.IsName("PGIdle"))
			{
				if (curSkill < skillCount)
					curSkill++;
				else
					curSkill = 1;
				anim.SetInteger(animName,curSkill);
			}
			isKeepPress = false;
		}
		else
		{
			anim.SetInteger(animName,1);
			curSkill = 1;
		}
		timeSinceLastPress = Time.time;
		boxing = false;
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
