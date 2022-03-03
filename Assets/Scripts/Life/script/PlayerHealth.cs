using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    #region Public
    public int attack;
    public GameObject healthBar;
    public Image ImgHealthBar;
    public GameObject Nave;
    // public Image damageImage;
    public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // the color of the damageImage
    public int Min;
    public int Max;
    #endregion

    
    ////////////
    bool damaged;

    //principal function 
    //this sets the spaceships life and changes the UI when it receives damage

    #region Properties
    public int Health { get; private set; } = 100;
    public bool Visible
    {
        set 
        {
            healthBar.SetActive(value);
        }
        get
        {
            return healthBar.activeInHierarchy;
        }
    }

    public float CurrentPercent { get; private set; }

    public int CurrentValue { get; private set; }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        if (healthBar == null)
        {
            Debug.LogError("Error, health bar G.O reference is necessary");
        }
    }

    // Update is called once per frame

    void Update()
    {
        // If the player has just been damaged...
        if (damaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            // damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
           // damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;
    }

    #endregion

    /// <summary>
    /// Sets the health.
    /// </summary>
    /// <param name="health">Health.</param>
    public void SetHealth(int health)
    {
        //checks the the value of the new health against the last value saved

        if (health != CurrentValue)
        {

           
                //if its different we change the current value of the health and calculate the new percentage 
                CurrentValue = health;
                CurrentPercent = (float)CurrentValue / (float)(Max - Min);

            

            //show the changes of health on the UI

            ImgHealthBar.fillAmount = CurrentPercent;
        }
    }

    /// <summary>
    /// (DEPRECATED) Receives the damage. Use SetHealth instead.
    /// </summary>
    /// <param name="damage">Damage.</param>
    public void receiveDamage(int damage)
    {
        //bool will become true

        damaged = true;

        //spaceship life will decrease

        Health -= damage;

        //the int will be passed down to the setHealth function
        SetHealth(Health);
    }
   


    //temporal function to show how the script of the life works
    //<SUMARY>
    //if life is equal to zero the GameObect referenced as Nave will be destroyed 
    //if not it will receive damaged
    //</SUMARY>
    void OnMouseDown()
    {

        if (Health == 0)
        {
            Debug.Log("esta destruida");
            Destroy(Nave);
        }
        else
        {
            receiveDamage(attack);
        }
    }
}
