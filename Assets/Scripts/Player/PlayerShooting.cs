using UnityEngine;

//using UnitySampleAssets.CrossPlatformInput;

namespace CompleteProject
{
	public class PlayerShooting : MonoBehaviour
	{
		public Transform boardHead;
		public int damagePerShot = 20;                  // The damage inflicted by each bullet.
		public float timeBetweenBullets = 0.5f;         // The time between each shot.
		public float range = 100f;                      // The distance the gun can fire.
		public Transform gazePoint;

		float timer;                                    // A timer to determine when to fire.
		Ray shootRay;                                   // A ray from the gun end forwards.
		RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
		int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
		ParticleSystem gunParticles;                    // Reference to the particle system.
		LineRenderer gunLine;                           // Reference to the line renderer.
		AudioSource gunAudio;                           // Reference to the audio source.
		Light gunLight;                                 // Reference to the light component.
		float effectsDisplayTime = 0.1f;                // The proportion of the timeBetweenBullets that the effects will display for.


		void Awake ()
		{
			// Create a layer mask for the Shootable layer.
			shootableMask = LayerMask.GetMask ("Shootable");

			// Set up the references.
			gunParticles = GetComponent<ParticleSystem> ();
			gunLine = GetComponent <LineRenderer> ();
			gunAudio = GetComponent<AudioSource> ();
			gunLight = GetComponent<Light> ();
		}

		void Update ()
		{
			// Add the time since Update was last called to the timer.
			timer += Time.deltaTime;

			if (timer >= timeBetweenBullets && Time.timeScale != 0) {
				Shoot ();
			}

			// If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
			if (timer >= timeBetweenBullets * effectsDisplayTime) {
				// ... disable the effects.
				DisableEffects ();
			}
		}

		public void DisableEffects ()
		{
			// Disable the line renderer and the light.
			gunLine.enabled = false;
			gunLight.enabled = false;
		}

		void Shoot ()
		{
			// Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
			shootRay.origin = boardHead.position;
			shootRay.direction = boardHead.forward;

			if (Physics.Raycast (shootRay, out shootHit, range, shootableMask)) {

				gazePoint.position = shootHit.point;
				float Cachescale = 0.015f * (shootHit.point - boardHead.position).magnitude;
				gazePoint.localScale = new Vector3 (Cachescale, Cachescale, Cachescale);
				
				EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
				
				// If the EnemyHealth component exist...
				if (enemyHealth != null) {
					// ... the enemy should take damage.
					enemyHealth.TakeDamage (damagePerShot, shootHit.point);
				} else {
					return;
				}

				// Reset the timer.
				timer = 0f;
				
				// Play the gun shot audioclip.
				gunAudio.Play ();
				
				// Enable the lights.
				gunLight.enabled = true;
				
				// Stop the particles from playing if they were, then start the particles.
				gunParticles.Stop ();
				gunParticles.Play ();
				
				// Enable the line renderer and set it's first position to be the end of the gun.
				gunLine.SetPosition (0, transform.position);
				gunLine.SetPosition (1, shootHit.point);
				gunLine.enabled = true;
			} else {

				gazePoint.localScale = Vector3.zero;

			}
		}
	}
}