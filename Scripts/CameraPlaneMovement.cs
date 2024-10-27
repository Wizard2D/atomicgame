using Godot;
using System;

public partial class CameraPlaneMovement : Camera2D
{
    public bool _isDragging = false;
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        // For mouse dragging
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.ButtonIndex == MouseButton.Right && mouseButtonEvent.Pressed)
        {
            // Start dragging
            _isDragging = true;
        }
        else if (@event is InputEventMouseButton mouseButtonEventReleased && mouseButtonEventReleased.ButtonIndex == MouseButton.Right)
        {
            // Stop dragging
            _isDragging = false;
        }
        else if(@event is InputEventMouseButton mouseScroll && mouseScroll.ButtonIndex == MouseButton.WheelDown)
        {
            var zoomDelta = 2.0 * mouseScroll.Factor != 0 ? -mouseScroll.Factor : -1;
            Zoom += new Vector2(zoomDelta, zoomDelta);
        }
        else if (@event is InputEventMouseButton mouseScrollUp && mouseScrollUp.ButtonIndex == MouseButton.WheelUp)
        {
            var zoomDelta = 2.0 * mouseScrollUp.Factor != 0 ? -mouseScrollUp.Factor : -1;
            Zoom -= new Vector2(zoomDelta, zoomDelta);
        }

        // Dragging logic
        if (_isDragging && @event is InputEventMouseMotion mouseMotionEvent)
        {
            Vector2 dragDelta = mouseMotionEvent.Relative; // The change in position during the drag
            GlobalPosition -= dragDelta;// Update camera position directly
        }
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Zoom = Zoom.Clamp(new Vector2(0.5f, 0.5f), new Vector2(2.0f, 2.0f));
    }
}
