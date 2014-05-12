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
		print ("idoit");
		if (colliderObject == "Untagged") 
			return;
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
		print (enemyHP.HP);
//		if(playerAnimState.IsName("PGSweep"))
//			return Random.Range(40,90);
//		return 0;
	}
	void OnCollisionEnter2D(Collision2D coll) 
	{
		//print (coll.collider.tag);
		colliderObject = coll.collider.tag;
	}
}
