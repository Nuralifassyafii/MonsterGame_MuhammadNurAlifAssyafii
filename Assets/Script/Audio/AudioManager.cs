using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class AudioClass
{
    public string audioName;
    public AudioSource _audio;
}
public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClass> listAudio; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayAudio(string audioName)
    {
        listAudio.Find(item => item.audioName == audioName)._audio.Play();
    }

    public void StopAudio(string audioName)
    {
        listAudio.Find(item => item.audioName == audioName)._audio.Stop();
    }

    public void StopAllAudio()
    {
        for(int i =0; i < listAudio.Count; i++)
        {
            listAudio[i]._audio.Stop();
        }
    }
}
