using Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComixController : MonoBehaviour
{
    [SerializeField] private FinalTrigger _finalTrigger;
    [SerializeField] private CanvasGroup _thisPanel;
    [SerializeField] private List<Image> _images = new List<Image>();
    [SerializeField] private List<Sprite> _startParts = new List<Sprite>();
    [SerializeField] private List<Sprite> _finalParts = new List<Sprite>();
    [SerializeField] private float _delay;

    private void OnEnable()
    {
        _finalTrigger.IsActive += ShowComix;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsParametrs.FirstGameStart) == 1)
        {
            _thisPanel.Deactivate();
            return;
        }

        StartCoroutine(ShowComixCoroutine(_startParts));
        StartCoroutine(StartEvent());
    }

    private void OnDisable()
    {
        _finalTrigger.IsActive -= ShowComix;
    }

    private void ShowComix()
    {
        StartCoroutine(ShowComixCoroutine(_finalParts));
    }

    private IEnumerator ShowComixCoroutine(List<Sprite> sprites)
    {
        _thisPanel.Activate();

        for (int i = 0; i < _images.Count; i++)
        {
            _images[i].gameObject.SetActive(false);
        }

        yield return new WaitForSeconds(_delay / 2);

        for (int i = 0; i < sprites.Count; i++) 
        {
            _images[i].gameObject.SetActive(true);
            _images[i].sprite = sprites[i];

            yield return new WaitForSeconds(_delay);
        }

        yield return new WaitForSeconds(2f);

        _thisPanel.Deactivate();
    }

    private IEnumerator StartEvent()
    {
        yield return new WaitForSeconds(_delay * 3 + _delay / 2 + 2);
        GameAnalytics.gameAnalytics.LogEvent("open_game_start");
    }
}