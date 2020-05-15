using Godot;
using System.Collections.Generic;

public class ObjectPool<T> where T : Spatial
{
    PackedScene creator;
    private int max = 0;
    private Node parent;
    private Queue<T> queue = new Queue<T>();
    private string name;
    public ObjectPool(string name, int amount = 10, Node parent = null)
    {
        creator = ResourceLoader.Load<PackedScene>("res://Resources/SpawnInstances/" + name);
        max = amount;
        if (parent == null)
        {
            this.parent = GameManager.Instance.Root;
        }
        else
        {
            this.parent = parent;
        }
        this.name = name;
        while (queue.Count < amount)
        {
            queue.Enqueue((T)creator.Instance());
        }
    }

    public T Pull()
    {
        T holder = queue.Dequeue();
        GameManager.Instance.Root.AddChild(holder);
        holder.Owner = parent;
        return holder;
    }

    public T Pull(Vector3 pos, Vector3 rot)
    {
        T holder;
        if (queue.Count == 0)
        {
            holder = (T)ResourceLoader.Load<PackedScene>("res://Resources/SpawnInstances/" + name).Instance();
        }
        else
        {
            holder = queue.Dequeue();
        }
        holder.Translation = pos;
        holder.Rotation = rot;
        GameManager.Instance.Root.AddChild(holder);
        holder.Owner = parent;
        return holder;
    }

    public void Push(T thing)
    {
        GameManager.Instance.Root.RemoveChild(thing);
        queue.Enqueue(thing);
    }
}
