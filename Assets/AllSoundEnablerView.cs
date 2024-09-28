using UnityEngine;
using UnityEngine.UI;

public class AllSoundEnablerView : MonoBehaviour
{
    [SerializeField] private AllSoundEnabler _allSoundEnabler;
    [SerializeField] private Image _targetImage;
    [SerializeField] private Sprite _enableSprite;
    [SerializeField] private Sprite _disableSprite;

    private void OnEnable()
    {
        _allSoundEnabler.SwitchMusic += ChangeSprite;
    }

    private void OnDisable()
    {
        _allSoundEnabler.SwitchMusic -= ChangeSprite;
    }

    private void ChangeSprite(bool isActive)
    {
        if (isActive)
        {
            _targetImage.sprite = _enableSprite;
        }
        else
        {
            _targetImage.sprite = _disableSprite;
        }
    }
}