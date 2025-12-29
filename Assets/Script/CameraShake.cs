using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake I;

    Vector3 basePos;
    Coroutine routine;

    void Awake()
    {
        if (I != null && I != this) { Destroy(this); return; }
        I = this;
        basePos = transform.position;
    }

    void LateUpdate()
    {
        // başka script kamerayı oynatıyorsa basePos güncel kalsın
        if (routine == null)
            basePos = transform.position;
    }

    public void Shake(float duration, float strength)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(ShakeRoutine(duration, strength));
    }

    IEnumerator ShakeRoutine(float duration, float strength)
    {
        basePos = transform.position;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;
            transform.position = basePos + new Vector3(x, y, 0f);
            yield return null;
        }

        transform.position = basePos;
        routine = null;
    }
}
