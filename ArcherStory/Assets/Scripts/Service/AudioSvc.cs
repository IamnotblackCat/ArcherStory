/****************************************************
    文件：AudioSvc.cs
	作者：Echo
    邮箱: 350383921@qq.com
    日期：2019/10/13 11:19:15
	功能：声音播放服务
*****************************************************/

using UnityEngine;

public class AudioSvc : MonoBehaviour 
{
    public static AudioSvc instance = null;
    public AudioSource bgAudio;
    public AudioSource uiAudio;
    
    public void InitSvc()
    {
        instance = this;

    }
    public void StopBGMusic()
    {
        if (bgAudio!=null)
        {
            bgAudio.Stop();
        }
    }
    public void PlayBGMusic(string name, bool isLoop = true)
    {
        AudioClip audio = ResSvc.instance.LoadAudio("Audio/ArcherAudio/"+name,true);
       
        if (bgAudio.clip==null||bgAudio.clip!=audio)
        {
            bgAudio.clip = audio;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }
    public void PlayUIAudio(string name)
    {
        AudioClip audio = ResSvc.instance.LoadAudio("Audio/ArcherAudio/" + name,true);
        uiAudio.clip = audio;
        uiAudio.Play();
    }
    public void PlayCharacterAudio(string name,AudioSource audioSource)
    {
        AudioClip audio = ResSvc.instance.LoadAudio("Audio/ArcherAudio/" + name,true);
        audioSource.clip = audio;
        audioSource.Play();
    }
}