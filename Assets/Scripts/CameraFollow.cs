using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public float xMargin = 1f;
	public float yMargin = 1f;
	public float xSmooth = 8f;
	public float ySmooth = 8f;
	public Vector2 maxXAndY;
	public Vector2 minXAndY;

	private Transform player;

	void Awake ()
	{
		player = GameObject.FindGameObjectWithTag ("p1").transform;
	}

	bool checkXMargin ()
	{
		return Mathf.Abs (transform.position.x - player.position.x) > xMargin;
	}

	bool checkYMargin ()
	{
		return Mathf.Abs (transform.position.y - player.position.y) > yMargin;
	}

	void FixedUpdate ()
	{
		trackPlayer ();
	}

	void trackPlayer ()
	{
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		if (checkXMargin ())
			targetX = Mathf.Lerp (transform.position.x, player.position.x, xSmooth * Time.deltaTime);
		if (checkYMargin ())
			targetY = Mathf.Lerp (transform.position.y, player.position.y, ySmooth * Time.deltaTime);

		targetX = Mathf.Clamp (targetX, minXAndY.x, maxXAndY.y);
		targetY = Mathf.Clamp (targetY, minXAndY.x, maxXAndY.y);

		transform.position = new Vector3 (targetX, targetY, transform.position.z);
	}
}
