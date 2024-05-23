using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    //Vars
    private int _health = 100;

    //Getters
    public int HealthAmount
    {
        get { return _health; }
    }

    //References
    [SerializeField]
    private TMP_Text _healthText;

    void Start()
    {
        UpdateDisplay(_health);
    }

    //Change the text to display how much health the player has
    private void UpdateDisplay(int amount)
    {
        _healthText.text = amount.ToString();
    }

    //Money Logic
    public bool AddCurrency(int amount)
    {
        //If value is positive, add
        if (amount >= 0)
        {
            StartCoroutine(AnimateText(Color.blue));
            _health += amount;
            return true;
        }
        //If negative, call a function for gameover
        else
        {
            if (_health + amount > 0)
            {
                StartCoroutine(AnimateText(Color.red));
                _health += amount;
                return true;
            }
            StartCoroutine(AnimateText(Color.red));
            //TODO Gameover function
            _health = 0;
            return false;
        }
    }

    [ContextMenu("AddCurrency")]
    public void Add()
    {
        AddCurrency(10);
    }

    [ContextMenu("SubCurrency")]
    public void Sub()
    {
        AddCurrency(-10);
    }

    //This visually changes the text
    private IEnumerator AnimateText(Color referenceColor)
    {
        Color intialColor = _healthText.color;
        int initialAmount = _health;
        float elapsedTime = 0.0f;
        float duration = 0.5f; //Change this for longer animation

        //This calculates and lerps the animation
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            _healthText.color = Color.Lerp(intialColor, referenceColor, t);
            UpdateDisplay((int)Mathf.Lerp((float)initialAmount,(float)_health,t));

            yield return null;
        }

        _healthText.color = referenceColor;

        //This calls the same function, but returns the text to white. This may need to be changed if the text color is no longer white.
        StartCoroutine(AnimateText(Color.white));
    }
}
