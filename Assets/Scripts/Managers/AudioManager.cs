using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    private const string MASTER_VOLUME = "MasterVolume";
    private const string BGM_VOLUME = "BGMVolume";
    private const string SFX_VOLUME = "SFXVolume";

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private PairList<BGMType, AudioClip> _bgmClipList;
    [SerializeField] private PairList<SFXType, AudioClip> _sfxClipList;
    [SerializeField] private int _maxSameSFXCount = 3;

    private Dictionary<BGMType, AudioClip> _bgmClips;
    private Dictionary<SFXType, AudioClip> _sfxClips;
    private Dictionary<SFXType, int> _sfxCounts = new();
    private Coroutine _fadeVolumeCoroutine;
    private float _sfxPitchBias = 0f;

    protected override void Awake()
    {
        base.Awake();

        InitDict();
    }

    private void InitDict()
    {
        _bgmClips = _bgmClipList.GetDict();
        _sfxClips = _sfxClipList.GetDict();
    }

    private void Start()
    {
        Init();
        RegisterEvents();
    }


    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void Init()
    {
        var data = OptionManager.Instance.OptionData;

        OnOptionValueChanged(OptionType.MasterVolume, data.masterVolume);
        OnOptionValueChanged(OptionType.BGMVolume, data.bgmVolume);
        OnOptionValueChanged(OptionType.SFXVolume, data.sfxVolume);
    }

    #region Events
    private void RegisterEvents()
    {
        OptionUIEvents.OnOptionValueChanged += OnOptionValueChanged;
    }

    private void UnregisterEvents()
    {
        OptionUIEvents.OnOptionValueChanged -= OnOptionValueChanged;
    }

    private void OnOptionValueChanged(OptionType type, int value)
    {
        float db = VolumeToDecibel(value);

        switch (type)
        {
            case OptionType.MasterVolume:
                _audioMixer.SetFloat(MASTER_VOLUME, db);
                break;
            case OptionType.BGMVolume:
                _audioMixer.SetFloat(BGM_VOLUME, db);
                break;
            case OptionType.SFXVolume:
                _audioMixer.SetFloat(SFX_VOLUME, db);
                break;
        }
    }

    private float VolumeToDecibel(int volume)
    {
        float normalized = Mathf.Clamp01(volume / 10f);
        if (normalized <= 0.0001f) return -80f;
        return Mathf.Log10(normalized) * 20f;
    }
    #endregion

    public void PlayBGM(BGMType type)
    {
        if (_bgmClips.TryGetValue(type, out var clip))
        {
            _bgmSource.clip = clip;
            _bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"There is no BGM For : {type}");
        }
    }

    public void PlaySFX(SFXType type, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        if (_sfxClips.TryGetValue(type, out var clip))
        {
            if (!_sfxCounts.ContainsKey(type)) _sfxCounts[type] = 0;
            if (_sfxCounts[type] > _maxSameSFXCount) return;
            _sfxCounts[type]++;

            _sfxSource.pitch = Random.Range(minPitch, maxPitch) + _sfxPitchBias;
            _sfxSource.PlayOneShot(clip);
            _sfxSource.pitch = 1f;

            StartCoroutine(DecreaseSFXCount(type, clip.length));
        }
        else
        {
            Debug.LogWarning($"There is no SFX For : {type}");
        }
    }

    private IEnumerator DecreaseSFXCount(SFXType type, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (_sfxCounts.ContainsKey(type))
        {
            _sfxCounts[type] = Mathf.Max(0, _sfxCounts[type] - 1);
        }
    }

    #region ChangeVolume
    private IEnumerator FadeVolumeCoroutine(float targetVolume, float duration)
    {
        float start = _bgmSource.volume;
        float end = Mathf.Clamp01(targetVolume);

        if (duration > 0.0f)
        {
            float time = 0f;
            while (time < duration)
            {
                float t = time / duration;
                float cur = Mathf.Lerp(start, end, t);
                _bgmSource.volume = cur;
                _sfxSource.volume = cur;

                time += Time.deltaTime;
                yield return null;
            }
        }

        _bgmSource.volume = end;
        _sfxSource.volume = end;
    }

    public void StartFadeVolume(float targetVolume, float duration)
    {
        StopFadeVolume();
        _fadeVolumeCoroutine = StartCoroutine(FadeVolumeCoroutine(targetVolume, duration));
    }

    private void StopFadeVolume()
    {
        if (_fadeVolumeCoroutine != null)
        {
            StopCoroutine(_fadeVolumeCoroutine);
            _fadeVolumeCoroutine = null;
        }
    }
    #endregion

    public void SetSFXPitchBias(float value)
    {
        _sfxPitchBias = value;
    }
}

public enum BGMType
{
    MainMenuScene,
    GameScene,
}

public enum SFXType
{
    ButtonDown,
    ButtonUp,
    DiceRoll,
    DiceCollide,
    Win,
    Lose,
    RoundClear,
    Error,
    DiceClick,
    DiceTrigger,
    Money,
    DiceGenerate,
    DiceRemove
}