using Microsoft.Extensions.Hosting;

namespace JackOfAllTrades.Abstractions
{
    public interface IRegisterServices
    {  
        IHostApplicationBuilder RegisterServices(IHostApplicationBuilder services);
    }
}
