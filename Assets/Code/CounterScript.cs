using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CounterScript : MonoBehaviour
{
    public TextMeshProUGUI counterText; // Verweis auf die UI-Textbox
    public Button incrementButton;
    public Button decrementButton;
    
    private int counter = 0;

    void Start()
    {
        // Event Listener für die Buttons hinzufügen
        incrementButton.onClick.AddListener(Increment);
        decrementButton.onClick.AddListener(Decrement);

        // Initialen Text setzen
        UpdateText();
    }

    void Increment()
    {
        counter++;
        UpdateText();
    }

    void Decrement()
    {
        counter--;
        UpdateText();
    }

   void UpdateText()
    {
        // Überprüfung für 88
        if (counter == 88)
        {
            counterText.text = "Unik ist ein Hund!";
        }
        else
        {
            counterText.text = counter.ToString();
        }
    }
}
