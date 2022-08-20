using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Fader : MonoBehaviour
{
    /// <summary>Version of this release.
    /// 
    /// The first digit represents a major, major release. 
    /// The second digit represents a significant update, gamechanging new content.
    /// The third digit represents a small update, new content or behavior.
    /// The fourth digit represents a noticeable tweak or hotfix.
    /// </summary>
    
    private readonly string version = "0.0.1.2";

    /// <summary>Type of this release.</summary>
    private readonly string releaseType = "demo";

    [SerializeField]
    /// <summary>Text component to display the version.</summary>
    private TMP_Text versionText;

    private void Start()
    {
        versionText.text = releaseType + " version " + version;
        transform.SetAsLastSibling();
    }

    
}
