using CAS.AdObject;
using UnityEngine;

public class RewardButton : MonoBehaviour
{
    [SerializeField] private RewardedAdObject _rewardedAdObject;
    //[SerializeField] private CreaterObjectButton _createrObjectButton;

    private AdRewardManager _rewardManager;

    private void Start()
    {
        _rewardedAdObject = FindAnyObjectByType<RewardedAdObject>();
        _rewardManager = FindAnyObjectByType<AdRewardManager>();
        _rewardManager.RemoveInShop(gameObject);
        _rewardedAdObject.OnReward.AddListener(OnReward);
     //   Destroy(_createrObjectButton);

        if (PlayerPrefs.GetInt("GelikIsReward") == 1)
        {
            OnReward();
        }
    }

    public void ShowAd()
    {
        _rewardedAdObject.Present();
    }

    private void OnReward()
    {
        _rewardManager.OnReward();
        _rewardedAdObject.OnReward.RemoveListener(OnReward);
        Destroy(gameObject);
        PlayerPrefs.SetInt("GelikIsReward", 1);
    }
}
