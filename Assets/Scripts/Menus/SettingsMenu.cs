using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

	[Header("Settings")]
	[SerializeField]
	private Slider m_MasterVolume;
	[SerializeField]
	private Slider m_MusicVolume;
	[SerializeField]
	private Slider m_SFXVolume;

	[Header("Menus")]
	[SerializeField]
	private GameObject m_MainMenu;

	private void Start()
	{
		if(PlayerPrefs.HasKey("Master Volume"))
			m_MasterVolume.value = PlayerPrefs.GetFloat("Master Volume");

		if (PlayerPrefs.HasKey("Music Volume"))
			m_MusicVolume.value = PlayerPrefs.GetFloat("Music Volume");

		if (PlayerPrefs.HasKey("Sound Effect Volume"))
			m_SFXVolume.value = PlayerPrefs.GetFloat("Sound Effect Volume");
	}

	public void OnBack()
	{
		RetroEffect.Get().StartFlicker();
		gameObject.SetActive(false);
		m_MainMenu.SetActive(true);
	}

	public void OnMasterVolume()
	{
		PlayerPrefs.SetFloat("Master Volume", m_MasterVolume.value);
		AudioManager.Get().UpdateSFXVolume();
	}

	public void OnMusicVolume()
	{
		PlayerPrefs.SetFloat("Music Volume", m_MusicVolume.value);
	}

	public void OnSFXVolume()
	{
		PlayerPrefs.SetFloat("Sound Effect Volume", m_SFXVolume.value);
		AudioManager.Get().UpdateSFXVolume();
	}
}
