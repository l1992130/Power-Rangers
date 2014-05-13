using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	private Boss boss;
	private Transform bossTransform;
	private Animator bossAnim;
	private AnimatorStateInfo bossAnimState;
	private PlayerController playerController;
	// Use this for initialization
	void Start () {
	
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
		bossAnimState = bossAnim.GetCurrentAnimatorStateInfo (0);
		if (coll.collider.tag == "Gluto")
		{
			if (bossAnimState.IsName("GlutoImpact") && bossAnimState.normalizedTime > 0.65f)
			{
				//print("addforce");

				rigidbody2D.AddForce(new Vector2(300 * Mathf.Sign(transform.position.x - boss.transform.position.x) ,0));
			}
		}

	}
}
