using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	private static AudioManager s_Instance;

	[System.Serializable]
	public class Sound
	{
		public string name;
		public AudioClip[] clips;

		[HideInInspector]
		public AudioSource[] sorce;
	}

	[SerializeField]
	private Sound[] m_AudioClips;

	AudioManager()
	{
		if (s_Instance != null)
			return;

		s_Instance = this;
	}

	public static AudioManager Get() { return s_Instance; }

	private void Start()
	{
		foreach(Sound sound in m_AudioClips)
		{
			sound.sorce = new AudioSource[sound.clips.Length];
			for(int i = 0; i < sound.clips.Length; i++)
			{
				sound.sorce[i] = gameObject.AddComponent<AudioSource>();
				sound.sorce[i].clip = sound.clips[i];
			}
		}

		UpdateSFXVolume();
	}

	public void Play(string name, int index = -1)
	{
		Sound clip = Array.Find(m_AudioClips, sound => sound.name == name);
		if(index == -1)
		{
			if (clip.clips.Length == 1)
				index = 0;
			else
				index = UnityEngine.Random.Range(0, clip.clips.Length);
			
		}

		clip.sorce[index].Play();
	}

	public void UpdateSFXVolume()
	{
		float volume = PlayerPrefs.GetFloat("Master Volume") * PlayerPrefs.GetFloat("Sound Effect Volume");
		foreach (Sound sound in m_AudioClips)
		{
			for (int i = 0; i < sound.clips.Length; i++)
			{
				sound.sorce[i].volume = volume;
			}
		}
	}

}
