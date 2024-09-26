using CAS.AdObject;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
            _text.text = $"����� �����!";
        else
            _text.text = $"����� ������: {Mathf.Round(_elapsedTime)}";
    }

    public void ShowInterstitial()
    {
        //  Time.timeScale = 1;
        if (PlayerPrefs.GetInt("IsAdsRemove") == 0)
        {
            if (_elapsedTime >= _delay)
            {
                Debug.Log(234);
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
        _checkPointControllerl.NextCheckPoint();
        _elapsedTime = 0;
    }

    public void ShowAdForReward()
    {
        if (PlayerPrefs.GetInt("IsAdsRemove") == 0)
            _rewardedAdObject.Present();
        else
            GiveReward();
    }
}
