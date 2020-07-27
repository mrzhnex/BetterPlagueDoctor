using Exiled.API.Interfaces;

namespace BetterPlagueDoctor
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
    }
}