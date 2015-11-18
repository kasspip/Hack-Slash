using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum Faction
{
	Ally,
	Enemy
}

public class Unit : MonoBehaviour 
{
	public float strength;
	public float agility;
	public float constitution;
	public GameObject rightEquipement;
	//public GameObject leftEquipement;
	public Faction faction;

	public float damage;
	public float dodge;
	public float precision;
	public float life;
	public float maxLife;
	public float attackCD;
	public float speed;
	public int experienceGain;

	[HideInInspector]public Image healthBar;
	[HideInInspector]public GameObject rightHand;
	[HideInInspector]public GameObject leftHand;
	[HideInInspector]public GameObject target = null;

	protected NavMeshAgent nav;
	protected Animator animator;
	protected Coroutine routineAtk = null;
	protected Coroutine routineDie = null;
	protected Coroutine routineInfos = null;
	protected bool attacks = false;

	void Start()
	{
		nav = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		CalculateStats ();
		life = maxLife;
		if (rightEquipement)
		{
			rightEquipement = GameObject.Instantiate(rightEquipement, rightHand.transform.position, Quaternion.identity) as GameObject;
			rightEquipement.transform.SetParent(rightHand.transform);
		}
		//if (leftEquipement)
		//{
		//	leftEquipement = GameObject.Instantiate(leftEquipement, leftHand.transform.position, Quaternion.identity) as GameObject;
		//	leftEquipement.transform.SetParent(leftHand.transform);
		//}
	}

	void CalculateStats()
	{
		maxLife = constitution * 2f;

		damage = strength / 2f;

		precision = 50f + agility * 5f;
		dodge = agility;
		attackCD = Mathf.Clamp(3f - (agility / 4f), 0.5f, 3f);
		speed = Mathf.Clamp(2f + agility / 4f, 2f, 5f);

		nav.speed = speed;
	}

	public void UpdateUnit ()
	{
		if (life <= 0) {
			if (routineDie == null)
				routineDie = StartCoroutine (DieRoutine (0.25f));
			return ;
		}
		animator.SetFloat ("Speed", Mathf.Max( Mathf.Abs( nav.velocity.x), Mathf.Abs( nav.velocity.z)));
		if (target != null) 
		{
			if (target.GetComponent<Unit>().life <= 0)
			{
				target = null;
				attacks = false;
			}
			else if (attacks == true)
				AttackUnit (target);
			else
				MoveTo (target.transform.position);
		}
	}

	public void MoveTo(Vector3 destination)
	{
		if (life > 0)
			nav.SetDestination( destination );
	}

	public virtual void OnAttackZoneEnter (GameObject other)
	{}
	
	public virtual void OnAggroZoneEnter (GameObject other)
	{}

	public virtual void OnAttackZoneExit (GameObject other)
	{
		attacks = false;
	}

	public virtual void OnAggroZoneExit (GameObject other)
	{}

	public void GetHit(GameObject attacker, float dammage, float delay)
	{
		float rand = Random.Range (0, 100);
		if (rand <= dodge)
			Debug.Log ("Dodge hit");
		else
		{
			if (life - dammage > 0)
			{
				life -= dammage;
				if (routineAtk == null)
					StartCoroutine(HitRoutine(delay));			
			}
			else
			{
				if (attacker.name == "Player")
					attacker.GetComponent<PlayerController>().GainXP(experienceGain);
				life = 0;
			}
		}
		if (routineInfos == null)
			routineInfos = StartCoroutine(UpdateInfos(delay));	
	}

	public void AttackUnit(GameObject other)
	{
		transform.LookAt (target.transform.position);
		if (routineAtk == null)
			routineAtk = StartCoroutine (AttackRoutine());
	}

	public void Heal(float amountPercent)
	{
		life = Mathf.Clamp( life + (maxLife / 100 * amountPercent), 0, maxLife );
		healthBar.fillAmount = life / maxLife;
	}

	public IEnumerator AttackRoutine()
	{
		float rand = Random.Range (0, 100);
		if (rand <= precision)
			target.GetComponent<Unit> ().GetHit (gameObject, damage, 0.5f);
		else
			Debug.Log ("Miss hit");
		nav.destination = transform.position;
		animator.SetTrigger ("Atk1");
		yield return new WaitForSeconds(attackCD);
		routineAtk = null;
		
	}

	public void BuffHaste(float amount, float time)
	{
		StartCoroutine(BuffHasteRouting (amount, time));
	}

	public IEnumerator BuffHasteRouting(float amount, float time)
	{
		speed += amount;
		nav.speed = speed;
		yield return new WaitForSeconds(time);
		speed -= amount;
		nav.speed = speed;
	}

	public IEnumerator UpdateInfos(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		healthBar.fillAmount = life / maxLife;
		routineInfos = null;
	}

	public IEnumerator HitRoutine(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		animator.SetTrigger ("Hit1");
	}

	public IEnumerator DieRoutine(float timeToWait)
	{
		yield return new WaitForSeconds(timeToWait);
		animator.SetTrigger("Die");
		gameObject.GetComponent<SphereCollider> ().enabled = false;
		healthBar.transform.parent.gameObject.SetActive( false );
	}
}
