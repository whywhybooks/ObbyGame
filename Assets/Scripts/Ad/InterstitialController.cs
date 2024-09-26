using CAS.AdObject;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InterstitialController : MonoBehaviour
{
    [SerializeField] private InterstitialAdObject _interstitialAdObject;
    [SerializeField] private CheckPointController _checkPointController;
    [SerializeField] private float _coffeBreak;
    [SerializeField] private SkipLevelAdObject _skipLevelAdObject;
    [SerializeField] private GameObject _breakPanel;
    [SerializeField] private TMP_Text _timerText;

    [SerializeField] private TMP_Text _text; //������� �����

    private float _coffeBreakElapsedTime;
    private bool _inGame;
    private bool _isPosible;
    public event UnityAction OnShowAd;

    private void OnEnable()
    {
        _interstitialAdObject.OnAdClosed.AddListener(AdClosedHandler);
        _interstitialAdObject.OnAdShown.AddListener(AdShowHandler);
        _skipLevelAdObject.OnShowAd += ResetTimer;
        _checkPointController.OnActiveCheckpoint += ShowAd;
        _checkPointController.OnReachedUnlockLevel += GivePosible;
    }

    private void OnDisable()
    {
        _interstitialAdObject.OnAdClosed.RemoveListener(AdClosedHandler);
        _interstitialAdObject.OnAdShown.RemoveListener(AdShowHandler);
        _skipLevelAdObject.OnShowAd -= ResetTimer;
        _checkPointController.OnActiveCheckpoint -= ShowAd;
        _checkPointController.OnReachedUnlockLevel -= GivePosible;
    }

    void Update()
    {
       // if (PlayerPrefs.GetInt("IsAdsRemove") == 1) //������� �� ���� ������� ���������
      //      return;

      /*  if (_inGame == false)
            return;*/

        if (_isPosible)
            _coffeBreakElapsedTime += Time.deltaTime;

        if (_coffeBreakElapsedTime > _coffeBreak)
            _text.text = $"����� �����!";
        else
            _text.text = $"����� ������: {Mathf.Round(_coffeBreakElapsedTime)}";
    }

    private void GivePosible()
    {
        _isPosible = true;
    }

    private void ShowAd()
    {
        if (PlayerPrefs.GetInt("IsAdsRemove") == 0)
        {
            if (_coffeBreakElapsedTime >= _coffeBreak)
            {
                StartCoroutine(StartCountdown());
                _coffeBreakElapsedTime = 0;
            }
        }
    }

    private IEnumerator StartCountdown()
    {
        _isPosible = false;
        _breakPanel.SetActive(true);
        _timerText.text = "3";

        yield return new WaitForSeconds(1);

        _timerText.text = "2";

        yield return new WaitForSeconds(1);

        _timerText.text = "1";

        yield return new WaitForSeconds(1);

        _breakPanel.SetActive(false);
        _interstitialAdObject.Present();
        OnShowAd?.Invoke();
        _isPosible = true;
    }

    private void ResetTimer()
    {
        _coffeBreakElapsedTime = 0;
    }

    private void AdClosedHandler()
    {
      //  Time.timeScale = 1;
    }

    private void AdShowHandler()
    {
      //  Time.timeScale = 0;
    }
}
