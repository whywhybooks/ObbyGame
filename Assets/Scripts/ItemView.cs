using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _countText;
}

public class Item : MonoBehaviour
{
    public bool Found { get; private set; }
}

public class Quest : MonoBehaviour
{
    [SerializeField] private Sprite _iconItem;
    [SerializeField] private List<Item> _items = new List<Item>();
}
