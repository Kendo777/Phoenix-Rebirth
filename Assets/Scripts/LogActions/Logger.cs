/// <summary>
/// Everything that you want loged in the log window is handled by this
/// static class.
/// </summary>
static public class Logger
{
    #region Public

    #endregion

    #region Private
    static private string _currentActionStringRed = "";
    static private string _currentActionStringBlue = "";
    #endregion

    #region Properties
    /// <summary>
    /// Returns the specified player's text action history.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    static public string Player(PlayerTeams player) => 
            player == PlayerTeams.RED ? _currentActionStringRed : _currentActionStringBlue;
    #endregion

    #region Methods
    /// <summary>
    /// Logs into the screen every bullet that is instantiated by the <paramref name="player"/>
    /// </summary>
    /// <param name="player"></param>
    /// <param name="ship"></param>
    static public void ShootAction(PlayerManager player, ShipManager ship)
    {
        string action = player.Name + " shot with his " + ship.shipName + "!";
        AddAction(player.team, action);
    }

    /// <summary>
    /// Logs into the screen that <paramref name="player"/>'s ship received damage.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="ship"></param>
    /// <param name="damage"></param>
    /// <param name="current"></param>
    static public void ReceiveDamage(PlayerManager player, ShipManager ship, int damage, int current)
    {
        string action = player.Name + "'s " + ship.shipName + " received " + damage + " points of damage!";
        string action2 = current + " health left for " + player.Name + "'s " + ship.shipName;
        AddAction(player.team, action);
        AddAction(player.team, action2);
    }

    /// <summary>
    /// Logs into the screen that <paramref name="player"/> is controlling <paramref name="ship"/>
    /// </summary>
    /// <param name="player"></param>
    /// <param name="ship"></param>
    static public void Controlling(PlayerManager player, ShipManager ship)
    {
        string action = player.Name + " is using his " + ship.shipName + "!";
        AddAction(player.team, action);
    }

    /// <summary>
    /// Logs into the screen that <paramref name="player"/> got his <paramref name="ship"/> destroyed.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="ship"></param>
    static public void Destroyed(PlayerManager player, ShipManager ship)
    {
        string action = player.Name + "'s " + ship.shipName + " is completely destroyed!";
        string action2 = player.Ships.Length - 1 + " ships left for " + player.Name + "!";
        AddAction(player.team, action);
        AddAction(player.team, action2);
    }

    /// <summary>
    /// Adds an action to the specified <paramref name="team"/> with the string that is passed to <paramref name="action"/>
    /// </summary>
    /// <param name="team"></param>
    /// <param name="action"></param>
    static private void AddAction(PlayerTeams team, string action)
    {
        if (PlayerTeams.RED == team)
        {
            _currentActionStringRed  = action + "\n" + _currentActionStringRed;
        }
        else
        {
            _currentActionStringBlue = action + "\n" + _currentActionStringBlue;
        }
    }

    /// <summary>
    /// Logs into the screen that <paramref name="player"/> is currently playing
    /// </summary>
    /// <param name="player"></param>
    static public void ChangeTurn(PlayerManager player)
    {
        string action = "Next turn for " + player.Name + "!";
        AddAction(player.team, action);
    }

    /// <summary>
    /// Logs into the screen that <paramref name="ship"/> has been selected by <paramref name="player"/>
    /// </summary>
    /// <param name="player"></param>
    /// <param name="ship"></param>
    static public void Selection(PlayerManager player, ShipManager ship)
    {
        string action = player.Name + " selected his " + ship.shipName + "!";
        AddAction(player.team, action);
    }
    #endregion

}
