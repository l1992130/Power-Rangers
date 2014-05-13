using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	public int health = 1000;
	private Boss boss;
	private Transform bossTransform;
	private Animator bossAnim;
	private AnimatorStateInfo bossAnimState;
	private PlayerController playerController;
	private static int startHealth;
	// Use this for initialization
	void Start () {
		startHealth = health;
	}

	void Awake ()
	{
		playerController = GetComponent<PlayerController> ();
		bossAnim = GameObject.FindGameObjectWithTag ("Gluto").GetComponent<Animator>();
		bossTransform = GameObject.FindGameObjectWithTag ("Gluto").GetComponent<Transform>();
		boss = GameObject.FindGameObjectWithTag ("Gluto").GetComponent<Boss>();
	}

	// Update is called once per frame
	void Update () {

		//bossAnimState = bossAnim.GetCurrentAnimatorStateInfo (0);
	}
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (health < 0.3 * startHealth && playerController.playerState != PlayerController.PlayerState.NEEDBLOOD)
		{
			GetComponent<Animator>().SetTrigger("NeedBlood");
			playerController.playerState = PlayerController.PlayerState.NEEDBLOOD;
		}
//		if (health < 0 )
//		{
//			GetComponent<Animator>().SetTrigger("Die");
//		}
		if (coll.collider.tag == "Gluto")
		{
			print("collider");
			bossAnimState = bossAnim.GetCurrentAnimatorStateInfo (0);
			if (bossAnimState.IsName("GlutoImpact") && bossAnimState.normalizedTime > 0.65f)
			{
				//print("addforce");
				health -= Random.Range(30,120);
				rigidbody2D.AddForce(new Vector2(300 * Mathf.Sign(transform.position.x - boss.transform.position.x) ,0));
			}
			if (bossAnimState.IsName("GlutoAttack") && bossAnimState.normalizedTime > 0.7)
			{
				health -= Random.Range(30,100);
			}
		}
		if (coll.collider.tag == "glutoBullet")
			health -= Random.Range(40,60);

	}
}
