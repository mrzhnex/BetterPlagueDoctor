using EXILED.Extensions;
using UnityEngine;

namespace BetterPlagueDoctor
{
    public class CureCurrentBody : MonoBehaviour
    {
        private ReferenceHub scp049;
        private float timer = 0f;
        private readonly float timeIsUp = 1.0f;
        private Ragdoll curBody;
        public ReferenceHub curZombie;
        private float time_to_make_zombie = 0f;

        public void Start()
        {
            scp049 = Player.GetPlayer(gameObject);

            foreach (Ragdoll rd in FindObjectsOfType<Ragdoll>())
            {
                if (Global.damageTypes.Contains(rd.owner.DeathCause.GetDamageType()))
                {
                    if (Vector3.Distance(gameObject.transform.position, rd.transform.position) < Global.distance)
                    {
                        curBody = rd;
                        curZombie = Player.GetPlayer(rd.owner.PlayerId);
                        break;
                    }
                }
            }
        }

        public void Update()
        {
            timer += Time.deltaTime;
            if (timer > timeIsUp)
            {
                timer = 0f;

                if (Vector3.Distance(gameObject.transform.position, curBody.transform.position) < Global.distance)
                {
                    curZombie = Player.GetPlayer(curZombie.GetPlayerId());
                    if (curZombie != null && curZombie.GetRole() == RoleType.Spectator)
                    {
                        time_to_make_zombie += timeIsUp;
                        scp049.ClearBroadcasts();
                        scp049.Broadcast(1, string.Concat(new object[]
                        {
                            "<color=#228b22>Вы поднимаете ",
                            curZombie.nicknameSync.Network_myNickSync,
                            " осталось: ",
                            Global.timeToMakeZombie - time_to_make_zombie + "</color>"
                        }), true);
                    }
                    else
                    {
                        scp049.ClearBroadcasts();
                        scp049.Broadcast(10, "<color=#ff0000>" + curZombie.nicknameSync.Network_myNickSync + " внезапно стал неподвластен вашему лечению</color>", true);
                        Destroy(gameObject.GetComponent<CureCurrentBody>());
                    }

                    if (time_to_make_zombie >= Global.timeToMakeZombie)
                    {
                        curZombie.SetRole(RoleType.Scp0492, true);
                        curZombie.SetPosition(curBody.gameObject.transform.position + Vector3.up);
                        Destroy(curBody.gameObject, 0f);
                        scp049.ClearBroadcasts();
                        scp049.Broadcast(10, "<color=#228b22>" + curZombie.nicknameSync.Network_myNickSync + " был излечен</color>", true);
                        Destroy(gameObject.GetComponent<CureCurrentBody>());
                    }
                }
                else
                {
                    scp049.ClearBroadcasts();
                    scp049.Broadcast(10, "<color=#228b22>Вы прекратили лечение " + curZombie.nicknameSync.Network_myNickSync + "</color>", true);
                    Destroy(gameObject.GetComponent<CureCurrentBody>());
                }
            }
        }
    }
}