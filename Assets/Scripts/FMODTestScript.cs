using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODTestScript : MonoBehaviour
{
    [SerializeField] [FMODUnity.EventRef]  private string aSound;


    float a = 1;

    private void Update()
    {

        a -= Time.deltaTime;
        if (a < 0)
        {
            PlayerStep();
                a = 2;
        }
    }


    public void PlayerStep()
    {
        FMODUnity.RuntimeManager.PlayOneShot(aSound);
    }
}

