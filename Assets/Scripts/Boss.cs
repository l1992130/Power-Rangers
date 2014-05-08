using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour 
{
	public enum BossState {IDLE,WALK,JUMP,ATTACK,HURT,DIE};
	public float addForce = 25;
	public float minDistance = 5f;
	public float shockWaveDistance = 3f;
	public float maxSpeed = 1f;
	public float changeProbability = 75f;
	public bool facingRight;

	private Transform player;
	private Animator anim;
	// Use this for initialization
	void Start () 
	{
	
	}

	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("player").transform;
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () 
	{
		facingRight = Mathf.Sign (player.transform.position.x - transform.position.x) > 0 ? true : false;
		Flip (facingRight);

		print ("facing:"+facingRight);
		float dist = Vector3.Distance (player.transform.position, transform.position);
		print ("distance:"+dist);
		if (dist < minDistance) 
		{
			anim.SetBool ("ShockWave", true);
			//anim.SetBool ("Walk", true);
			rigidbody2D.AddForce (new Vector2 (addForce * Mathf.Sign (player.transform.position.x - transform.position.x), 0));
			if (Mathf.Abs (rigidbody2D.velocity.x) > maxSpeed)				
				rigidbody2D.velocity = new Vector2 (Mathf.Sign (rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);
		}
		else if (dist>shockWaveDistance)
		{
			float random =  Random.Range(1,100);
			if (random > 0)
			{
				anim.SetBool ("ShockWave", true);
			}
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
