//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
	public class Impact : MonoBehaviour
	{

		[Header("Impact Despawn Timer")]
		//How long before the impact is destroyed
		public float despawnTimer = 10.0f;

		[Header("Audio")]
		public AudioClip[] impactSounds;

		public AudioSource audioSource;

		public event UnityAction<Impact> Impacted;

        private void OnEnable()
		{
			// Start the despawn timer
			StartCoroutine(DespawnTimer());

			//Get a random impact sound from the array
			audioSource.clip = impactSounds
				[Random.Range(0, impactSounds.Length)];
			//Play the random impact sound
			audioSource.Play();
        }

		private IEnumerator DespawnTimer()
		{
			//WaitBeforeLockCursor for set amount of time
			yield return new WaitForSeconds(despawnTimer);
            //Destroy the impact gameobject
            Impacted?.Invoke(this);
		}
	}
}