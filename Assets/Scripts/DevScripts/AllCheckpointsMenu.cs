using System;
using UnityEngine;
using UnityEngine.UI;

public class AllCheckpointsMenu : MonoBehaviour
{
    [SerializeField] private Transform _buttonContainer;
    [SerializeField] private SelectCheckPointButton _buttonPrefab;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _openButton;
    [SerializeField] private CanvasGroup _thisPanel;

    private bool _isOpen;

    private CheckPointController _checkPointController;

    private void OnEnable()
    {
        _openButton.onClick.AddListener(Open);
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(Open);
        _closeButton.onClick.RemoveListener(Close);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_isOpen)
            {
                _isOpen = false;
                Close();
            }
            else
            {
                _isOpen = true;
                Open();
            }
        }
    }

    private void Close()
    {
        _thisPanel.Deactivate();
    }

    private void Open()
    {
        _thisPanel.Activate();
    }

    private void Start()
    {
        _thisPanel.Deactivate();
        _checkPointController = FindObjectOfType<CheckPointController>();

        for (int i = 0; i < _checkPointController.CheckPoints.Count; i++)
        {
            SelectCheckPointButton selectCheckPointButton = Instantiate(_buttonPrefab, _buttonContainer);
            selectCheckPointButton.Initialized(i);
            selectCheckPointButton.OnClick += SetCheckpoint;
        }
    }

    private void SetCheckpoint(int index)
    {
        _checkPointController.SetCheckpointOnIndex(index);
    }
}