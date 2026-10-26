using Godot;
using System;

public partial class BulletTrail : Line2D
{
	[Export] public int Length { get; set; } = 3; 

    private Node2D parent; 
    public override void _Ready()
    {
        parent = GetParent<Node2D>();

        ClearPoints();
        TopLevel = true;
    }

    public override void _Process(double delta)
    {
    
        if (parent == null) return;

        AddPoint(parent.GlobalPosition);

        if (GetPointCount() > Length)
        {
            RemovePoint(0);
        }
    }
}
