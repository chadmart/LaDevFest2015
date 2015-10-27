using UnityEngine;

//using UnitySampleAssets.CrossPlatformInput;

namespace CompleteProject
{
	public class PlayerMovement : MonoBehaviour
	{
		public Transform boardCamera;
		public Transform eyePosition;
		public float speed = 6f;            // The speed that the player will move at.

		Vector3 movement;                   // The vector to store the direction of the player's movement.
		Animator anim;                      // Reference to the animator component.
		Rigidbody playerRigidbody;          // Reference to the player's rigidbody.

		void Awake ()
		{
			// Set up references.
			anim = GetComponent <Animator> ();
			playerRigidbody = GetComponent <Rigidbody> ();
		}

		bool walking = false;

		void Update ()
		{
			if (Cardboard.SDK.Triggered) {
				walking = !walking;
			}
		}

		void FixedUpdate ()
		{
			float h, v;
			// Store the input axes.

			h = walking ? Vector3.Dot (transform.forward, Vector3.right) : 0f;
			v = walking ? Vector3.Dot (transform.forward, Vector3.forward) : 0f;
			
			// Move the player around the scene.
			Move (h, v);

			// Animate the player.
			anim.SetBool ("IsWalking", walking);
		}

		void Move (float h, float v)
		{
			// Set the movement vector based on the axis input.
			movement.Set (h, 0f, v);
            
			// Normalise the movement vector and make it proportional to the speed per second.
			movement = movement.normalized * speed * Time.deltaTime;

			// Move the player to it's current position plus the movement.
			playerRigidbody.MovePosition (transform.position + movement);
			playerRigidbody.MoveRotation (Quaternion.LookRotation (Cardboard.Controller.Head.Gaze.direction));

			boardCamera.position = eyePosition.position;
		}
	}
}