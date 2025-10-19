using MBDK.Advertisement.AdsManager;
using UnityEngine;

public class TestScriptForCooldown : MonoBehaviour
{
    public float time = 30;
    private AdsCooldownController _adsCooldownController;

    private void Awake()
    {
        _adsCooldownController = new();
        _adsCooldownController.SetCooldownDuration(time);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_adsCooldownController.CanShowAds())
            {
                Debug.Log("Cooldown!");
                _adsCooldownController.MarkAdsShown();
            }
        }
    }
}
