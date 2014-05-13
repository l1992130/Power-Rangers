using UnityEngine;
using System.Collections;

public class EnemyHP : MonoBehaviour {
	public int HP = 2000;
	public int Armour;
	public float angryState;

	private int staticHP;
	private Boss boss;
	private Animator anim;
	private PlayerAttack playerAttack;
	// Use this for initialization
	void Start () {
		staticHP = HP;
	}

	void Awake ()
	{
		boss = GameObject.FindGameObjectWithTag ("Gluto").GetComponent<Boss> ();
		playerAttack = GameObject.FindGameObjectWithTag ("p1").GetComponent<PlayerAttack> ();
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {
		//print("staticHP:"+staticHP);
		if (HP < staticHP * angryState)
		{
			//print ("iam angry!");
			boss.angryFactor = 0.5f;
		}
		if (HP < 0)
		{
			playerAttack.isAttackEnable = false;
			anim.SetTrigger("Die");
			//Destroy (gameObject,2);
		}
	}
}
