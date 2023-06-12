using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "PokemonSO/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] DialogueNode[] nodes;
}
}
