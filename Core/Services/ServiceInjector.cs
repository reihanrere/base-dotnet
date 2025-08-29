namespace BaseDotnet.Core.Services;

public static class ServiceInjector
{
    public static void Inject(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
    }
}