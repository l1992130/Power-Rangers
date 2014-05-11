using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour 
{
	public enum BossState {IDLE,WALK,JUMP,ATTACK,HURT,DIE,SHOCKWAVE,IMPACT};
	public float addForce = 25;
	public float minDistance = 5f;
	public float shockWaveDistance = 3f;
	public float impactDistance = 1.5f;
	public float maxSpeed = 1f;
	public float changeProbability = 50f;
	public bool facingRight;
	public BossState bossState;

	private Transform player;
	private Animator anim;
	private AnimatorStateInfo stateInfo;
	private float timeSinceLastShockWave = 0f;
	// Use this for initialization
	void Start () 
	{
	
	}

	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("player").transform;
		anim = GetComponent<Animator>();
		bossState = BossState.IDLE;
	}

	// Update is called once per frame
	void Update () 
	{
		print("bossState:"+bossState);
//		if (!anim.GetBool ("Walk"))
//						print ("walkfalse");
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		facingRight = Mathf.Sign (player.transform.position.x - transform.position.x) > 0 ? true : false;
		Flip (facingRight);

		float dist = Vector3.Distance (player.transform.position, transform.position);
		//print ("distance:"+dist);
		if (dist < minDistance) 
		{
//			if (bossState == BossState.IDLE || (bossState != BossState.SHOCKWAVE && bossState != BossState.IMPACT))
//			{
//				print("walkTrue");
//				anim.SetBool("Walk",true);
//			}
			//print("enterMinDistance");
			if (dist > shockWaveDistance)
			{
				shockWave(1);
			}
			else if (dist > impactDistance)
			{
				impact(1.2f);
			}
			else
			{
				bossState = BossState.WALK;
			}

		}
		else
		{
			bossState = BossState.IDLE;
		}

		switch (bossState)
		{
		case BossState.WALK :
			if (!anim.GetBool ("Walk"))
			anim.SetBool ("Walk", true);
			rigidbody2D.AddForce (new Vector2 (addForce * Mathf.Sign (player.transform.position.x - transform.position.x), 0));
			if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)				
				rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
			print("walk:"+rigidbody2D.velocity.x+"force:"+Mathf.Sign (player.transform.position.x - transform.position.x));
			break;
		case BossState.SHOCKWAVE :
			//print("shockwave");
			if (anim.GetBool("Walk"))
				anim.SetBool ("Walk", false);
			anim.SetTrigger ("ShockWave");

			break;
		case BossState.IMPACT :
			//print("impact");
			if (anim.GetBool("Walk"))
				anim.SetBool ("Walk", false);
			anim.SetTrigger ("Impact");

			break;
		case BossState.IDLE :
			//print("idle");
			anim.SetBool("Walk",false);
			anim.SetTrigger("Idle");
			break;
		}
	}

	void shockWave(float waitTime) 
	{
		timeSinceLastShockWave += Time.deltaTime;
		if (timeSinceLastShockWave >= waitTime)
		{
			print("time:"+timeSinceLastShockWave);
			float random =  Random.Range(1,100);
			if (random > changeProbability)
				bossState = BossState.SHOCKWAVE;
			else
				bossState = BossState.WALK;
			timeSinceLastShockWave = 0;
		}
	}
	void impact (float waitTime)
	{
		timeSinceLastShockWave += Time.deltaTime;
		if (timeSinceLastShockWave >= waitTime)
		{
			if (stateInfo.IsName ("GlutoImpactEnd"))
				return;
			print("time:"+timeSinceLastShockWave);
			float random =  Random.Range(1,100);
			if (random > changeProbability)
				bossState = BossState.IMPACT;
			else
				bossState = BossState.WALK;
//			if (stateInfo.IsName("GlutoWalk"))
//			bossState = BossState.IMPACT;
//			else
//				bossState = BossState.WALK;
			timeSinceLastShockWave = 0;
		}
	}

	void Flip(bool faceRight)
	{
		Vector3 theScale = transform.localScale;
		if (faceRight)
			theScale.x = -1;
		else
			theScale.x = 1;
		transform.localScale = theScale;
	}
}
