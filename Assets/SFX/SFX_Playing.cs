using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Playing : MonoBehaviour
{
    public AudioSource damage;
    public AudioSource bleed;
    public AudioSource heal;
    public AudioSource music;
    
    public void PlayDamage()
    {
        damage.Play();
    }
    
    public void PlayBleed()
    {
        bleed.Play();
    }
    
    public void PlayHeal()
    {
        heal.Play();
    }
    
    public void PlayMusic()
    {
        music.Play();
    }
}

