using System.Collections;
using UnityEngine;

public static class YieldHelper
{
    public static WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();

    public static IEnumerator WaitForSeconds(float totalTime, bool ignoreTimeScale = false)
    {
        float time = 0f;
        while (time < totalTime)
        {
            time += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            yield return null;
        }
    }
}
