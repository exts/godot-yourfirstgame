using Godot;
using System;

public class Main : Node
{
    [Export] public PackedScene Mob;

    int score = 0;

    private Timer scoreTimer;
    private Timer mobTimer;
    private Timer startTimer;
    private Player player;
    private Position2D startPosition;
    private PathFollow2D mobSpawnLocation;

    private Random rand;

    private HUD hud;

    public override void _Ready()
    {
        rand = new Random();

        hud = GetNode("HUD") as HUD;

        //2d paths
        mobSpawnLocation = GetNode("MobPath/MobSpawnLocation") as PathFollow2D;

        //player object
        player = GetNode("Player") as Player;
        
        //position 
        startPosition = GetNode("StartPosition") as Position2D;

        //timers
        mobTimer = GetNode("MobTimer") as Timer;
        startTimer = GetNode("StartTimer") as Timer;
        scoreTimer = GetNode("ScoreTimer") as Timer;

        //connect signals
        hud.Connect("StartGame", this, "new_game");
        player.Connect("Hit", this, "game_over");
    }

    public void game_over()
    {
        score = 0;
        hud.update_score(score);
        hud.show_game_over();
        scoreTimer.Stop();
        mobTimer.Stop();
        var children = GetChildren();
        foreach(var child in children) {
            if(child is Mob) {
                var mob = child as Mob;
                mob.QueueFree();
            }
        }

        hud.show_game_over();
    }

    public void new_game()
    {
        score = 0;
        hud.update_score(score);
        hud.show_message("Get Ready!");
        player.start(startPosition.Position);
        startTimer.Start();
    }

    public void _on_MobTimer_timeout()
    {
        //choose random location on path2d
        mobSpawnLocation.SetOffset(rand.Next());

        //set direction
        var direction = mobSpawnLocation.Rotation + Mathf.PI/2;
        direction += rand_rand(-Mathf.PI/4, Mathf.PI/4);

        //create mob instance and add it to scene
        var mob_instance = Mob.Instance() as RigidBody2D;
        AddChild(mob_instance);

        mob_instance.Position = mobSpawnLocation.Position;
        mob_instance.Rotation = direction;
        mob_instance.SetLinearVelocity(new Vector2(rand_rand(150f, 250f), 0).Rotated(direction));
    }

    public void _on_ScoreTimer_timeout()
    {
        score += 1;
        hud.update_score(score);
    }

    public void _on_StartTimer_timeout()
    {
        mobTimer.Start();
        scoreTimer.Start();
    }

    private float rand_rand(float min, float max) 
    {
        return (float) (rand.NextDouble() * (max - min) + min);
    }

}
