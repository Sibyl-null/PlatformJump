using System;
using System.Reflection;
using System.Threading;

public class BaseSingleton<T> where T : BaseSingleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Type type = typeof(T);
                ConstructorInfo[] ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                ConstructorInfo noArgCtor = Array.Find(ctors, c => c.GetParameters().Length == 0);

                if (noArgCtor == null) throw new Exception("Without NoArgCtor " + type.Name);

                T tmp = noArgCtor.Invoke(null) as T;
                Interlocked.CompareExchange(ref _instance, tmp, null);
            }

            return _instance;
        }
    }
}
