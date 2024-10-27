using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class AtomNode : Node2D
{
    public float repulsionStrength = 500f; // How strongly they repel each other
    public float idealDistance = 200f; // The desired minimum distance between atoms
    public List<AtomNode> otherAtoms = new List<AtomNode>(); // List of other atoms to check against
    private RigidBody2D rb;

    public override void _Ready()
    {
        base._Ready();
        
        foreach(var atom in GetParent().GetChildren())
        {
            if(atom is AtomNode)
            {
                if(!((AtomNode)atom).Equals(this))
                    otherAtoms.Add((AtomNode)atom);
            }
        }

        rb = GetNode<RigidBody2D>("Sprite/RB");
    }

    float timeTotal = 0;
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        GD.Print(timeTotal);
        if (timeTotal >= 5)
        {
            Vector2 dir = new Vector2(GD.RandRange(0, 1), GD.RandRange(0, 1));

            GD.Print(dir);

            rb.ApplyCentralImpulse(dir * 2);

            timeTotal = 0;
        }

        timeTotal += (float)delta;
    }
}
