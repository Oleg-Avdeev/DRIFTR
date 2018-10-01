using UnityEngine.UI;
using UnityEngine;
using Engine;
using System.Text;

namespace UI
{
    public class TitleTextEffect : ActiveObject
    {
        [SerializeField] private string text = "DRIFTR"; 
        [SerializeField] private Text targetText;

        private float frequency = 1f;
        private float outset = 0.1f;

        private readonly string openTag = "<size={0}>";
        private readonly string closeTag = "</size>";
        private StringBuilder stringBuilder;

        public override void Act()
        {
            stringBuilder = new StringBuilder();
            
            for (int i = 0; i < text.Length; i++)
            {
                float size = 172*(1.0f + Mathf.Sin(Time.time * frequency + i*outset)/3);
                stringBuilder.Append(string.Format(openTag + text[i] + closeTag, size));
            }

            targetText.text = stringBuilder.ToString();
        }
    }
}