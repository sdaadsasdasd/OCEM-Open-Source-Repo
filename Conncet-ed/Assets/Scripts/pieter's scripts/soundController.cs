using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundController : MonoBehaviour
{
    public AudioClip daySound;
    public AudioClip nightSound;
    public AudioSource daySoundSource;
    public AudioSource nightSoundSource;
    public sunRotation time;

    [Header("volume curves")]
    public AnimationCurve dayCurve;
    public AnimationCurve nightCurve;

    private void Start()
    {
        daySoundSource.clip = daySound;
        daySoundSource.Play();
        nightSoundSource.clip = nightSound;
        nightSoundSource.Play();
    }

    private void Update()
    {
        daySoundSource.volume = dayCurve.Evaluate(time.time);
        nightSoundSource.volume = nightCurve.Evaluate(time.time);
    }
}
