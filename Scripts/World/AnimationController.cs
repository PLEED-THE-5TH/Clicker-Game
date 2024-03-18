using Godot;
using System;

public partial class AnimationController : Node
{
	static AnimationController singleton;
	static PackedScene dustCloudScene = GD.Load<PackedScene>("res://Scenes/World/Animations/DustCloud.tscn");
    static PackedScene bombScene = GD.Load<PackedScene>("res://Scenes/World/Animations/Bomb.tscn");
    static PackedScene explosionScene = GD.Load<PackedScene>("res://Scenes/World/Animations/Explosion.tscn");
    public override void _Ready()
	{
		singleton = this;
    }
	public override void _Process(double delta)
	{
	}
	public static void MiningAnimation(Godot.Vector2 animLocation)
	{
        AnimatedSprite2D sprite = dustCloudScene.Instantiate<AnimatedSprite2D>();
        singleton.AddChild(sprite);
        sprite.AnimationFinished += () => sprite.QueueFree();
		sprite.GlobalPosition = animLocation;
		sprite.Play();
    }
    public static void BombAnimation(Godot.Vector2 animLocation, Action callback = null)
	{
        AnimatedSprite2D sprite = bombScene.Instantiate<AnimatedSprite2D>();
        singleton.AddChild(sprite);
        sprite.AnimationFinished += () => ExplosionAnimation(animLocation);
        sprite.AnimationFinished += callback;
        sprite.AnimationFinished += () => sprite.QueueFree();
        sprite.GlobalPosition = animLocation;
        sprite.Play();
    }
    public static void ExplosionAnimation(Godot.Vector2 animLocation)
    {
        AnimatedSprite2D sprite = explosionScene.Instantiate<AnimatedSprite2D>();
        singleton.AddChild(sprite);
        sprite.AnimationFinished += () => sprite.QueueFree();
        sprite.GlobalPosition = animLocation;
        sprite.Play();
    }
}
