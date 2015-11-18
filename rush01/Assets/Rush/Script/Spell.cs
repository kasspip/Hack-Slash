using UnityEngine;
using System.Collections;

public enum Range
{
	self,
	drop,
	cast
}

public class Spell : MonoBehaviour
{
	public GameObject targetVisual;
	public Range type;
	public float damage;
	public float heal;
	public float hasteBonus;
	public float CD;
	public float lifeTime;
	
	GameObject targetZone = null;
	bool validateSpell = false;
	GameObject caster = null;

	void Update()
	{
		if (targetZone) 
		{
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100)) 
			{
				if (hit.collider.gameObject.tag == "Floor") 
				{
					targetZone.transform.position = hit.point;
				}
			}
			if (Input.GetMouseButtonDown(1))
				SpellEffect();
		}
	}

	public void Cast(GameObject c)
	{
		caster = c;
		if (type == Range.self)
			SpellEffect ();
		else if (type == Range.drop)
			DropSpell ();
		else if (type == Range.cast)
			DirectSpell ();
	}

	void SpellEffect()
	{
		Unit unit = caster.GetComponent<Unit> ();
		if (heal > 0)
			unit.Heal (heal);
		if (hasteBonus > 0)
			unit.BuffHaste (hasteBonus, lifeTime);
	}

	void DropSpell()
	{
		targetZone = GameObject.Instantiate (targetVisual) as GameObject;
	}

	void DirectSpell()
	{

	}

	void OnTriggerEnter(Collider other)
	{
		if (damage > 0 && other.tag == "Unit") 
		{
			Unit unit = other.gameObject.GetComponent<Unit> ();
			if (unit.faction == Faction.Enemy)
			{
				unit.GetHit(caster, damage, 0f);
			}
		}
	}

}
