using CAS.AdObject;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RestartController : MonoBehaviour
{
    [SerializeField] private CharacterHealth _characterHealth;
    [SerializeField] private CheckPointController _checkpointController;
    [SerializeField] private InterstitialAdObject _interstitialAdObject;
    [SerializeField] private SkipLevelAdObject _adRewardManager;
    [SerializeField] private GameObject _oopsPanel;
    [SerializeField] private Button _skipLevelButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private int _maxDieCount;


    [SerializeField][FMODUnity.ParamRef] private string aGlobalParameter; //оепедекюрэ онд юпухрейрспс

    [SerializeField] private string SceneStart;
    [SerializeField] private string Menu;

    private int _dieCounter;

    public event UnityAction Restart;
    public event UnityAction SkipLevel;

    private void OnEnable()
    {
        _characterHealth.OnDied += AddDieCounter;
        _skipLevelButton.onClick.AddListener(OnSkipLevel);
        _restartButton.onClick.AddListener(OnRestart);
        //_interstitialAdObject.OnAdClosed.AddListener(OnRestart);
    }

    private void OnDisable()
    {
        _characterHealth.OnDied -= AddDieCounter;
        _skipLevelButton.onClick.RemoveListener(OnSkipLevel);
        _restartButton.onClick.RemoveListener(OnRestart);
      //  _interstitialAdObject.OnAdClosed.RemoveListener(OnRestart);
    }

    private void AddDieCounter()
    {
      //  _characterHealth.gameObject.SetActive(false);//////////////////////////
        _dieCounter++;
        _checkpointController.Restart();
        
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, Menu);

        if (_dieCounter >= _maxDieCount)
        {
            _dieCounter = 0;
            _oopsPanel.SetActive(true);
        }
    }

    private void OnRestart()
    {
       // _characterHealth.gameObject.SetActive(true);/////////////////////////////
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, SceneStart);
        Restart?.Invoke();
        _oopsPanel.SetActive(false);
        _characterHealth.Restart();
    }

    private void OnSkipLevel()
    {
       // _characterHealth.gameObject.SetActive(true);/////////////////////////////////////
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, SceneStart);
        SkipLevel?.Invoke();
        _oopsPanel.SetActive(false);
    }
}
