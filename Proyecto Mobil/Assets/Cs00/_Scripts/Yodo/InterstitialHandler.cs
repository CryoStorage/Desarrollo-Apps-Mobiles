using UnityEngine;
using Yodo1.MAS;

public class InterstitialHandler : MonoBehaviour
    {

    public void Start()
    {
        this.InitializeInterstitial();
        this.RequestInterstitial();
    }

    private void InitializeInterstitial()
    {
        // Instantiate
        Yodo1U3dInterstitialAd.GetInstance();

        // Ad Events
        Yodo1U3dInterstitialAd.GetInstance().OnAdLoadedEvent += OnInterstitialAdLoadedEvent;
        Yodo1U3dInterstitialAd.GetInstance().OnAdLoadFailedEvent += OnInterstitialAdLoadFailedEvent;
        Yodo1U3dInterstitialAd.GetInstance().OnAdOpenedEvent += OnInterstitialAdOpenedEvent;
        Yodo1U3dInterstitialAd.GetInstance().OnAdOpenFailedEvent += OnInterstitialAdOpenFailedEvent;
        Yodo1U3dInterstitialAd.GetInstance().OnAdClosedEvent += OnInterstitialAdClosedEvent;
    }

    private void RequestInterstitial()
    {
        Yodo1U3dInterstitialAd.GetInstance().LoadAd();
    }

    private void ShowInterstitial()
    {
        bool isLoaded = Yodo1U3dInterstitialAd.GetInstance().IsLoaded();

        if(isLoaded) Yodo1U3dInterstitialAd.GetInstance().ShowAd();
    }

    private void OnInterstitialAdLoadedEvent(Yodo1U3dInterstitialAd ad)
    {
        Debug.Log("[Yodo1 Mas] OnInterstitialAdLoadedEvent event received");
    }

    private void OnInterstitialAdLoadFailedEvent(Yodo1U3dInterstitialAd ad, Yodo1U3dAdError adError)
    {
        Debug.Log("[Yodo1 Mas] OnInterstitialAdLoadFailedEvent event received with error: " + adError.ToString());
    }

    private void OnInterstitialAdOpenedEvent(Yodo1U3dInterstitialAd ad)
    {
        Debug.Log("[Yodo1 Mas] OnInterstitialAdOpenedEvent event received");
    }

    private void OnInterstitialAdOpenFailedEvent(Yodo1U3dInterstitialAd ad, Yodo1U3dAdError adError)
    {
        Debug.Log("[Yodo1 Mas] OnInterstitialAdOpenFailedEvent event received with error: " + adError.ToString());
        // Load the next ad
        this.RequestInterstitial();
    }

    private void OnInterstitialAdClosedEvent(Yodo1U3dInterstitialAd ad)
    {
        Debug.Log("[Yodo1 Mas] OnInterstitialAdClosedEvent event received");
        // Load the next ad
        this.RequestInterstitial();
    }
}
