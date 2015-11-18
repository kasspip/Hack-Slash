using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public GameObject SkinEquip;
	public float strengthBonus;
	public float agilityBonus;
	public float constitutionBonus;

	bool isEquip = false;

	void Update()
	{
		if (!isEquip)
			transform.Rotate (0,10 * Time.deltaTime,0);
	}

	public void Equip( Transform hand )
	{
		if (isEquip)
			return;

		isEquip = true;
	}
	public void UnEquip( Transform hand )
	{
		if (isEquip)
			return;
		
		isEquip = true;
	}
}
