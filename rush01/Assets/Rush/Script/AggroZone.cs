using UnityEngine;
using System.Collections;

public class AggroZone : MonoBehaviour {

	public GameObject owner;
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "AggroZone")
			owner.GetComponent<Unit> ().OnAggroZoneEnter (other.gameObject.GetComponent<AggroZone> ().owner);
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "AggroZone")
			owner.GetComponent<Unit> ().OnAggroZoneExit (other.gameObject.GetComponent<AggroZone> ().owner);
	}
}
