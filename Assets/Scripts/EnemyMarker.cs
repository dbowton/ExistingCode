using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class EnemyMarker : Marker, INotification, INotificationOptionProvider
{
	public PropertyName id => throw new System.NotImplementedException();

	[System.Serializable]
	public struct SpawnProps
	{
		public float location;
		public GameObject prefab;
	}

	public List<SpawnProps> m_SpawnProps;

	NotificationFlags INotificationOptionProvider.flags => NotificationFlags.TriggerInEditMode;
}
