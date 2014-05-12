using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	private string colliderObject;
	private AnimatorStateInfo stateInfo;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		stateInfo = GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0);
	}
	void isPlayerAttack (int attackCount)
	{

		if (colliderObject == "Untagged" || colliderObject == "") 
			return;
		print (colliderObject);
		EnemyHP enemyHP = GameObject.FindGameObjectWithTag (colliderObject).GetComponent<EnemyHP> ();
//		AnimatorStateInfo playerAnimState = GameObject.FindGameObjectWithTag ("player").GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0);
		if(stateInfo.IsName("PGAttackKick") || stateInfo.IsName("PGBoxing"))
			enemyHP.HP -= Random.Range (20, 40) - enemyHP.Armour;
		if(stateInfo.IsName("PGKeepKick") || stateInfo.IsName("PGKeepBoxing"))
		{
			if (attackCount == 1)
				enemyHP.HP -= Random.Range (30, 50) - enemyHP.Armour;
			if (attackCount == 2)
				enemyHP.HP -= Random.Range (30, 50) - enemyHP.Armour;
			if (attackCount == 3)
				enemyHP.HP -= Random.Range (35, 55) - enemyHP.Armour;
			if (attackCount == 4)
				enemyHP.HP -= Random.Range (40, 60) - enemyHP.Armour;
		}
		if(stateInfo.IsName("PGPunch") || stateInfo.IsName("PGRunPunch"))
		{
			float random = Random.Range(1,100);
			if (random > 50)
				enemyHP.HP -= Random.Range (60,100) * 2 * attackCount - enemyHP.Armour;
			else
				enemyHP.HP -= Random.Range (60,100) * 2 * attackCount - enemyHP.Armour;
		}
		if(stateInfo.IsName("PGSweep"))
			enemyHP.HP -= Random.Range (50, 70) - enemyHP.Armour;
		if(stateInfo.IsName("PGSweepSword") || stateInfo.IsName("PGKeepSweepSword"))
			enemyHP.HP -= Random.Range (30, 55) * attackCount - enemyHP.Armour;
		if(stateInfo.IsName("PGSword"+attackCount))
		{
			if (attackCount == 4)
				enemyHP.HP -= Random.Range (30, 40) * attackCount - enemyHP.Armour;
			else
				enemyHP.HP -= Random.Range (20, 30) * attackCount - enemyHP.Armour;
		}
		if (stateInfo.IsName("PGJumpHeaven"))
			enemyHP.HP -= Random.Range (50, 60) - enemyHP.Armour;
		colliderObject = "";
		print (enemyHP.HP);
//		if(playerAnimState.IsName("PGSweep"))aaaaaaaa
//			return Random.Range(40,90);
//		return 0;
	}
	void OnCollisionStay2D(Collision2D coll) 
	{
		//print (coll.collider.tag);
		colliderObject = coll.collider.tag;
	}
}
