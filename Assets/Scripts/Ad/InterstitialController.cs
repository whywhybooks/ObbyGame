using CAS.AdObject;
using UnityEngine;

public class InterstitialController : MonoBehaviour
{
    [SerializeField] private InterstitialAdObject _interstitialAdObject;
    [SerializeField] private float _coffeBreak;
    [SerializeField] private SkipLevelAdObject _skipLevelAdObject;

    private float _coffeBreakElapsedTime;
    private bool _inGame;

    private void OnEnable()
    {
        _interstitialAdObject.OnAdClosed.AddListener(AdClosedHandler);
        _interstitialAdObject.OnAdShown.AddListener(AdShowHandler);
        _skipLevelAdObject.OnShowAd += ResetTimer;
    }

    private void OnDisable()
    {
        _interstitialAdObject.OnAdClosed.RemoveListener(AdClosedHandler);
        _interstitialAdObject.OnAdShown.RemoveListener(AdShowHandler);
        _skipLevelAdObject.OnShowAd -= ResetTimer;
    }

    void Update()
    {
       // if (PlayerPrefs.GetInt("IsAdsRemove") == 1) //Условие на если реклама отключена
      //      return;

      /*  if (_inGame == false)
            return;*/

        _coffeBreakElapsedTime += Time.deltaTime;

        if (_coffeBreakElapsedTime >= _coffeBreak)
        {
            _interstitialAdObject.Present();
            _coffeBreakElapsedTime = 0;
        }
    }

    private void ResetTimer()
    {
        _coffeBreakElapsedTime = 0;
    }

    private void AdClosedHandler()
    {
        Time.timeScale = 1;
    }

    private void AdShowHandler()
    {
        Time.timeScale = 0;
    }
}
