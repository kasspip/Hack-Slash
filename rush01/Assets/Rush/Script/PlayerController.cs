using UnityEngine;
using System.Collections;

public class PlayerController : Unit {

	public int experienceFactor;
	public int currentXP = 0;
	public int nextLevelXP;
	public int level = 1;
	[HideInInspector]public int skillPoints = 0;
	public GameObject[] spells;
	
	void GainLevel()
	{
		level++;
		nextLevelXP += experienceFactor * level;
		skillPoints++;
		Debug.Log ("Ding ! level " + level);
	}

	public void GainXP(int xp)
	{
		currentXP += xp;
	}

	void Update ()
	{
		if (nextLevelXP == 0)
			nextLevelXP = experienceFactor;
		UpdateUnit ();
		if (life <= 0)
			return;
		if (Input.GetMouseButtonDown (0)) 
		{
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100)) {
				if (hit.collider.gameObject.tag == "Floor") {
					MoveTo (hit.point);
					target = null;
					attacks = false;
				} else if (hit.collider.gameObject.tag == "Unit" && hit.collider.gameObject.GetComponent<Unit> ().faction == Faction.Enemy) {
					target = hit.collider.gameObject;
					attacks = false;
					MoveTo (target.transform.position);
				}	
			}
		}
		if (Input.GetKeyDown (KeyCode.Alpha1))
			CastSpell (0);
		if (Input.GetKeyDown (KeyCode.Alpha2))
			CastSpell (1);
		if (Input.GetKeyDown (KeyCode.Alpha3))
			CastSpell (2);	
		if (currentXP >= nextLevelXP)
			GainLevel();
	}

	public void CastSpell(int id)
	{
		if (spells.Length > id) {
			GameObject go = GameObject.Instantiate (spells [id], transform.position, Quaternion.identity) as GameObject;
			go.transform.SetParent(transform);
			Spell script = go.GetComponent<Spell> ();
			script.Cast (gameObject);
			GameObject.Destroy(go, script.lifeTime);
		}
	}

	public override void OnAttackZoneEnter(GameObject other)
	{
		Unit unit = other.GetComponent<Unit> ();
		if (unit.faction == Faction.Enemy && other.gameObject == target) {
			attacks = true;
		}
	}

	public void OnTriggerEnter( Collider other )
	{
		if (other.gameObject.tag == "Potion") {
			Heal(30);
			GameObject.Destroy(other.gameObject);
		}
	}
}
