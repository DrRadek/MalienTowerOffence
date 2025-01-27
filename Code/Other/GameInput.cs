using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


// https://www.youtube.com/watch?v=sNjpSAcC878
public partial class GameInput : Node
{
    string inputsResourceFile = "res://inputs.tres";

    static KeyValuePair<string, Godot.Collections.Array<InputEvent>> CreateActionLambda(Func<string, Godot.Collections.Array<InputEvent>> lambda, string actionName)
    {
        InputMap.AddAction(actionName);
        return new(actionName, lambda(actionName));
    }

    static InputEventKey CreatePhysicalInputEventKey(String actionName, Key physicalKey)
    {
        var inputEventKey = new InputEventKey();
        inputEventKey.PhysicalKeycode = physicalKey;

        InputMap.ActionAddEvent(actionName, inputEventKey);
        return inputEventKey;
    }

    static InputEventMouseButton CreateInputEventMouseButton(String actionName, MouseButton mouseButton)
    {
        var inputEventKey = new InputEventMouseButton();
        inputEventKey.ButtonIndex = mouseButton;

        InputMap.ActionAddEvent(actionName, inputEventKey);
        return inputEventKey;
    }

    void AddAction(KeyValuePair<string, Godot.Collections.Array<InputEvent>> pair)
    {
        defaultInputs.Add(pair.Key, pair.Value);
    }


    public override void _Ready()
    {
        CreateDefaultInputs();

       if (!ResourceLoader.Exists(inputsResourceFile))
       {
            var resource = new Resource();
            var defaultInputsResource = new InputsResource();
            defaultInputsResource.inputs = defaultInputs;
            return;
        }

        var inputsResource = (InputsResource)ResourceLoader.Load(inputsResourceFile);
        customInputs = inputsResource.inputs;
        foreach(var key in customInputs.Keys.ToList())
        {
            if (defaultInputs.ContainsKey(key))
            {
                InputMap.EraseAction(key);
                InputMap.AddAction(key);
                foreach(var inputEvent in customInputs[key])
                {
                    InputMap.ActionAddEvent(key, inputEvent);
                }
            }
            else
            {
                customInputs.Remove(key);
            }
        }

        // TODO: make a function out of this
        var customInputsResource = new InputsResource();
        customInputsResource.inputs = customInputs;
        ResourceSaver.Save(customInputsResource, inputsResourceFile);

        // TODO: Apply Custom inputs and mix it with default ones




        //InputsResource inputsResource = new InputsResource();


        // Movement
        //    InputEventKey inputEvent = new();
        //inputEvent.PhysicalKeycode = Key.Z;

        //InputMap.AddAction("abc");
        //InputMap.ActionAddEvent("abc", inputEvent);

        // Camera
    }
}
