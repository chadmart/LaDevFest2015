using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class MixLevels : MonoBehaviour
{
	public AudioMixer masterMixer;

	public void SetSfxLvl (float sfxLvl)
	{
		masterMixer.SetFloat ("sfxVol", sfxLvl);
	}

	public void SetMusicLvl (float musicLvl)
	{
		masterMixer.SetFloat ("musicVol", musicLvl);
	}

	public void SetMuteMusic (bool unmute)
	{
		AudioListener.volume = unmute ? 1f : 0f;
	}
}