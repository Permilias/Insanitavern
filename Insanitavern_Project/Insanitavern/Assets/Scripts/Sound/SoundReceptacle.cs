using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReceptacle : MonoBehaviour
{
    public AudioSource source;

    public void PlaySound(Sound _sound, bool loops = false, bool bypassAudioListener = false)
    {
        StartCoroutine(PlaySoundCoroutine(_sound, loops, false, Vector3.zero, 0f, 500f, 0f, bypassAudioListener));
    }

    public AudioSource PlaySoundReturnSource(Sound _sound, bool loops = false, bool bypassAudioListener = false)
    {
        StartCoroutine(PlaySoundCoroutine(_sound, loops, false, Vector3.zero, 0f, 500f, 0f, bypassAudioListener));
        return source;
    }

    public AudioSource PlaySoundAtPosition(Sound _sound, bool loops, bool hasPos, Vector3 pos, float spatialBlend, float maximumDistance, float minimumDistance = 5f, bool bypassAudioListener = false)
    {
        StartCoroutine(PlaySoundCoroutine(_sound, loops, true, pos, spatialBlend, maximumDistance, minimumDistance, bypassAudioListener));
        return source;
    }

    public IEnumerator PlaySoundCoroutine(Sound _sound, bool loops, bool hasPos, Vector3 pos, float spatialBlend, float maximumDistance, float minimumDistance = 5f, bool bypassAudioListener = false)
    {
        source.enabled = true;
        source.clip = _sound.clips[Random.Range(0, _sound.clips.Count)];
        source.volume = Random.Range(_sound.minVolume, _sound.maxVolume) * SoundManager.sfxVolume;
        source.pitch = Random.Range(_sound.minPitch, _sound.maxPitch);
        source.maxDistance = maximumDistance;
        source.minDistance = minimumDistance;
        source.loop = loops;
        source.bypassListenerEffects = bypassAudioListener;
        source.gameObject.transform.position = pos;
        source.spatialBlend = spatialBlend;

        source.Play();

        if (!loops)
        {
            yield return new WaitForSeconds(source.clip.length);
            SoundManager.RepoolReceptacle(this);
        }
    }
}
