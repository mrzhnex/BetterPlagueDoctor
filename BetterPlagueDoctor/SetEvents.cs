using EXILED;
using EXILED.Extensions;
using UnityEngine;

namespace BetterPlagueDoctor
{
    public class SetEvents
    {
        public void OnCallCommand(ConsoleCommandEvent ev)
        {
            if (!Global.can_use_commands)
            {
                ev.ReturnMessage = "Дождитесь начала раунда!";
                return;
            }
            string command = ev.Command.ToLower();

            if (command == "cure")
            {
                if (ev.Player.GetRole() != RoleType.Scp049)
                {
                    ev.ReturnMessage = Global._not049;
                    return;
                }
                GameObject gameObject = ev.Player.gameObject;
                if (gameObject.GetComponent<CureCurrentBody>() != null)
                {
                    ev.ReturnMessage = Global._isalreadycure + gameObject.GetComponent<CureCurrentBody>().curZombie.nicknameSync.Network_myNickSync;
                    return;
                }
                foreach (Ragdoll rd in UnityEngine.Object.FindObjectsOfType<Ragdoll>())
                {
                    if (Global.damageTypes.Contains(rd.owner.DeathCause.GetDamageType()))
                    {
                        if (Vector3.Distance(gameObject.transform.position, rd.transform.position) <= Global.distance)
                        {
                            if (Player.GetPlayer(rd.owner.PlayerId) != null && Player.GetPlayer(rd.owner.PlayerId).GetRole() == RoleType.Spectator)
                            {
                                gameObject.AddComponent<CureCurrentBody>();
                                ev.ReturnMessage = Global._successstart + Player.GetPlayer(rd.owner.PlayerId).nicknameSync.Network_myNickSync;
                                return;
                            }
                        }
                    }
                }
                ev.ReturnMessage = Global._istoolargefor049_2;
                return;
            }
            else if (command == "stopcure")
            {
                if (ev.Player.GetRole() != RoleType.Scp049)
                {
                    ev.ReturnMessage = Global._not049;
                    return;
                }
                GameObject gameObject = ev.Player.gameObject;
                if (gameObject.GetComponent<CureCurrentBody>() != null)
                {
                    ev.ReturnMessage = Global._stopcure + gameObject.GetComponent<CureCurrentBody>().curZombie.nicknameSync.Network_myNickSync;
                    UnityEngine.Object.Destroy(gameObject.GetComponent<CureCurrentBody>());
                    return;
                }
                else
                {
                    ev.ReturnMessage = Global._isnotcure;
                    return;
                }
            }
            else if (command.Contains("gate049"))
            {
                if (ev.Player.GetTeam() == Team.SCP && ev.Player.GetRole() != RoleType.Scp049 || ev.Player.GetRole() == RoleType.Spectator)
                {
                    ev.ReturnMessage = "Вы не можете использовать эту команду";
                    return;
                }
                if (Vector3.Distance(Global.SCP049GateWorkstation.transform.position, ev.Player.gameObject.transform.position) < 3.0f)
                {
                    if (command.Contains("open"))
                    {
                        if (Global.SCP049Gate.NetworkisOpen)
                        {
                            ev.ReturnMessage = "Гермоворота уже открыты";
                            return;
                        }
                        else
                        {
                            Global.SCP049Gate.NetworkisOpen = true;
                            ev.ReturnMessage = "Вы открыли гермоворота";
                            return;
                        }
                    }
                    else if (command.Contains("close"))
                    {
                        if (Global.SCP049Gate.NetworkisOpen)
                        {
                            Global.SCP049Gate.NetworkisOpen = false;
                            ev.ReturnMessage = "Вы закрыли гермоворота";
                            return;
                        }
                        else
                        {
                            ev.ReturnMessage = "Гермоворота уже закрыты";
                            return;
                        }
                    }
                    else if (command.Contains("unlock"))
                    {
                        if (Global.SCP049GateIsLock)
                        {
                            Global.SCP049GateIsLock = false;
                            ev.ReturnMessage = "Вы разблокировали гермоворота";
                            return;
                        }
                        else
                        {
                            ev.ReturnMessage = "Гермоворота уже разблокированы";
                            return;
                        }
                    }
                    else if (command.Contains("lock"))
                    {
                        if (Global.SCP049GateIsLock)
                        {
                            ev.ReturnMessage = "Гермоворота уже заблокированы";
                            return;
                        }
                        else
                        {
                            Global.SCP049GateIsLock = true;
                            ev.ReturnMessage = "Вы заблокировали гермоворота";
                            return;
                        }
                    }
                    else
                    {
                        ev.ReturnMessage = "Использование: gate049 <open|close|unlock|lock>";
                        return;
                    }
                }
                else
                {
                    ev.ReturnMessage = "Вы слишком далеко от рычага";
                    return;
                }
            }
        }

        internal void OnDoorInteract(ref DoorInteractionEvent ev)
        {
            if (ev.Door == Global.SCP049Gate && Global.SCP049GateIsLock)
            {
                ev.Allow = false;
            }
        }

        public void OnRoundStart()
        {
            Door armory049 = null;
            Global.SCP049GateIsLock = false;
            foreach (Door door in Map.Doors)
            {
                if (door.DoorName.ToLower().Contains("049") && door.DoorName.ToLower().Contains("armory"))
                {
                    armory049 = door;
                    break;
                }
            }
            foreach (Door door in Map.Doors)
            {
                if (Vector3.Distance(armory049.gameObject.transform.position, door.gameObject.transform.position) < 15.0f)
                {
                    if (door.DoorName == armory049.DoorName)
                        continue;
                    Global.SCP049Gate = door;
                    break;
                }
            }
            foreach (WorkStation workStation in UnityEngine.Object.FindObjectsOfType<WorkStation>())
            {
                if (Vector3.Distance(armory049.gameObject.transform.position, workStation.gameObject.transform.position) < 15.0f)
                {
                    Global.SCP049GateWorkstation = workStation;
                }
            }
            Global.can_use_commands = true;
        }

        public void OnWaitingForPlayers()
        {
            Global.can_use_commands = false;
        }
    }
}