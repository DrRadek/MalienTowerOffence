using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class GameInput
{
    Godot.Collections.Dictionary<string, Godot.Collections.Array<InputEvent>> defaultInputs = new();
    Godot.Collections.Dictionary<string, Godot.Collections.Array<InputEvent>> customInputs = new();

    void CreateDefaultInputs()
    {
        // Movement
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.W),
            };
        }, MovementForward));

        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.S),
            };
        }, MovementBackward));

        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.A),
            };
        }, MovementLeft));

        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.D),
            };
        }, MovementRight));

        // MouseModeSwitch
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.M),
            };
        }, MouseModeSwitch));

        // Shoot
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreateInputEventMouseButton(actionName, MouseButton.Left),
            };
        }, Shoot));

        // Use
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.F),
            };
        }, Use));

        // PlayerActionFight
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.Key1),
            };
        }, PlayerActionFight));

        // PlayerActionRepair
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.Key2),
            };
        }, PlayerActionRepair));

        // PlayerActionBuild
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.Key3),
            };
        }, PlayerActionBuild));

        // PlayerActionMove
        //AddAction(CreateActionLambda(actionName =>
        //{
        //    return new(){
        //        CreatePhysicalInputEventKey(actionName, Key.Key4),
        //    };
        //}, PlayerActionMove));

        // PlayerActionDelete
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.Key4),
            };
        }, PlayerActionDelete));

        // PlayerActionNext
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreateInputEventMouseButton(actionName, MouseButton.WheelDown),
            };
        }, PlayerActionNext));

        // PlayerActionPrevious
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreateInputEventMouseButton(actionName, MouseButton.WheelUp),
            };
        }, PlayerActionPrevious));
    }
}