using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineBasicMultiChannelPerlin perlin;
    [SerializeField] private float shakeIntensity = 1, duration = .3f;

    private void OnEnable()
    {
        EventCallback.OnPlayerHit += Shake;
    }

    private void OnDisable()
    {
        EventCallback.OnPlayerHit -= Shake;
    }

    private void Shake(int health) => StartCoroutine(Shaka());

    private IEnumerator Shaka()
    {
        perlin.AmplitudeGain = shakeIntensity;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        perlin.AmplitudeGain = 0f;
    }
}