using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenu : MonoBehaviour
{
	[Header("Menus")]
	[SerializeField]
	private GameObject m_MainMenu;

	private int m_Level = 1;

	public void OnSelectLevle(int level)
	{
		AudioManager.Get().Play("Typing");
		m_Level = level;
	}

	public void OnPlay()
	{
		#if !UNITY_EDITOR
				if (m_Level - 1 > PlayerPrefs.GetInt("Unlocked Levels")) return;
		#endif

		GameManager.Get().currentLevel = m_Level;

		if(SceneManager.GetSceneByName("Level"+m_Level) != null)
			SceneManager.LoadScene("Level"+m_Level);
	}

	public void OnBack()
	{
		RetroEffect.Get().StartFlicker();
		gameObject.SetActive(false);
		m_MainMenu.SetActive(true);
	}
}
