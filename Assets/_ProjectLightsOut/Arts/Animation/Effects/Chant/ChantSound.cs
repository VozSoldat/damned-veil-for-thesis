using System.Collections;
using System.Collections.Generic;
using ProjectLightsOut.DevUtils;
using ProjectLightsOut.Managers;
using UnityEngine;

public class ChantSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    public void PlayChantSound()
    {
        audioSource.Play();
    }
}
