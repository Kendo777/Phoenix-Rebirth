using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    #region Public
    public GameObject sfxDestroy;
    public int health;
    #endregion

    #region MonoBehaviour
    private void Update()
    {
        if (health <= 0)
        {
            GameObject sfx = Instantiate(sfxDestroy);
            sfx.transform.position = transform.position;
            Destroy(sfx, 1f);
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health--;
        }
    }
    #endregion
}
