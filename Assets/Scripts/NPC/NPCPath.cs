using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCPath
{
    [SerializeField] GameObject[] nodes;

    public GameObject[] Nodes => nodes;
}
