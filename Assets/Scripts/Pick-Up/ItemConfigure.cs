using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Create new pick-up item")]
public class ItemConfigure : ScriptableObject
{
    [field: SerializeField] public Sprite _icon;
    [field: SerializeField] public string _label;
}
