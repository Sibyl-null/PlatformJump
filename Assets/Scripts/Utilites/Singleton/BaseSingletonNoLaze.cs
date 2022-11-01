public class BaseSingletonNoLaze<T> where T : BaseSingletonNoLaze<T>, new()
{
    private static T _instance = new T();
    public static T Instance => _instance;
}
