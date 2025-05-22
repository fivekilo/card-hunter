using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    [System.Serializable]
    public class SoundEffect
    {
        public int cardNum;
        public AudioClip playSound;
        public AudioClip specialSound;
    }
    
    public List<SoundEffect> soundEffects;
    public AudioSource cardAudioSource;
    
    void Awake()
    {
     /*   if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }*/
    }
    
    public void PlayCardPlaySound(int cardNum)
    {
        SoundEffect sfx = soundEffects.Find(s => s.cardNum == cardNum);
        if(sfx != null && sfx.playSound != null)
        {
            cardAudioSource.PlayOneShot(sfx.playSound);
        }
        else
        {
            Debug.LogWarning($"No play sound found for card: {cardNum}");
        }
    }

    public void PlayCardSpecialSound(int cardNum)
    {
        SoundEffect sfx = soundEffects.Find(s => s.cardNum == cardNum);
        if (sfx != null && sfx.specialSound != null)
        {
            cardAudioSource.PlayOneShot(sfx.specialSound);
        }
        else
        {
            Debug.LogWarning($"No play sound found for card: {cardNum}");
        }
    }
}
