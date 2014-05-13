using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	void DestroyGameObject (float waittime)
	{
		//print ("remove");
		if (waittime != 0)
			Destroy (gameObject,waittime);
		else
			Destroy (gameObject);
	}
}
