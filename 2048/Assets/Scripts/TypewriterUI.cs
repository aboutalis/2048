using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypewriterUI : MonoBehaviour
{
    [SerializeField] private float startDelay = 0f;
    [SerializeField] private float timeBetweenCharacters = 0.1f;
    [SerializeField] private string leadingCharacter = "";
    [SerializeField] private bool addLeadingCharacterBeforeDelay = false;

    private Text textComponent;
    private TMP_Text tmpProTextComponent;
    private string originalText;
    private AudioManager audio;

    private void Awake()
    {
        audio = FindObjectOfType<AudioManager>();
        textComponent = GetComponent<Text>();
        tmpProTextComponent = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        Invoke("AudioPlay", 0.3f);
        
        if (textComponent != null)
        {
            originalText = textComponent.text;
            textComponent.text = "";

            StartCoroutine(TypeWriterText());
        }

        if (tmpProTextComponent != null)
        {
            originalText = tmpProTextComponent.text;
            tmpProTextComponent.text = "";

            StartCoroutine(TypeWriterTMP());
        }

    }

    private void AudioPlay()
    {
        audio.Play("TypingEffect");
    }

    private IEnumerator TypeWriterText()
    {
        textComponent.text = addLeadingCharacterBeforeDelay ? leadingCharacter : "";

        yield return new WaitForSeconds(startDelay);

        foreach (char c in originalText)
        {
            if (textComponent.text.Length > 0)
            {
                textComponent.text = textComponent.text.Substring(0, textComponent.text.Length - leadingCharacter.Length);
            }

            textComponent.text += c;
            textComponent.text += leadingCharacter;

            yield return new WaitForSeconds(timeBetweenCharacters);
        }

        if (leadingCharacter != "")
        {
            textComponent.text = textComponent.text.Substring(0, textComponent.text.Length - leadingCharacter.Length);
        }
        if (textComponent.text.Length == 26)
        {
            audio.Stop("TypingEffect");
        }
    }

    private IEnumerator TypeWriterTMP()
    {
        tmpProTextComponent.text = addLeadingCharacterBeforeDelay ? leadingCharacter : "";

        yield return new WaitForSeconds(startDelay);

        foreach (char c in originalText)
        {
            if (tmpProTextComponent.text.Length > 0)
            {
                tmpProTextComponent.text = tmpProTextComponent.text.Substring(0, tmpProTextComponent.text.Length - leadingCharacter.Length);
            }

            tmpProTextComponent.text += c;
            tmpProTextComponent.text += leadingCharacter;

            yield return new WaitForSeconds(timeBetweenCharacters);
        }

        if (leadingCharacter != "")
        {
            tmpProTextComponent.text = tmpProTextComponent.text.Substring(0, tmpProTextComponent.text.Length - leadingCharacter.Length);
        }
        if (tmpProTextComponent.text.Length == 26)
        {
            audio.Stop("TypingEffect");
        }
    }
}
