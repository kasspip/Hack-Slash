using UnityEngine;
using System.Collections;

public class EnemyController : Unit
{
	public float lootChance;
	public GameObject[] lootTable;

	bool hasLoot = false;

	void Update () 
	{
		UpdateUnit ();
		if (life <= 0) {
			if (hasLoot == false)
				DropLoot();
			return;
		}
	}

	void DropLoot()
	{
		StartCoroutine (PopLoot(1f));
		hasLoot = true;
	}

	public override void OnAttackZoneEnter(GameObject other)
	{
		Unit unit = other.GetComponent<Unit> ();
		if (unit.faction == Faction.Ally) {
			attacks = true;
		}
	}
	
	public override void OnAggroZoneEnter(GameObject other)
	{
		Unit unit = other.GetComponent<Unit> ();
		if (unit.faction == Faction.Ally) {
			target = other;
			MoveTo (target.transform.position);
		}
	}

	public override void OnAggroZoneExit(GameObject other)
	{
		Unit unit = other.GetComponent<Unit> ();
		if (unit.faction == Faction.Ally) {
			target = null;
			nav.destination = transform.position;
		}
	}

	public IEnumerator PopLoot(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		float rand = Random.Range (0, 100);
		if (rand <= lootChance && lootTable.Length > 0) {
			int choice = Mathf.RoundToInt( Random.Range(0, lootTable.Length ));
			GameObject.Instantiate(lootTable[choice], transform.position + new Vector3(Random.Range(0,3), 0, Random.Range(0,3)), lootTable[choice].transform.rotation);
		}
	}
}
