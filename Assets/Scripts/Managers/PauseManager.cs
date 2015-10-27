using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour
{
	
	public AudioMixerSnapshot paused;
	public AudioMixerSnapshot unpaused;
	Canvas canvas;
	
	void Start ()
	{
		canvas = GetComponent<Canvas> ();
	}
	
	void Update ()
	{
		bool action = false;
		if (Input.gyro.enabled) {
			float cache = Mathf.Abs (Input.gyro.gravity.z);
			if (!canvas.enabled && (cache > 0.8f)) {
				action = true;
			}
			if (canvas.enabled && (cache <= 0.3f)) {
				action = true;
			}
		} else if (Input.GetKeyDown (KeyCode.Escape)) {
			action = true;
		}
		if (action) {
			canvas.enabled = !canvas.enabled;
			Pause ();
		}
	}
	
	public void Pause ()
	{
		Time.timeScale = Time.timeScale == 0 ? 1 : 0;
		Lowpass ();
		
	}
	
	void Lowpass ()
	{
		if (Time.timeScale == 0) {
			paused.TransitionTo (.01f);
		} else {
			unpaused.TransitionTo (.01f);
		}
	}
	
	public void Quit ()
	{
		#if UNITY_EDITOR 
		EditorApplication.isPlaying = false;
		#else 
		Application.Quit ();
		#endif
	}
}
