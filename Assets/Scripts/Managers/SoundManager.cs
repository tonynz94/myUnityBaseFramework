using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    //AudioSource : 시디플레이어
    //AudioClip   : 시디
    private AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.Max];
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    private GameObject _soundRoot = null;
    public void Init()
    {
        if (_soundRoot == null)
        {
            _soundRoot = GameObject.Find("@SoundRoot");
            if (_soundRoot == null)
            {
                _soundRoot = new GameObject() { name = "@SoundRoot" };
                Object.DontDestroyOnLoad(_soundRoot);

                string[] soundTypeNames = System.Enum.GetNames(typeof(Define.Sound));
                for (int count = 0; count < soundTypeNames.Length - 1; count++)
                {
                    GameObject go = new GameObject() { name = soundTypeNames[count] };
                    _audioSources[count] = go.AddComponent<AudioSource>();
                    go.transform.parent = _soundRoot.transform;
                }

                _audioSources[(int)Define.Sound.Bgm].loop = true;
            }
        }
    }
    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
            audioSource.Stop();

        _audioClips.Clear();
    }
    public void SetPetch(Define.Sound type, float pitch = 1.0f)
    {
        _audioSources[(int)type].pitch = pitch;
    }
    public bool Play(Define.Sound type, string path, float volume = 1.0f, float pitch = 1.0f)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        AudioSource audioSource = _audioSources[(int)type];
        if (!path.Contains("Sound/"))
            path = string.Format("Sound/{0}", path);

        if (audioSource == null)
            return false;

        audioSource.volume = volume;
        audioSource.pitch = pitch;

        if (type == Define.Sound.Bgm)
        {
            AudioClip audioClip = Managers.Resource.Load<AudioClip>(path);
            if (audioClip == null)
                return false;

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.Play();
            return true;
        }
        else if (type == Define.Sound.Effect)
        {
            AudioClip audioClip = GetAudioClip(path);
            if (audioClip == null)
                return false;

            audioSource.PlayOneShot(audioClip);
            return true;
        }

        return false;
    }
    public void Stop(Define.Sound type)
    {
        if (_audioSources[(int)type].isPlaying)
            _audioSources[(int)type].Stop();
    }
    public float GetAudioClipLength(string path)
    {
        AudioClip audioClip = GetAudioClip(path);
        if (audioClip == null)
            return 0.0f;

        return audioClip.length;
    }
    public AudioClip GetAudioClip(string path)
    {
        AudioClip audioClip = null;
        _audioClips.TryGetValue(path, out audioClip);

        if (audioClip == null)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
            _audioClips.Add(path, audioClip);
        }

        return audioClip;
    }
}
