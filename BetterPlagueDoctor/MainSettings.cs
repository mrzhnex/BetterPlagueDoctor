using EXILED;

namespace BetterPlagueDoctor
{
    public class MainSettings : Plugin
    {
        public override string getName => "BetterPlagueDoctor";
        private SetEvents SetEvents;

        public override void OnEnable()
        {
            SetEvents = new SetEvents();
            Events.RoundStartEvent += SetEvents.OnRoundStart;
            Events.WaitingForPlayersEvent += SetEvents.OnWaitingForPlayers;
            Events.ConsoleCommandEvent += SetEvents.OnCallCommand;
            Events.DoorInteractEvent += SetEvents.OnDoorInteract;
            Log.Info(getName + " on");
        }

        public override void OnDisable()
        {
            Events.RoundStartEvent -= SetEvents.OnRoundStart;
            Events.WaitingForPlayersEvent -= SetEvents.OnWaitingForPlayers;
            Events.ConsoleCommandEvent -= SetEvents.OnCallCommand;
            Events.DoorInteractEvent -= SetEvents.OnDoorInteract;
            Log.Info(getName + " off");
        }

        public override void OnReload() { }
    }
}