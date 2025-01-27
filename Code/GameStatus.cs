using Godot;
using System;

public partial class GameStatus : Label
{
    public static GameStatus Instance { get; private set; }

    bool isWin = false;
    bool isLose = false;

    public override void _Ready()
    {
        Instance = this;
    }

    public void SetWinStatus()
    {
        if (isWin)
            return;

        if (!isLose)
        {
            Text = "Mission Success!\nYou have eliminated all alien homes.";
        }
        else
        {
            Text += "\n\nYou have eliminated all alien homes, but you are supposed to be dead.\nThat's cheating!"; 
        }

        isWin = true;
    }

    public void SetLoseStatus()
    {
        if (isLose)
            return;

        if (!isWin)
        {
            Text = "Mission Failed!\nAliens have destroyed your base and you are now dead.";
        }
        else
        {
            Text += "\n\nYou have managed to inform Earth about your success\n but you didn't survive.";
        }

        isLose = true;
    }
}
