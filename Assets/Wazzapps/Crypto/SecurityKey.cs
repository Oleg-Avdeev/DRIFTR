using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wazzapps.Crypto
{
    [CreateAssetMenu(fileName = "Assets/Resources/Wazzapps/Crypto/SecurityKey", menuName = "Wazzapps/Crypto/SecurityKey", order = 1)]
    public class SecurityKey : ScriptableObject
    {
        public string KEY;
    }
}
