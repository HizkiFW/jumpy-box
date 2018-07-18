using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager {
    public static void PlayAd() {
        if(Advertisement.IsReady()) {
            Advertisement.Show();
        }
    }
}
