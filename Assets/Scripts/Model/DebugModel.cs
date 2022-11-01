
public class DebugModel : BaseSingletonNoLaze<DebugModel>
{
    public BindableProperty<string> Message = new BindableProperty<string>()
    {
        Value = "test"
    };
}
