using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectCheckPointButton : MonoBehaviour
{
    [SerializeField] private Button _thisButton;
    [SerializeField] private TMP_Text _numberView;

    private int _index;

    public event Action<int> OnClick;

    private void OnEnable()
    {
        _thisButton.onClick.AddListener(Click);
    }

    private void OnDisable()
    {
        _thisButton?.onClick.RemoveListener(Click);
    }

    public void Initialized(int index)
    {
        _index = index;
        _numberView.text = (index + 1).ToString();
    }

    private void Click()
    {
        OnClick?.Invoke(_index);
    }
}
