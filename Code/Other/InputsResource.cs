using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class InputsResource : Resource
{
    [Export]
    public Godot.Collections.Dictionary<string, Godot.Collections.Array<InputEvent>> inputs;
}
