using Godot;
using System;

public class Player : Area2D
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    [Signal]
    public delegate void Hit();

    [Export]
    public int SPEED = 400;

    private Vector2 velocity;

    private Vector2 screensize;

    private CollisionShape2D collision_shape;

    public override void _Ready()
    {
        collision_shape = GetNode("CollisionShape2D") as CollisionShape2D;
        screensize = GetViewport().GetSize();
        this.Hide();

        // AddUserSignal("hit");
    }

   public override void _Process(float delta)
   {
        velocity = new Vector2();
        if(Input.IsActionPressed("ui_right")) {
            velocity.x += 1;
        }
        
        if(Input.IsActionPressed("ui_left")) {
            velocity.x -= 1;
        }
        
        if(Input.IsActionPressed("ui_down")) {
            velocity.y += 1;
        }

        if(Input.IsActionPressed("ui_up")) {
            velocity.y -= 1;
        }

        var animated_sprite = GetNode("AnimatedSprite") as AnimatedSprite;
        if(velocity.Length() > 0) {
            velocity = velocity.Normalized() * SPEED;
            animated_sprite.Play();
        } else {
            animated_sprite.Stop();
        }

        this.Position += velocity * delta;

        var posx = Mathf.Clamp(this.Position.x, 0, screensize.x);
        var posy = Mathf.Clamp(this.Position.y, 0, screensize.y);

        this.SetPosition(new Vector2(posx, posy));

        if(velocity.x != 0) {
            animated_sprite.Animation = "right";
            animated_sprite.FlipH = velocity.x < 0;
            animated_sprite.FlipV = false;
        } else if(velocity.y != 0) {
            animated_sprite.Animation = "up";
            animated_sprite.FlipV = velocity.y > 0;
        }
   }

   public void _on_Player_body_entered(Godot.Object body)
   {
       this.Hide();
       EmitSignal("Hit");
       
       collision_shape.Disabled = true;
   }

   public void start(Vector2 pos) 
   {
       this.Position = pos;
       this.Show();
       collision_shape.Disabled = false;
   }
}
