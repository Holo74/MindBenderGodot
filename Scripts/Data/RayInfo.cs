using Godot;
public class RayInfo
{
    public RayInfo(RayCastData caster)
    {
        this.caster = caster;
    }
    public Vector3 normal;
    public bool colliding;
    public Node hit;
    public RayCastData caster;
}
