using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private int _MaxHealth = 100; // This is just the default value; change it in the inspector instead.


    //Vars    
    private int _CurrentHealth;

    //Getters
    public int HealthAmount
    {
        get { return _CurrentHealth; }
    }

    //References
    [SerializeField]
    private TMP_Text _healthText;


    void Awake()
    {
        _CurrentHealth = _MaxHealth;
    }

    void Start()
    {
        UpdateDisplay(_CurrentHealth);
    }

    //Change the text to display how much health the player has
    private void UpdateDisplay(int amount)
    {
        _healthText.text = amount.ToString();
    }

    //Money Logic
    public bool AddHealth(int amount)
    {
        int oldHealthValue = _CurrentHealth;


        // Change the health by the specified amount.
        _CurrentHealth = Mathf.Clamp(_CurrentHealth + amount, 0, _MaxHealth);

        //If value is positive, update the screen.
        if (_CurrentHealth > 0)
        {
            StartCoroutine(AnimateText(Color.blue, oldHealthValue, _CurrentHealth));
            return true;
        }
        else // The player's health has reached 0.
        {
            StartCoroutine(AnimateText(Color.red, oldHealthValue, _CurrentHealth));

            GameOverScreen.Show();

            return false;
        }
    }

    [ContextMenu("AddHealth")]
    public void Add()
    {
        AddHealth(10);
    }

    [ContextMenu("SubHealth")]
    public void Sub()
    {
        AddHealth(-10);
    }

    public void TakeDamage(int amount)
    {
        amount = Mathf.Abs(amount);

        AddHealth(-amount);
    }

    //This visually changes the text
    private IEnumerator AnimateText(Color referenceColor, int initialAmount, int finalAmount)
    {
        Color intialColor = _healthText.color;
        float elapsedTime = 0.0f;
        float duration = 0.5f; //Change this for longer animation

        //This calculates and lerps the animation
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            _healthText.color = Color.Lerp(intialColor, referenceColor, t);
            UpdateDisplay((int)Mathf.Lerp((float)initialAmount,(float)finalAmount,t));

            yield return null;
        }

        _healthText.color = referenceColor;

        //This calls the same function, but returns the text to white. This may need to be changed if the text color is no longer white.
        StartCoroutine(AnimateText(Color.white, _CurrentHealth, _CurrentHealth));
    }
}
