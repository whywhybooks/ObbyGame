using CAS.AdObject;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkipLevelAdObject : MonoBehaviour
{
    [Header("Skip:")]
    [SerializeField] private float _delay;

    private float _elapsedTime;

    [Header("Skip:")]
    [SerializeField] private RewardedAdObject _rewardedAdObject;
    [SerializeField] private InterstitialAdObject _interstitialAdObject;
    [SerializeField] private CheckPointController _checkPointControllerl;
    [SerializeField] private CharacterHealth _characterHealth;
    [SerializeField] private GameObject _rewardPanel;
    [SerializeField] private Button _showAdButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private int _maxDieCount;

    public event UnityAction OnShowAd;

    private int _dieCounter;

    public FMOD.Studio.EventInstance theBusASoundPassesThrough;

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

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
    }

    private void CloseRewardPanel()
    {
        _rewardPanel.SetActive(false);
        Time.timeScale = 1;

        if (PlayerPrefs.GetInt("IsAdsRemove") == 0)
        {
            if (_elapsedTime >= _delay)
            {
                _interstitialAdObject.Present();
                OnShowAd?.Invoke();
                _elapsedTime = 0;
            }
        }
    }

    private void AddDieCounter()
    {
        _dieCounter++;

        if (_dieCounter >= _maxDieCount)
        {
            _dieCounter = 0;
            _rewardPanel.SetActive(true);
            theBusASoundPassesThrough.setPaused(true);
          //  theBusASoundPassesThrough.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            // FMOD.Studio.EventInstance.setPaused(false);
            //  Studio.EventInstance.setPaused(false);
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
        Time.timeScale = 1;
        _rewardPanel.SetActive(false);

        if (PlayerPrefs.GetInt("IsAdsRemove") == 0)
            _rewardedAdObject.Present();
        else
            GiveReward();
    }
}
