using UnityEngine;
using UnityEngine.EventSystems;

public class ShipClickManager : MonoBehaviour
{
    #region Private
    private ShipManager manager;
    #endregion

    #region MonoBehaviour
    private void OnMouseOver()
    {
        if(Input.GetMouseButton(0) && manager.PlayerParent.State == PLAYERSTATE.SELECTING_SHIP)
        {
            manager.Clicked();
        }
    }
    #endregion

    #region Methods
    public void SetManager(ShipManager mng)
    {
        manager = mng;
    }
    #endregion
}
