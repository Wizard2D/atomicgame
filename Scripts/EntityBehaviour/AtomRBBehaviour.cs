using AtomGame.Scripts;
using Godot;
using System;
using System.Collections.Generic;

public partial class AtomRBBehaviour : RigidBody2D
{
    private bool _dragging = false;
    private bool _smoothReleasing = false;
    private float _dragStrength = 10f;
    private Vector2 _releaseTarget;
    private Sprite2D sprite;
    public AtomSpec AtomDef = Atoms.Hydrogen;
    public List<AtomRBBehaviour> otherAtoms = new List<AtomRBBehaviour>();
    public bool Simulate = true;
    private RigidBody2D rb;
    int i = 0;

    // Atom Properties
    public float Entropy = 0f;
    public float Temperature = 0f;

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode<Sprite2D>("Sprite");
        bool found = false;
     
    }

    private Rect2 GetGlobalRect()
    {
        var localRect = sprite.GetRect();

        var globalRect = new Rect2(
            sprite.GlobalTransform * localRect.Position,
            localRect.Size * sprite.GlobalScale
        );

        return globalRect;
    }

    float timeTotal = 0;
    float etrTimeTotal = 0;
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (Simulate)
        {
            if (timeTotal >= 5 + i)
            {
                Vector2 dir = new Vector2(GD.RandRange(0, 1), GD.RandRange(0, 1));

                ApplyCentralImpulse(dir * 1.5f * ((Entropy + 1) / 10));

                timeTotal = 0;
            }

            if (etrTimeTotal >= 5)
            {
                Entropy += 10 * (LinearVelocity.Length() / 100);
                Entropy = Mathf.Clamp(Entropy, 0, 5000);
                etrTimeTotal = 0;
            }

            if (_dragging)
            {
                var dir = GlobalPosition.DirectionTo(GetGlobalMousePosition());
                ApplyForce(dir * 15);
            }

            timeTotal += (float)delta;
            etrTimeTotal += (float)delta;

            foreach (AtomRBBehaviour atom in GetParent().GetChildren())
            {
                if (atom.Equals(this)) continue;

                var distance = GlobalPosition.DistanceTo(atom.GlobalPosition);
                if (distance < 150)
                {
                    if (distance > 0.01f)
                    {
                        if (Entropy > 100 * ((AtomDef.AtomicMass + atom.AtomDef.AtomicMass) / 10))
                        {
                            float forceMagnitude = (1000 / (distance * distance)) * (Entropy / 8);
                            var dir = GlobalPosition.DirectionTo(atom.GlobalPosition);
                            ApplyCentralImpulse(dir * forceMagnitude);
                            atom.ApplyCentralImpulse(-dir * forceMagnitude);
                            HandleFusion(atom);
                        }
                        else
                        {
                            float forceMagnitude = (1500 / (distance * distance)) * (1 + Mass) * LinearVelocity.Length();
                            var dir = -GlobalPosition.DirectionTo(atom.GlobalPosition);
                            ApplyCentralImpulse(dir * forceMagnitude);
                            atom.ApplyCentralImpulse(-dir * forceMagnitude);
                        }
                    }
                }
            }
        }
    }

    public void HandleFusion(AtomRBBehaviour other)
    {
        var oAD = other.AtomDef;

        var newAtomSpec = Atoms.FindAtomByAtomicNumber(oAD.AtomicNumber + AtomDef.AtomicNumber);

        if(newAtomSpec != null)
        {
            var newAtom = (AtomRBBehaviour)ResourceLoader.Load<PackedScene>("Scenes/atom.tscn").Instantiate();
            newAtom.Entropy = 0;
            newAtom.AtomDef = newAtomSpec.Value;
            other.QueueFree();
            QueueFree();

            other.Simulate = false;
            Simulate = false;

            foreach (AtomRBBehaviour atom in GetParent().GetChildren())
            {
                if (atom.Equals(this)) continue;

                var distance = GlobalPosition.DistanceTo(atom.GlobalPosition);
                if (distance < 450)
                {
                    if (distance > 0.01f)
                    {
                        float forceMagnitude = (1700 / (distance * distance)) * (1 + Mass) * LinearVelocity.Length();
                        var dir = -GlobalPosition.DirectionTo(atom.GlobalPosition);
                        ApplyCentralImpulse(dir * forceMagnitude);
                        atom.ApplyCentralImpulse(-dir * forceMagnitude);
                    }
                    atom.Entropy += 5;
                }
            }

            GetParent().AddChild(newAtom);
            newAtom.GlobalPosition = GlobalPosition+new Vector2(0,80);
            newAtom.LinearVelocity = new Vector2(0, 10);
        }
        else
        {
            Entropy = 1;
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("mouse_left"))
        {
            if (GetGlobalRect().HasPoint(GetGlobalMousePosition()))
            {
                _dragging = true;
                _smoothReleasing = false;
            }
        }

        if (Input.IsActionJustReleased("mouse_left"))
        {
            if (_dragging)
            {
                _releaseTarget = GetGlobalMousePosition();
            }
            _dragging = false;
        }

        var mat = (ShaderMaterial)sprite.Material;
        mat.SetShaderParameter("pulse_entr", Entropy);

        GetNode<RichTextLabel>("RichTextLabel").Text = AtomDef.Symbol;
    }
}
