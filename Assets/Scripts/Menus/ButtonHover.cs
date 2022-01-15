using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

	private Image m_Image;
	private TextMeshProUGUI m_Text;


	private void Start()
	{
		m_Image = GetComponent<Image>();
		m_Text = GetComponentInChildren<TextMeshProUGUI>();

		SetColors(0, 1);
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		SetColors(1, 0);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		SetColors(0, 1);
	}

	private void SetColors(float ba, float tc)
	{
		Color backgroundColor = m_Image.color;
		backgroundColor.a = ba;
		m_Image.color = backgroundColor;

		m_Text.color = new Color(tc, tc, tc, 1);
	}

	
}
