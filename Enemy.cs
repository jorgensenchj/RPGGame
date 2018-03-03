using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
public class Enemy : MonoBehaviour, IDamagable {
	
	[SerializeField] float maxHealthPoints = 100;
	[SerializeField] float chaseRadius = 6f;
	[SerializeField] float attackRadius = 4f;
	[SerializeField] float damagePerShot = 9f;
	[SerializeField] float secondsBetweenShots = 1f;
	[SerializeField] GameObject arrowToUse;
	[SerializeField] GameObject arrowSocket;
    float currentHealthPoints;
	[SerializeField] Vector3 aimOffSet = new Vector3(0f,1f,0f);
	AICharacterControl  aiCharacterControl = null;
	GameObject player = null;

	public bool isAttacking = false;


	public float healthAsPercentage
	{
		get
		{
			return currentHealthPoints / maxHealthPoints;
		}
	}

     void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		aiCharacterControl = GetComponent<AICharacterControl>();
		currentHealthPoints = maxHealthPoints;
	}
	void Update()
	{
		float distanceToPlayer = Vector3.Distance (player.transform.position, transform.position);
		if (distanceToPlayer <= attackRadius && !isAttacking)
		{
			isAttacking = true;
			InvokeRepeating ("SpawnArrow", 0f, secondsBetweenShots);//TODO
			//TODO spawn arrow
		}
		if (distanceToPlayer > attackRadius)
		{
		    isAttacking = false;
			CancelInvoke();
		}

				if (distanceToPlayer <= chaseRadius) 
		{
			aiCharacterControl.SetTarget (player.transform);
		} 
		else
		{
			aiCharacterControl.SetTarget (transform);
		}
	}

	void SpawnArrow()
	{
		GameObject newArrow = Instantiate (arrowToUse, arrowSocket.transform.position, Quaternion.identity);
		ArrowUD arrowComponent = newArrow.GetComponent<ArrowUD> ();
		arrowComponent.SetDamage (damagePerShot);
		Vector3 unitVectorToPlayer = (player.transform.position + aimOffSet - arrowSocket.transform.position).normalized;
		float arrowSpeed = arrowComponent.arrowSpeed;
		newArrow.GetComponent<Rigidbody> ().velocity = unitVectorToPlayer * arrowSpeed;
	}

	public void TakeDamage (float damage)
	{
		currentHealthPoints = Mathf.Clamp (currentHealthPoints - damage,0f,maxHealthPoints );
		if (currentHealthPoints <= 0) 
		{
			Destroy (gameObject);
		}
	}
	void OnDrawGizmos()
		{
		
		   //Draw attack shpere
		Gizmos.color = new Color(255f,0, 0, 0.5f);
		Gizmos.DrawWireSphere (transform.position, attackRadius);
		//Draw Chase shpere
		Gizmos.color = new Color(0,0, 0255f, 0.5f);
		Gizmos.DrawWireSphere (transform.position, chaseRadius);
		}
}


