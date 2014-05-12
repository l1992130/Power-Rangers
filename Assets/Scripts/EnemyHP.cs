using UnityEngine;
using System.Collections;

public class EnemyHP : MonoBehaviour {
	public int HP;

	public int Armour;
	public float angryState;
	private int staticHP;
	// Use this for initialization
	void Start () {
		staticHP = HP;
	}
	
	// Update is called once per frame
	void Update () {
		print("staticHP:"+staticHP);
		if (HP < staticHP * angryState)
			return;
	}
}
