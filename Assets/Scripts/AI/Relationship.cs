
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Windows;

public class Relationship : MonoBehaviour
{
    public AIBehaviour aib;
    public GameObject slider;
    public float sliderMinPos;
    public float sliderMaxPos;
    public GameObject bar;
    public float barMinRot;
    public float barMaxRot;
    public OutlineFx.OutlineFxFeature ofx;
    public float outlineFillMax;
    public Volume volume;
    public float vignetteMin;
    public float vignetteMax;
    public Color hueColor;
    public float grainMin;
    public float grainMax;
    public AudioSource noise;
    public float noiseMax;
    public AudioSource bgMusic;
    public float bgMusicPitchMax;
    void Update()
    {
        float percentage = aib.happyPoints + 50;
        slider.transform.localPosition = new Vector3((percentage * (sliderMaxPos - sliderMinPos) / 100) + sliderMinPos, 0, 0);
        bar.transform.localRotation = Quaternion.Euler(0, 0, ((percentage * (barMaxRot - barMinRot) / 100) + barMinRot)*-1);

        float effectPercentage = 0;
        if (percentage < 50)
        {
            effectPercentage = 100 - (percentage * 2);
        }
        ofx.Solid = effectPercentage * outlineFillMax / 100;
        Vignette vignette;
        volume.profile.TryGet(out vignette);
        vignette.color.value = Color.Lerp(Color.black, Color.red, effectPercentage/100);
        vignette.intensity.value = (effectPercentage * (vignetteMax - vignetteMin) / 100) + vignetteMin;
        ColorAdjustments ca;
        volume.profile.TryGet(out ca);
        ca.colorFilter.value = Color.Lerp(Color.white, hueColor, effectPercentage/100);
        FilmGrain fg;
        volume.profile.TryGet(out fg);
        fg.intensity.value = (effectPercentage * (grainMax - grainMin) / 100) + grainMin;
        bgMusic.pitch = bgMusicPitchMax - ((effectPercentage * (bgMusicPitchMax - 1) / 100) + 1);
        noise.volume = (effectPercentage * noiseMax / 100);
    }
}
