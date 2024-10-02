using Analytics;
using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectCharacterPanel : UIPanel
{
    [Header("Side components")]
    [SerializeField] private Animator _animatror;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private CharacterTypeChanger _characterTypeChanger;

    [Header("Panel components")]
    [SerializeField] private Image _nameImage;
    [SerializeField] private Image _superPowerImage;
    [SerializeField] private Image _changeButtonImage;
    [SerializeField] private Button _selectButton;
    [SerializeField] private Button _changeSkinButton;
    [SerializeField] private float _delayForClose;

    private CharacterType _selectType;
    private CharacterTypeConfigure _selectCharacter;

    public event UnityAction<CharacterType> OnSelectCharacter;

    private void Awake()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsParametrs.FirstGameStart) == 1)
        {
            Close();
        }
    }

    private void Start()
    {
        SetCharacterView();
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, Menu);
    }

    private void OnEnable()
    {
        _selectButton.onClick.AddListener(SelectCharacter);
        _changeSkinButton.onClick.AddListener(SetCharacterView);
    }

    private void OnDisable()
    {
        _selectButton.onClick.RemoveListener(SelectCharacter);
        _changeSkinButton.onClick.RemoveListener(SetCharacterView);
    }

    private void SetCharacterView()
    {
        if (_selectType == CharacterType.Man)
        {
            _selectType = CharacterType.Girl;
        }
        else
        {
            _selectType = CharacterType.Man;
        }

        DrawView();
        PlayAnimation();
    }

    private void DrawView()
    {
        foreach(var c in _characterTypeChanger.ConfiguresCharacter)
        {
            if (c.CharacterType == _selectType)
            {
                _nameImage.sprite = c.StartNameLabel;
                _superPowerImage.sprite = c.SuperPowerSprite;
                _changeButtonImage.sprite = c.NextCharacterImage;
                _skinnedMeshRenderer.sharedMesh = c.Mesh;
                _selectCharacter = c;
                return;
            }
        }
    }

    public void PlayAnimation()
    {
        _animatror.StopPlayback();

        if (_selectType == CharacterType.Man)
        {
            _animatror.SetTrigger("SelectMan");
            GameAnalytics.gameAnalytics.LogEvent("tap_animation_baddy");
        }
        else
        {
            _animatror.SetTrigger("SelectGirl");
            GameAnalytics.gameAnalytics.LogEvent("tap_animation_chica");
        }
    }

    [SerializeField][FMODUnity.ParamRef] private string aGlobalParameter; //оепедекюрэ онд юпухрейрспс

    [SerializeField] private string SceneStart;
    [SerializeField] private string Menu;

    private void SelectCharacter()
    {
        PlayAnimation();
        StartCoroutine(ClosePanel());

        if (_selectType == CharacterType.Man)
        {
            PlayerPrefs.SetInt(PlayerPrefsParametrs.IsAdsMan, 1);
            GameAnalytics.gameAnalytics.LogEvent("choose_baddy");
        }
        else if (_selectType == CharacterType.Girl)
        {
            PlayerPrefs.SetInt(PlayerPrefsParametrs.IsAdsGirl, 1);
            GameAnalytics.gameAnalytics.LogEvent("choose_chica");
        }

        // _selectCharacter.IsOpen = true;
        _characterTypeChanger.SetCharacter(_selectType);
        _characterTypeChanger.SetIsOpen(_selectType, true);
        _changeSkinButton.enabled = false;
        OnSelectCharacter?.Invoke(_selectType);

        PlayerPrefs.SetInt(PlayerPrefsParametrs.FirstGameStart, 1);
    }

    private IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(_delayForClose);

        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, SceneStart);
        Close();
    }
}