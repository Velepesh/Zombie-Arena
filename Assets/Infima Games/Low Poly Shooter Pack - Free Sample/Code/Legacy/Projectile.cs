﻿using System;
using UnityEngine;
using System.Collections;
using InfimaGames.LowPolyShooterPack;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour 
{
	[SerializeField] private int _damage;
	[Range(5, 100)]
	[Tooltip("After how long time should the bullet prefab be destroyed?")]
	public float destroyAfter;
	[Tooltip("If enabled the bullet destroys on impact")]
	public bool destroyOnImpact = false;
	[Tooltip("Minimum time after impact that the bullet is destroyed")]
	public float minDestroyTime;
	[Tooltip("Maximum time after impact that the bullet is destroyed")]
	public float maxDestroyTime;

	[Header("Impact Effect Prefabs")]
	public Transform [] bloodImpactPrefabs;
	public Transform [] metalImpactPrefabs;
	public Transform [] dirtImpactPrefabs;
	public Transform []	concreteImpactPrefabs;

    private void OnValidate()
    {
		_damage = Math.Clamp(_damage, 0, int.MaxValue);
    }

    private void Start ()
	{
		//Grab the game mode service, we need it to access the player character!
		var gameModeService = ServiceLocator.Current.Get<IGameModeService>();
		//Ignore the main player character's collision. A little hacky, but it should work.
		Physics.IgnoreCollision(gameModeService.GetPlayerCharacter().GetComponent<Collider>(), GetComponent<Collider>());
		
		//Start destroy timer
		StartCoroutine (DestroyAfter ());
	}

	//If the bullet collides with anything
	private void OnCollisionEnter (Collision collision)
	{
		//Ignore collisions with other projectiles.
		if (collision.gameObject.GetComponent<Projectile>() != null)
			return;
		
		//If destroy on impact is false, start 
		//coroutine with random destroy timer
		if (!destroyOnImpact) 
		{
			StartCoroutine(DestroyTimer());
		}
		//Otherwise, destroy bullet on impact
		else 
		{
			Destroy (gameObject);
		}

        if (collision.gameObject.TryGetComponent(out DamageHandler damageHandler))
        {
			damageHandler.TakeDamage(_damage, collision.contacts[0].normal);
            //Instantiate random impact prefab from array
            var go = Instantiate(bloodImpactPrefabs[Random.Range
                (0, bloodImpactPrefabs.Length)], transform.position,
                Quaternion.LookRotation(collision.contacts[0].normal));
			//Destroy bullet object
			go.SetParent(collision.gameObject.transform);
            Destroy(gameObject);
        }

        //If bullet collides with "Blood" tag
        if (collision.transform.tag == "Blood") 
		{
            //Instantiate random impact prefab from array
            var go = Instantiate(bloodImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));

            go.SetParent(collision.gameObject.transform);
            //Destroy bullet object
            Destroy(gameObject);
		}

		//If bullet collides with "Metal" tag
		if (collision.transform.tag == "Metal") 
		{
            //Instantiate random impact prefab from array
            var go = Instantiate(metalImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
            //Destroy bullet object

            go.SetParent(collision.gameObject.transform);
            Destroy(gameObject);
		}

		//If bullet collides with "Dirt" tag
		if (collision.transform.tag == "Dirt") 
		{
            //Instantiate random impact prefab from array
            var go = Instantiate (dirtImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
            //Destroy bullet object
            go.SetParent(collision.gameObject.transform);
            Destroy(gameObject);
		}

		//If bullet collides with "Concrete" tag
		if (collision.transform.tag == "Concrete") 
		{
            //Instantiate random impact prefab from array
            var go = Instantiate(concreteImpactPrefabs [Random.Range 
				(0, bloodImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
            //Destroy bullet object
            go.SetParent(collision.gameObject.transform);
            Destroy(gameObject);
		}

		//If bullet collides with "Target" tag
		if (collision.gameObject.TryGetComponent(out TargetScript target))
		{
            //Toggle "isHit" on target object
            target.isHit = true;
            //Destroy bullet object
            Destroy(gameObject);
        }

		//if (collision.transform.tag == "Target") 
		//{
		//	//Toggle "isHit" on target object
		//	collision.transform.gameObject.GetComponent
		//		<TargetScript>().isHit = true;
		//	//Destroy bullet object
		//	Destroy(gameObject);
		//}
			
		//If bullet collides with "ExplosiveBarrel" tag
		if (collision.gameObject.TryGetComponent(out ExplosiveBarrelScript explosiveBarrel)) 
		{
            //Toggle "explode" on explosive barrel object
            explosiveBarrel.explode = true;
			//Destroy bullet object
			Destroy(gameObject);
		}

        ////If bullet collides with "ExplosiveBarrel" tag
        //if (collision.transform.tag == "ExplosiveBarrel")
        //{
        //    //Toggle "explode" on explosive barrel object
        //    collision.transform.gameObject.GetComponent
        //        <ExplosiveBarrelScript>().explode = true;
        //    //Destroy bullet object
        //    Destroy(gameObject);
        //}

        //If bullet collides with "GasTank" tag
        if (collision.gameObject.TryGetComponent(out GasTankScript gasTankScript))
        {
            //Toggle "isHit" on gas tank object
            gasTankScript.isHit = true;
			//Destroy bullet object
			Destroy(gameObject);
		}

        ////If bullet collides with "GasTank" tag
        //if (collision.transform.tag == "GasTank")
        //{
        //    //Toggle "isHit" on gas tank object
        //    collision.transform.gameObject.GetComponent
        //        <GasTankScript>().isHit = true;
        //    //Destroy bullet object
        //    Destroy(gameObject);
        //}
    }

	private IEnumerator DestroyTimer () 
	{
		//Wait random time based on min and max values
		yield return new WaitForSeconds
			(Random.Range(minDestroyTime, maxDestroyTime));
		//Destroy bullet object
		Destroy(gameObject);
	}

	private IEnumerator DestroyAfter () 
	{
		//Wait for set amount of time
		yield return new WaitForSeconds (destroyAfter);
		//Destroy bullet object
		Destroy (gameObject);
	}
}