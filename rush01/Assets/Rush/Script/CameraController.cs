using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	const int orthographicSizeMin = 2;
	const int orthographicSizeMax = 8;
	Vector3 offset;

	void Start()
	{
		offset = new Vector3(transform.position.x - target.position.x, transform.position.y - target.position.y, transform.position.z - target.position.z);
	}

	void Update () 
	{
		if ( Input.GetAxis("Mouse ScrollWheel") < 0 )
			Camera.main.orthographicSize++;
		if ( Input.GetAxis("Mouse ScrollWheel") > 0 )
			Camera.main.orthographicSize--;
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, orthographicSizeMin, orthographicSizeMax );
		transform.position = new Vector3 (target.position.x + offset.x, target.position.y + offset.y, target.position.z + offset.z);
	}
}
