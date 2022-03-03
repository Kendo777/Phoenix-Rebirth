using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Apply here every piece that will be set free. (Make sure that it has the ShipPieceBehaviour set.
/// </summary>
public class ShipPieceDestroy : MonoBehaviour
{
    #region Public
    public GameObject[] pieces;
    #endregion

    #region MonoBehaviour
    public void Destroy()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i].GetComponent<ShipPieceBehaviour>() != null)
                pieces[i].GetComponent<ShipPieceBehaviour>().destroyed = true;
            else
                Debug.LogError("Please, make sure that every piece has ShipPieceBehaviour");
        }
    }

    #endregion
}
