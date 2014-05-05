using UnityEngine;
using System.Collections;

public class ComboTest : MonoBehaviour {

	//public Animation[] AttackAnis;
	private string[] AttackAnis = new string[] { "断子绝孙脚", "还我漂漂拳", "无敌龙抓手", "无敌风火轮" };
	private int curskill;
	private bool attackagain;
	private bool releaseing;
	private float timer;
	private float skillspacing =1f;
	// Use this for initialization
	void Start()
	{
	}
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			ReleaseSkill();
		}
	}
	public void ReleaseSkill() 
	{
		if (releaseing)
			attackagain = true;
		timer = 0;
		StartCoroutine(ReleaseSkills());
	}
	private IEnumerator ReleaseSkills()
	{
		releaseing = true;
		if (attackagain)
			curskill++;
		if (curskill >= AttackAnis.Length)
			curskill = 0;
		//AttackAnis[curskill].Play(); 
		Debug.Log("你释放了:" + AttackAnis[curskill]);
		while (timer < skillspacing)
		{
			timer += Time.deltaTime;
			//Debug.Log(timer);
			if (attackagain)
			{
				attackagain = false;
				yield break;
			}
			yield return 0;
		}
		curskill = 0;
		releaseing = false;
		Debug.Log("释放完毕!");
		yield return 0;
	}
}
