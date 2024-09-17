using CAS.AdObject;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkipLevelAdObject : MonoBehaviour
{
    [SerializeField] private RewardedAdObject _rewardedAdObject;
    [SerializeField] private CheckPointController _checkPointControllerl;
    [SerializeField] private CharacterHealth _characterHealth;
    [SerializeField] private GameObject _rewardPanel;
    [SerializeField] private Button _showAdButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private int _maxDieCount;

    public event UnityAction OnShowAd;

    private int _dieCounter;

    private void OnEnable()
    {
        _characterHealth.OnDied += AddDieCounter;
        _showAdButton.onClick.AddListener(ShowAd);
        _closeButton.onClick.AddListener(CloseRewardPanel);
    }

    private void OnDisable()
    {
        _characterHealth.OnDied -= AddDieCounter;
        _showAdButton.onClick.RemoveListener(ShowAd);
        _closeButton.onClick.RemoveListener(CloseRewardPanel);
    }

    private void CloseRewardPanel()
    {
        _rewardPanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void AddDieCounter()
    {
        _dieCounter++;

        if (_dieCounter >= _maxDieCount)
        {
            _dieCounter = 0;
            _rewardPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void GiveReward()
    {
        _checkPointControllerl.NextCheckPoint();
        CloseRewardPanel();
        OnShowAd?.Invoke();
    }

    public void ShowAd()
    {
        _rewardedAdObject.Present();
    }
}
