using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CertificateSHA1Fingerprint {

    private static AndroidJavaClass unityClass;
    private static AndroidJavaObject unityActivity;
    private static AndroidJavaObject unityContext;
    private static AndroidJavaClass customClass;

    public static string getCertificateSHA1Fingerprint()
    {
#if UNITY_EDITOR
        return "sha1_of_your_fingerprint";
#elif UNITY_ANDROID
        unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
        unityContext = unityActivity.Call<AndroidJavaObject>("getApplicationContext");

        customClass = new AndroidJavaClass("org.wazzapps.certificatesha1fingerprint.CertificateSHA1Fingerprint");

        string result = customClass.CallStatic<string>("getCertificateSHA1Fingerprint", unityContext);
        return result;
#elif UNITY_IOS

#endif
    }
}
