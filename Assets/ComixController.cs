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
        StartCoroutine(ShowComixCoroutine(_startParts));
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


        _thisPanel.Deactivate();
    }
}