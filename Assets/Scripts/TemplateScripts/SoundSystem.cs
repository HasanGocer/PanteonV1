using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSystem : MonoSingleton<SoundSystem>
{
    [SerializeField] private AudioSource _mainSource;
    [SerializeField] private AudioClip _buildAbandoned, _buildHit, _buildPlacement, _coin, _Sword, _warTime;
    [SerializeField] Scrollbar _effectMusicBar;


    public void MainMusicPlay()
    {
        _mainSource.Play();
    }

    public void MainMusicStop()
    {
        _mainSource.Stop();
    }
    public void CallBuildAbandoned()
    {
        _mainSource.PlayOneShot(_buildAbandoned);
    }
    public void CallBuildHit()
    {
        _mainSource.PlayOneShot(_buildHit);
    }
    public void CallBuildPlacement()
    {
        _mainSource.PlayOneShot(_buildPlacement);
    }
    public void CallCoin()
    {
        _mainSource.PlayOneShot(_coin);
    }
    public void CallSword()
    {
        _mainSource.PlayOneShot(_Sword);
    }
    public void CallWarTime()
    {
        _mainSource.PlayOneShot(_warTime);
    }

    public float GetEffectMusicBar()
    {
        return _effectMusicBar.value;
    }
    public void SetEffectMusicBar(float value)
    {
        _effectMusicBar.value = value;
    }

    public void EffectMusicPlacement()
    {
        PlayerPrefs.SetInt("sound", (int)(_effectMusicBar.value * 100));
        _mainSource.volume = (int)(_effectMusicBar.value * 100);
    }
    public float GetEffect()
    {
        float tempFloat = 1;

        if (PlayerPrefs.HasKey("sound"))
        {
            tempFloat = PlayerPrefs.GetInt("sound", (int)(_effectMusicBar.value * 100));
            tempFloat /= 100;
        }
        _mainSource.volume = tempFloat;

        return tempFloat;
    }
}
