using Godot;
using System;

public partial class BuildAction : Node2D, IPlayerAction
{
    [Export] Control buildMenuUi;
    [Export] Resource cursor;

    bool isBuilding = false;
    Node2D instance;
    PackedScene sceneToIstance = null;
    int price = 0;

    public void StartBuilding(Node2D instance, PackedScene scene, int price)
    {
        buildMenuUi.Visible = false;
        this.instance = instance;
        this.price = price;
        sceneToIstance = scene;
        //instance = (Node2D)scene.Instantiate();
        //GetParent().AddChild(instance);
        isBuilding = true;
        // Pick item to build
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEvent keyEvent && keyEvent.IsPressed())
        {

            if (@event.IsActionPressed(GameInput.Shoot))
            {
                if (isBuilding && Inventory.Instance.AlientStuffAmount >= price)
                {
                    Inventory.Instance.AlientStuffAmount -= price;
                    var newInstance = (Turret)sceneToIstance.Instantiate();
                    //instance.ProcessMode = ProcessModeEnum.Disabled;
                    newInstance.previewRangeMesh.Visible = false;
                    GameManager.Instance.storageNode.AddChild(newInstance);
                    newInstance.GlobalPosition = instance.GlobalPosition;
                    //instance.GlobalPosition = GetGlobalMousePosition();
                    //var duplicatedInstance = instance.Duplicate();
                    //GameManager.Instance.storageNode.AddChild(duplicatedInstance);
                }
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (isBuilding)
        {
            instance.GlobalPosition = GetGlobalMousePosition();
        }
    }

    public void Start()
    {
        Input.SetCustomMouseCursor(cursor);
        buildMenuUi.Visible = true;
        //throw new NotImplementedException();
    }

    public void Stop()
    {
        buildMenuUi.Visible = false;
        isBuilding = false;
        if (instance != null) { 
            instance.QueueFree();
            instance = null;
        }
        //throw new NotImplementedException();
    }
}
