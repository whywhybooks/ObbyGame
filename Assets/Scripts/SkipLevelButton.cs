using UnityEngine;
using UnityEngine.UI;

public class SkipLevelButton : MonoBehaviour
{
    [SerializeField] private Image _targetImage;
    [SerializeField] private Sprite _normalAd;
    [SerializeField] private Sprite _pressAd;
    [SerializeField] private Sprite _normalNoAd;
    [SerializeField] private Sprite _pressNoAd;

    private void Start()
    {
        if (PlayerPrefs.GetInt("IsAdsRemove") == 0)
        {
            _targetImage.sprite = _normalAd;
        }
        else
        {
            _targetImage.sprite = _normalNoAd;
        }
    }

    public void MouseDown()
    {
        if (PlayerPrefs.GetInt("IsAdsRemove") == 0)
        {
            _targetImage.sprite = _pressAd;
        }
        else
        {
            _targetImage.sprite = _pressNoAd;
        }
    }

    public void MouseUp()
    {
        if (PlayerPrefs.GetInt("IsAdsRemove") == 0)
        {
            _targetImage.sprite = _normalAd;
        }
        else
        {
            _targetImage.sprite = _normalNoAd;
        }
    }
}
