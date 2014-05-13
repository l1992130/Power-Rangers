using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void check()
	{
//		if (coll.collider.tag == "ground" || coll.collider.tag == "p1")
//			GetComponent<Animator>().SetTrigger("Collide");
	}
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.collider.tag == "ground" || coll.collider.tag == "p1")
			GetComponent<Animator>().SetTrigger("Collide");
		
	}
}
