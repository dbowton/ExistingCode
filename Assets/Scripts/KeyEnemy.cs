using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEnemy : MonoBehaviour
{
    [SerializeField] List<GameObject> connectedBlocks;

    public void Activate()
    {
        foreach (GameObject block in connectedBlocks)
        {
            Destroy(block);
        }
        Destroy(gameObject);
    }
}
