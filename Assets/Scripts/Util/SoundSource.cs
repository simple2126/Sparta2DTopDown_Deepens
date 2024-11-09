using Unity.VisualScripting;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    private AudioSource audioSource;

    public void Play(AudioClip clip, float soundEffectVolume, float soundEffectPitchVaricance)
    {
        if(audioSource == null)
            audioSource = GetComponent<AudioSource>();

        CancelInvoke();
        audioSource.clip = clip;
        audioSource.volume = soundEffectVolume;
        audioSource.pitch = 1f * Random.Range(-soundEffectVolume, soundEffectPitchVaricance); // 다양한 음향 효과
        audioSource.Play();

        Invoke("Disable", clip.length * 2); // clip.length -> 오디오 클립의 재생시간
    }

    public void Disable()
    {
        audioSource.Stop();
        gameObject.SetActive(false);
    }
}