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
        audioSource.pitch = 1f * Random.Range(-soundEffectVolume, soundEffectPitchVaricance); // �پ��� ���� ȿ��
        audioSource.Play();

        Invoke("Disable", clip.length * 2); // clip.length -> ����� Ŭ���� ����ð�
    }

    public void Disable()
    {
        audioSource.Stop();
        gameObject.SetActive(false);
    }
}