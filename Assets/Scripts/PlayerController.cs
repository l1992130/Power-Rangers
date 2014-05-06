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

	[HideInInspector]
	public bool facingRight = true;  //是否面朝右边
	[HideInInspector]
	public bool jump = false;  //能否条约
	public bool kick = false;
	public bool sit = false;
	public bool boxing = false;
	public bool sword = false;

	public float moveForce = 365f;  //移动给的力
	public float maxSpeed = 2f;  //最大移动速度
	public AudioClip[] jumpClips; //跳跃时候的声音
	public float jumpForce = 1000f; //跳跃时候的力度

	private Transform groundCheck; //玩家着地位置
	private float groundCheckVariable = 0.16f; //着地检测范围变量
	private bool grounded = false; //是否着地
	private Transform wallCheck;
	private bool wallTouched = false;
	private Animator anim; //引用角色动画
	SpriteRenderer sr;

	//腿部攻击相关参数
	private float timeSinceLastKick = 0f;
	private bool isKickKeepPress = false;

	//下蹲系列动作相关
	private AnimatorStateInfo stateInfo;

	//手部攻击相关参数
	private float timeSinceLastBoxing = 0f;
	private bool isBoxingKeepPress = false;

	private float timeSinceLastPress = 0f;
	private bool isKeepPress = false;
	private int curSkill = 1;

	// Use this for initialization
	void Start () 
	{
	
	}

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
		//print ("size:"+sr.sprite.bounds.size);
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		//跳
		for (int i = -1; i < 2; i++)
		{
			groundCheck.localPosition = new Vector3(groundCheckVariable * i,groundCheck.localPosition.y,groundCheck.position.z);
			grounded = Physics2D.Linecast(transform.position,groundCheck.position,1 << LayerMask.NameToLayer("Ground"));
			if (grounded)
			{
				anim.SetBool(GroundedPrm,true);
				print("iiiii:"+i);
				break;
			}
			else if( i == 1)
				anim.SetBool(GroundedPrm,false);
		}
		grounded = Physics2D.Linecast(transform.position,groundCheck.position,1 << LayerMask.NameToLayer("Ground"));
		wallTouched = Physics2D.Linecast (transform.position, wallCheck.position, 1 << LayerMask.NameToLayer ("Ground"));
		if (grounded)
			anim.SetBool(GroundedPrm,true);
		else
			anim.SetBool(GroundedPrm,false);
		if (wallTouched) {
			print("Walltrue");
						anim.SetBool (JumpWallPrm, true);
				}
				else {
						anim.SetBool (JumpWallPrm, false);
				}
		if (Input.GetButtonDown(JumpButton) && (grounded || (wallTouched && stateInfo.IsName("PGClimbWallEnd")))) 
		{
			print("jumpbutton"+ Time.time);
			jump = true;
			anim.SetBool(JumpWallPrm,false);
			print(anim.GetLayerName(0));
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
		//挥剑//
		if (Input.GetButtonDown (SwordButton))
						sword = true;	
		if (anim.GetInteger (SwordPrm) != 0)
			anim.SetInteger (SwordPrm, 0);
		//除了跳和正常跑，其余速度降低
//		if (getButton("PGHorizontal") && !stateInfo.IsName ("PGRun") && !stateInfo.IsName("PGJump"))
//			maxSpeed = 0.1f;
//		if (stateInfo.IsName("PGRun"))
//			maxSpeed = 2f;

		if (stateInfo.IsName ("PGRunPunch") || stateInfo.IsName ("PGBackKick") || stateInfo.IsName ("PGBoxing") || stateInfo.IsName ("PGKeepBoxing")
		    || stateInfo.IsName ("PGAttackKick") || stateInfo.IsName ("PGKeepKick")
		    ) 
			maxSpeed = 0.1f;
		else 
			maxSpeed = 2f;

		//左右跑动
		float h = Input.GetAxis (HorizontalButton);
		anim.SetFloat (SpeedPrm, Mathf.Abs (h));

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
			timeSinceLastKick = Time.time - timeSinceLastKick;
			Debug.Log("okok:"+timeSinceLastKick);
			if (timeSinceLastKick < 0.5 && timeSinceLastKick > 0)
			{
				isKickKeepPress = true;
			}
			if (isKickKeepPress)
			{
				anim.SetInteger(KickPrm,2);
				isKickKeepPress = false;
			}
			else
			{
				anim.SetInteger(KickPrm,1);
			}
			timeSinceLastKick = Time.time;
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
			isKeepPressing(SwordPrm,2);
			sword = false;
		}
	}

	void isKeepPressing(string animName,int skillCount)
	{
		timeSinceLastPress = Time.time - timeSinceLastPress;
		if (timeSinceLastPress < 0.5 && timeSinceLastPress > 0)
		{
			isKeepPress = true;
		}
		if (isKeepPress)
		{
			if(curSkill < skillCount)
				curSkill++;
			else
				curSkill = 1;
			anim.SetInteger(animName,curSkill);
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
