using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DiaolgueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] string[] dialogue;
    [SerializeField] Response[] responses;

    public string[] Dialogue => dialogue;

    public Response[] Responses => responses;

    public bool HasResponses => Responses != null && Responses.Length > 0;
}
