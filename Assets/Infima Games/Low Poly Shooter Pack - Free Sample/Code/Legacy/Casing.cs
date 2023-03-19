//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Collections;
using UnityEngine.Events;

namespace InfimaGames.LowPolyShooterPack.Legacy
{
	[RequireComponent(typeof(Rigidbody))]
	public class Casing : MonoBehaviour
	{
		[Header("Force X")]
		[Tooltip("Minimum force on X axis")]
		public float minimumXForce;

		[Tooltip("Maimum force on X axis")]
		public float maximumXForce;

		[Header("Force Y")]
		[Tooltip("Minimum force on Y axis")]
		public float minimumYForce;

		[Tooltip("Maximum force on Y axis")]
		public float maximumYForce;

		[Header("Force Z")]
		[Tooltip("Minimum force on Z axis")]
		public float minimumZForce;

		[Tooltip("Maximum force on Z axis")]
		public float maximumZForce;

		[Header("Rotation Force")]
		[Tooltip("Minimum initial rotation value")]
		public float minimumRotation;

		[Tooltip("Maximum initial rotation value")]
		public float maximumRotation;

		[Header("Despawn Time")]
		[Tooltip("How long after spawning that the casing is destroyed")]
		public float despawnTime;

		[Header("Audio")]
		public AudioClip[] casingSounds;

		public AudioSource audioSource;

		[Header("Spin Settings")]
		//How fast the casing spins
		[Tooltip("How fast the casing spins over time")]
		public float speed = 2500.0f;

		public event UnityAction<Casing> Disabled;

		private Rigidbody _rigidbody;

        //Launch the casing at start
        private void Awake()
		{
            _rigidbody = GetComponent<Rigidbody>();
		}

        private void OnEnable()
        {
            AddRelativeTorque();
			AddRelativeForce();
            //Start the remove/destroy coroutine
            StartCoroutine(RemoveCasing());
            //Set random rotation at start
            transform.rotation = Random.rotation;
            //Start play sound coroutine
            StartCoroutine(PlaySound());
        }

		private void FixedUpdate()
		{
			//Spin the casing based on speed value
			transform.Rotate(Vector3.right, speed * Time.deltaTime);
			transform.Rotate(Vector3.down, speed * Time.deltaTime);
		}

        private void AddRelativeTorque()
		{
            _rigidbody.AddRelativeTorque(
				 Random.Range(minimumRotation, maximumRotation), //X Axis
				 Random.Range(minimumRotation, maximumRotation), //Y Axis
				 Random.Range(minimumRotation, maximumRotation) //Z Axis
				 * Time.deltaTime);
        }

        private void AddRelativeForce()
		{
            //Random direction the casing will be ejected in
            _rigidbody.AddRelativeForce(
                Random.Range(minimumXForce, maximumXForce), //X Axis
                Random.Range(minimumYForce, maximumYForce), //Y Axis
                Random.Range(minimumZForce, maximumZForce)); //Z Axis		    
        }

		private IEnumerator PlaySound()
		{
			//Wait for random time before playing sound clip
			yield return new WaitForSeconds(Random.Range(0.25f, 0.85f));
			//Get a random casing sound from the array 
			audioSource.clip = casingSounds
				[Random.Range(0, casingSounds.Length)];
			//Play the random casing sound
			audioSource.Play();
		}

		private IEnumerator RemoveCasing()
		{
			yield return new WaitForSeconds(despawnTime);
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            Disabled?.Invoke(this);
		}
	}
}