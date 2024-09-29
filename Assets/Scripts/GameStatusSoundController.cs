using FMODUnity;
using UnityEngine;

public class GameStatusSoundController : MonoBehaviour
{
    [SerializeField] private RestartController _restartController;
    [SerializeField] private InterstitialController _interstitialController;

    [SerializeField][FMODUnity.ParamRef] private string aGlobalParameter;

    [SerializeField] private string SceneStart;
    [SerializeField] private string Menu;

    private void OnEnable() //≈—À» »√–¿ ”∆≈ ¡€À¿ «¿œ”Ÿ≈Õ¿, “Œ ¬ Àﬁ◊¿≈Ã «¬” 
    {
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, Menu);

        _interstitialController.BreakPanelActivate += SetMenu;
        _restartController.SkipPanelActivate += SetMenu;

        _interstitialController.AdClosed += SetSceneStart;
        _restartController.SkipLevel += SetSceneStart;
        _restartController.Restart += SetSceneStart;
    }

    private void OnDisable()
    {
        _interstitialController.BreakPanelActivate -= SetMenu;
        _restartController.SkipPanelActivate -= SetMenu;

        _interstitialController.AdClosed -= SetSceneStart;
        _restartController.SkipLevel -= SetSceneStart;
        _restartController.Restart -= SetSceneStart;
    }

    private void SetSceneStart()
    {
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, SceneStart);
    }

    private void SetMenu()
    {
        RuntimeManager.StudioSystem.setParameterByNameWithLabel(aGlobalParameter, Menu);
    }
}
