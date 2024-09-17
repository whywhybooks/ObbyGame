using CAS.AdObject;
using UnityEngine;

public class AdRewardManager : MonoBehaviour
{
    [SerializeField] private RewardedAdObject _rewardedAdObject;
  //  [SerializeField] private ButtonsSpawner _characterButtonSpawner;
   //[SerializeField] private ConfigureButton _gelik;

    public void ShowAd()
    {
        _rewardedAdObject.Present();
    }

    public void OnReward()
    {
       // _characterButtonSpawner.AddButton(_gelik);
        Debug.Log("The user earned the reward.");
    }

    public void RemoveInShop(GameObject gelik)
    {
       // _characterButtonSpawner.RemoveButton(gelik.GetComponent<CreaterObjectButton>());
    }
}
