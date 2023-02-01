using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCard : MonoBehaviour
{
	[SerializeField]
	private GameObject m_HidePanel;

	[SerializeField]
	private int m_LevelIndex = 0;

	private void OnEnable()
	{
		int unlockedLevels = PlayerPrefs.GetInt("Unlocked Levels");
		m_HidePanel.SetActive(unlockedLevels < m_LevelIndex);
	}
}
