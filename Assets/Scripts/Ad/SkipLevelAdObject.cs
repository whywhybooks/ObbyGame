using Analytics;
using CAS.AdObject;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SkipLevelAdObject : MonoBehaviour
{
    [Header("Restart:")]
    [SerializeField] private float _delay;
    [SerializeField] private TMP_Text _text;

    private float _elapsedTime;

    [Header("Skip:")]
    [SerializeField] private RewardedAdObject _rewardedAdObject;
    [SerializeField] private InterstitialAdObject _interstitialAdObject;
    [SerializeField] private InterstitialController _interstitialController;
    [SerializeField] private CheckPointController _checkPointControllerl;
    [SerializeField] private RestartController _restartControllerl;

    public event UnityAction OnShowAd;

    private void OnEnable()
    {
        _restartControllerl.SkipLevel += ShowAdForReward;
        _restartControllerl.Restart += ShowInterstitial;
        _rewardedAdObject.OnReward.AddListener(GiveReward);
        _interstitialController.OnShowAd += ResetTimer;
    }

    private void OnDisable()
    {
        _restartControllerl.SkipLevel -= ShowAdForReward;
        _restartControllerl.Restart -= ShowInterstitial;
        _rewardedAdObject.OnReward.RemoveListener(GiveReward);
        _interstitialController.OnShowAd -= ResetTimer;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _delay)
            _text.text = $"Стишл готов!";
        else
            _text.text = $"Время стишла: {Mathf.Round(_elapsedTime)}";
    }

    public void ShowInterstitial()
    {
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

    private void ResetTimer()
    {
        _elapsedTime = 0;
    }

    public void GiveReward()
    {
        GameAnalytics.gameAnalytics.LogEvent($"skip_point_{_checkPointControllerl.CurrentCheckPointIndex}");
        _checkPointControllerl.NextCheckPoint();
        _elapsedTime = 0;
    }

    public void ShowAdForReward()
    {
        if (PlayerPrefs.GetInt("IsAdsRemove") == 0)
        {
            _rewardedAdObject.Present();
        }
        else
            GiveReward();
    }
}
