using Exiled.API.Features;

namespace BetterPlagueDoctor
{
    public class MainSettings : Plugin<Config>
    {
        public override string Name => nameof(BetterPlagueDoctor);
        public SetEvents SetEvents { get; set; }

        public override void OnEnabled()
        {
            SetEvents = new SetEvents();
            Exiled.Events.Handlers.Server.RoundStarted += SetEvents.OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers += SetEvents.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.SendingConsoleCommand += SetEvents.OnSendingConsoleCommand;
            Exiled.Events.Handlers.Player.InteractingDoor += SetEvents.OnInteractingDoor;
            Log.Info(Name + " on");
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= SetEvents.OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= SetEvents.OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.SendingConsoleCommand -= SetEvents.OnSendingConsoleCommand;
            Exiled.Events.Handlers.Player.InteractingDoor -= SetEvents.OnInteractingDoor;
            Log.Info(Name + " off");
        }
    }
}