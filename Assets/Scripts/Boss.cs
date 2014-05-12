using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour 
{
	public enum BossState {IDLE,WALK,JUMP,ATTACK,HURT,DIE,SHOCKWAVE,IMPACT,FAINT};
	public float addForce = 25;
	public float minDistance = 5f;
	public float shockWaveDistance = 3f;
	public float impactDistance = 1.5f;
	public float maxSpeed = 1f;
	public float shockwaveProbability = 50f;
	public float impactProbability = 60f;
	public float attackProbability = 60f;
	public float hurtProbability = 70f;
	public float FaintProbability = 85f;
	public bool facingRight;
	public BossState bossState;

	private Transform player;
	private Animator anim;
	private AnimatorStateInfo stateInfo;
	private float timeSinceLastShockWave = 0f;
	private float timeSinceLastImpact = 0f;
	private float timeSinceLastAttack = 0f;
	private float timeSinceLastHurt = 0f;
	private float timeSinceLastAction = 0f;
	// Use this for initialization
	void Start () 
	{
	
	}

	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("player").transform;
//		PlayerController player1 = GameObject.FindGameObjectWithTag ("player").GetComponent<PlayerController> ();
//		print (player1.HP);
		anim = GetComponent<Animator>();
		bossState = BossState.IDLE;
	}

	// Update is called once per frame
	void Update () 
	{
		//print("bossState:"+bossState);
//		if (!anim.GetBool ("Walk"))
//						print ("walkfalse");
		stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		facingRight = Mathf.Sign (player.transform.position.x - transform.position.x) > 0 ? true : false;
		Flip (facingRight);

		float dist = Vector3.Distance (player.transform.position, transform.position);
		print ("distance:"+dist);
		if (dist < minDistance) 
		{
//			if (bossState == BossState.IDLE || (bossState != BossState.SHOCKWAVE && bossState != BossState.IMPACT))
//			{
//				print("walkTrue");
//				anim.SetBool("Walk",true);
//			}
//			print("enterMinDistance");
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
				//print("attack");
				attack(1f);
			}

		}
		else
		{
			bossState = BossState.IDLE;
		}
		SWITCH (bossState);
	}

	void SWITCH(BossState state)
	{
		switch (state)
		{
		case BossState.WALK :
			if (!anim.GetBool ("Walk"))
				anim.SetBool ("Walk", true);
			//			rigidbody2D.AddForce (new Vector2 (addForce * Mathf.Sign (player.transform.position.x - transform.position.x), 0));
			//			if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)				
			//				rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
			//			print("walk:"+rigidbody2D.velocity.x+"force:"+Mathf.Sign (player.transform.position.x - transform.position.x));
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
		case BossState.ATTACK :
			//print("impact");
			if (anim.GetBool("Walk"))
				anim.SetBool ("Walk", false);
			anim.SetTrigger ("Attack");
			break;
		case BossState.IDLE :
			//print("idle");
			if (anim.GetBool("Walk"))
				anim.SetBool("Walk",false);
			anim.SetTrigger("Idle");
			break;
		case BossState.HURT :
			//print("idle");
			if (anim.GetBool("Walk"))
				anim.SetBool("Walk",false);
			anim.SetTrigger("Hurt");
			break;
		case BossState.FAINT :
			//print("idle");
			if (anim.GetBool("Walk"))
				anim.SetBool("Walk",false);
			anim.SetTrigger("Faint");
			break;
		case BossState.DIE :
			//print("idle");
			if (anim.GetBool("Walk"))
				anim.SetBool("Walk",false);
			anim.SetTrigger("Die");
			break;
		}
	}
	public int count =0;
	void OnCollisionEnter2D(Collision2D coll) 
	{
		timeSinceLastHurt = Time.time - timeSinceLastHurt;
		if (coll.gameObject.tag == "player" )
		{
			timeSinceLastHurt += Time.deltaTime;
			if (timeSinceLastHurt > 2f)
			{
				float random =  Random.Range(1,100);
				//print ("random:"+random);
				if (random > FaintProbability)
				{
					print("Hurt1");
					bossState = BossState.FAINT;
				}
				else if (random > hurtProbability)
				{
					print("Hurt");
					bossState = BossState.HURT;
				}
				timeSinceLastHurt = 0;
			}
		}
	}

	void shockWave(float waitTime) 
	{
		timeSinceLastShockWave += Time.deltaTime;
		if (timeSinceLastShockWave >= waitTime)
		{
			float random =  Random.Range(1,100);
			if (random > shockwaveProbability)
				bossState = BossState.SHOCKWAVE;
			else
				bossState = BossState.WALK;
			timeSinceLastShockWave = 0;
		}
	}
	void impact (float waitTime)
	{
		timeSinceLastImpact += Time.deltaTime;
		if (timeSinceLastImpact >= waitTime)
		{
//			if (stateInfo.normalizedTime < 0.5)
//				return;
			float random =  Random.Range(1,100);
			if (random > impactProbability)
				bossState = BossState.IMPACT;
			else
				bossState = BossState.WALK;
			timeSinceLastImpact = 0;
		}
	}

	void attack (float waitTime)
	{
		if (anim.GetBool("Walk"))
			anim.SetBool("Walk",false);
		timeSinceLastAttack += Time.deltaTime;
		if (timeSinceLastAttack >= waitTime)
		{
//			if (stateInfo.normalizedTime < 0.5)
//				return;
			float random =  Random.Range(1,100);
			if (random > attackProbability)
				bossState = BossState.ATTACK;
			else if (random > (100 - impactProbability))
				bossState = BossState.IMPACT;
			else
				bossState = BossState.WALK;
			timeSinceLastAttack = 0;
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
