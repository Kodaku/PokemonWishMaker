using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DiaolgueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] string[] dialogue;

    public string[] Dialogue => dialogue;
}
