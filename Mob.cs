using Godot;
using System;

public class Mob : RigidBody2D
{
    [Export]
    public int MIN_SPEED = 150;

    [Export]
    public int MAX_SPEED = 250;

    private String[] mob_types = {"walk", "swim", "fly"};

    private AnimatedSprite animated_sprite;

    public override void _Ready()
    {
        var random_mob = new Random();
        animated_sprite = GetNode("AnimatedSprite") as AnimatedSprite;
        animated_sprite.Animation = mob_types[random_mob.Next(0, mob_types.Length)];
    }
    
    public override void _Process(float delta)
    {
        animated_sprite.Play();
    }

    public void _on_Visibility_screen_exited()
    {
        this.QueueFree();
    }

}
