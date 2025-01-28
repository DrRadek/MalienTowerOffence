using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerActionSwitcher : Node
{
    [Export] Godot.Collections.Array<Node> playerActionsScenes;
    [Export] Godot.Collections.Array<Control> selectedControls;

    List<IPlayerAction> playerActions = new();


    enum PlayerActions
    {
        Fight = 0,
        Repair = 1,
        Build = 2,
        //Move = 3,
        Delete = 3
    }

    PlayerActions activeAction = PlayerActions.Fight;
    public override void _Ready()
    {
        foreach( var action in playerActionsScenes)
        {
            playerActions.Add(action as IPlayerAction);
        }
        playerActions[(int)activeAction].Start();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEvent keyEvent && keyEvent.IsPressed())
        {

            if (@event.IsActionPressed(GameInput.PlayerActionFight))
            {
                SwitchAction(PlayerActions.Fight);
            }
            else if (@event.IsActionPressed(GameInput.PlayerActionRepair))
            {
                SwitchAction(PlayerActions.Repair);
            }
            else if (@event.IsActionPressed(GameInput.PlayerActionBuild))
            {
                SwitchAction(PlayerActions.Build);
            }
            //else if (@event.IsActionPressed(GameInput.PlayerActionMove))
            //{
            //    SwitchAction(PlayerActions.Move);
            //}
            else if (@event.IsActionPressed(GameInput.PlayerActionDelete))
            {
                SwitchAction(PlayerActions.Delete);
            }
            else if (@event.IsActionPressed(GameInput.PlayerActionPrevious))
            {
                var newAction = (int)activeAction - 1;
                if (newAction < 0)
                    newAction = 3;
                SwitchAction((PlayerActions)newAction);
            }
            else if (@event.IsActionPressed(GameInput.PlayerActionNext))
            {
                var newAction = (int)activeAction + 1;
                if (newAction > 3)
                    newAction = 0;
                SwitchAction((PlayerActions)newAction);
            }
        }
    }

    void SwitchAction(PlayerActions newAction)
    {
        //if (newAction == activeAction)
        //    return;

        selectedControls[(int)activeAction].Visible = false;
        playerActions[(int)activeAction].Stop();
        activeAction = newAction;
        playerActions[(int)activeAction].Start();
        selectedControls[(int)activeAction].Visible = true;

    }
}
