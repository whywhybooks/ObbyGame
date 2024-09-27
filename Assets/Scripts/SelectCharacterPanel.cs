using FMODUnity;
using System;
using System.Collections;
using UnityEngine;
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

    private void Awake()
    {
      /*  if (PlayerPrefs.HasKey("FirstStart") == false) //Проверка на первый запуск
        {
            PlayerPrefs.SetInt("FirstStart", 1);
        }
        else
        {
            Close();
        }*/
    }

    private void Start()
    {
        _selectType = CharacterType.Man;
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
        }
        else
        {
            _animatror.SetTrigger("SelectGirl");
        }
    }

    [SerializeField][FMODUnity.ParamRef] private string aGlobalParameter;

    [SerializeField] private string SceneStart;
    [SerializeField] private string Menu;

    private void SelectCharacter()
    {
        PlayAnimation();
        StartCoroutine(ClosePanel());

        _selectCharacter.IsOpen = true;
        _characterTypeChanger.SetCharacter(_selectType);
        _characterTypeChanger.SetIsOpen(_selectType, true);
        _changeSkinButton.enabled = false;
    }

    private IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(_delayForClose);

        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, SceneStart);
        Close();
    }
}