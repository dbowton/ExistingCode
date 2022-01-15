using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	private static MainMenu s_Instance;

	[Header("Buttons")]
	[SerializeField]
	private GameObject m_PlayBtn;
	[SerializeField]
	private GameObject m_SettingsBtn;
	[SerializeField]
	private GameObject m_ExitBtn;

	[Header("Menus")]
	[SerializeField]
	private GameObject m_SettingsMenu;

	MainMenu()
	{
		if (s_Instance != null)
			return;
		s_Instance = this;
	}

	private void Start()
	{
		SetButtonsAvtive(false);
	}

	public void EnableButtons()
	{
		SetButtonsAvtive(true);
	}

	private void SetButtonsAvtive(bool active)
	{
		m_PlayBtn.SetActive(active);
		m_SettingsBtn.SetActive(active);
		m_ExitBtn.SetActive(active);
	}
}
