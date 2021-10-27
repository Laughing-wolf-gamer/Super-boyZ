using System;
using UnityEngine;
// using GoogleMobileAds.Api.Mediation.Vungle;
// using GoogleMobileAds.Api.Mediation.UnityAds;

public class TACanvasController : MonoBehaviour{
    public GameObject adConsentCanvas;
    public GameObject mainMenuObject;

    public static event Action onAgree;

    // private void Start() {
    //     // if (!PlayerPrefs.HasKey("npa"))
    //     // {
    //     //     new Alert("Terms And Conditions", "Do you want to receive personalized ad?")
    //     //         .SetPositiveButton("Yes", () =>
    //     //         {
    //     //             PlayerPrefs.SetInt("npa", 0);
    //     //             UnityAds.SetGDPRConsentMetaData(true);
    //     //             Vungle.UpdateConsentStatus(VungleConsent.ACCEPTED);
    //     //         })
    //     //         .SetNegativeButton("No", () =>
    //     //         {
    //     //             PlayerPrefs.SetInt("npa", 1);
    //     //             UnityAds.SetGDPRConsentMetaData(false);
    //     //             Vungle.UpdateConsentStatus(VungleConsent.DENIED);
    //     //         })
    //     //         .Show();
    //     // }
    //     // else
    //     // {
    //     //     npa = PlayerPrefs.GetInt("npa");
    //     // }
    // }

    // public static void CallOnAgreeEvent(){
    //     onAgree?.Invoke();
    // }

    // private void EnableCanvas(){
    //     adConsentCanvas.SetActive(true);
    //     mainMenuObject.SetActive(false);
    // }

    // private void OnEnable(){
    //     onAgree += EnableCanvas;
    // }

    // private void OnDisable(){
    //     onAgree -= EnableCanvas;
    // }

    // public void Agree(){
    //     PlayerPrefs.SetInt("npa", 0);
    //     UnityAds.SetGDPRConsentMetaData(true);
    //     Vungle.UpdateConsentStatus(VungleConsent.ACCEPTED);
    //     FindObjectOfType<AdController>().InitializeSdk();
    //     mainMenuObject.SetActive(true);
    //     adConsentCanvas.SetActive(false);
    // }
    
    // public void TermsAndCondition(){
    //     Application.OpenURL("http://playresume.in/privacy_policy.html");
    // }
}
