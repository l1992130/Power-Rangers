using UnityEngine;
using System.Collections;

public class ScreenReltaivePosition : MonoBehaviour 
{
	public enum ScreenEdge {LEFT,RIGHT,TOP,BOTTOM};
	public ScreenEdge screenEdge;
	public float xOffset;
	public float yOffset;

	// Use this for initialization
	void Start () 
	{
		Vector3 newPosition = transform.position;
		Camera camera = Camera.main;

		switch(screenEdge)
		{
		case ScreenEdge.TOP:
			newPosition.x = xOffset;
			newPosition.y = camera.orthographicSize + yOffset;
			break;
		case ScreenEdge.BOTTOM:
			newPosition.x = xOffset;
			newPosition.y = -camera.orthographicSize + yOffset;
			break;
		case ScreenEdge.LEFT:
			newPosition.x = -camera.aspect * camera.orthographicSize + xOffset;
			newPosition.y = yOffset;
			break;
		case ScreenEdge.RIGHT:
			newPosition.x = camera.aspect * camera.orthographicSize + xOffset;
			newPosition.y = yOffset;
			break;
		}
		transform.position = newPosition;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
