using UnityEngine;
using System.Collections;

public class AttackZone : MonoBehaviour {

	public GameObject owner;

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "AttackZone")
			owner.GetComponent<Unit> ().OnAttackZoneEnter (other.gameObject.GetComponent<AttackZone> ().owner);
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "AttackZone")
			owner.GetComponent<Unit> ().OnAttackZoneExit (other.gameObject.GetComponent<AttackZone> ().owner);
	}
}
