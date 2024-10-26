public interface IHittable
{
    public bool IsHittable { get; }
    void OnHit(int damage);
}
