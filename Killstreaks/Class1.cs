using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinityScript;
namespace KillStreaks
{
    public class KillStreaks : BaseScript
    {
        List<Entity> AlklPlayer = new List<Entity>();
        public KillStreaks()
            : base()
        {
            base.PlayerConnected += connected;
            base.PlayerDisconnected += dissconnect;
        }
        public void connected(Entity player)
        {
            AlklPlayer.Add(player);
            player.SetField("pkills", 0);
            player.SetField("pSpeed", 1);
            player.SetField("pKillsAlle", 0);
            HudElem LabelB = HudElem.CreateFontString(player, "hudbig", 0.5f);
            LabelB.SetPoint("TOPRIGHT", "TOPRIGHT", -60, 80);
            LabelB.SetText("...");
            LabelB.HideWhenInMenu = true;

            HudElem LabelA = HudElem.CreateFontString(player, "hudbig", 0.5f);
            LabelA.SetPoint("TOPLEFT", "TOPLEFT", 10, 110);
            LabelA.SetText("^2FRIENDLY");
            LabelA.HideWhenInMenu = true;

            HudElem LabelC = HudElem.CreateFontString(player, "hudbig", 0.5f);
            LabelC.SetPoint("TOPLEFT", "TOPLEFT", 10, 120);
            LabelC.SetText("^2FRIENDLY");
            LabelC.HideWhenInMenu = true;

            
            OnInterval(50, () =>
            {
                int a = 0;
                int b = 0;
                foreach (Entity plo in AlklPlayer)
                {
                    a = a + 1;
                    if (plo.GetField<int>("pKillsAlle") > 4)
                    {
                        b = b + 1;
                    }
                }

                LabelB.SetText(b + "/" + a + " use ^2LSD");
                LabelA.SetText(player.GetField<int>("pkills") + "/5 ^2for next LSD!");
                LabelC.SetText(player.GetField<int>("pKillsAlle") + "/24 ^2for MOAB!");
                return true;
            });
        }
        public void dissconnect(Entity player)
        {
            AlklPlayer.Remove(player);
        }
        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            player.SetField("pkills", 0);
            player.SetField("pKillsAlle", 0);
            player.SetField("pSpeed", 1);
            player.Call("thermalvisionoff");
            attacker.SetField("pkills", attacker.GetField<int>("pkills") + 1);
            attacker.SetField("pKillsAlle", attacker.GetField<int>("pKillsAlle") + 1);
            //tell(attacker, "you have " + attacker.GetField<int>("pkills") + " Kills");
            if (attacker.GetField<int>("pkills") == 5)
            {
                attacker.Call("iprintlnbold", new Parameter[]
				{
					"^2you are now consuming LSD!"
				});
                attacker.Call("thermalvisionon");
                Call("iprintln", attacker.Name.ToString() + " ^2use LSD!");
                attacker.SetField("pSpeed", attacker.GetField<int>("pSpeed") + 1);
                //tell(attacker, "^2Speed x" + attacker.GetField<int>("pSpeed"));
                attacker.SetField("pkills", 0);
            }
            if (attacker.GetField<int>("pKillsAlle") >= 25)
            {
                attacker.Call("iprintlnbold", new Parameter[]
				{
					"^2you have a overdose!"
				});
                attacker.SetField("pkills", 0);
                attacker.SetField("pKillsAlle", 0);
                attacker.SetField("pSpeed", 1);
                attacker.Call("suicide");

            }
            //tell(player, "^2Speed reset!");
            attacker.Call("setmovespeedscale", attacker.GetField<int>("pSpeed"));
            player.Call("setmovespeedscale", player.GetField<int>("pSpeed"));
        }
        private T getDvar<T>(string dvar)
        {
            // would switch work here? - no
            if (typeof(T) == typeof(int))
            {
                return Call<T>("getdvarint", dvar);
            }
            else if (typeof(T) == typeof(float))
            {
                return Call<T>("getdvarfloat", dvar);
            }
            else if (typeof(T) == typeof(string))
            {
                return Call<T>("getdvar", dvar);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                return Call<T>("getdvarvector", dvar);
            }
            else
            {
                return default(T);
            }
        }
        private void tell(Entity player, string message)
        {
            Utilities.ExecuteCommand(string.Concat(new object[]
	        {
		        "tell ",
		        player.Call<int>("getentitynumber", new Parameter[0]),
		        " ",
		        message
	        }));
        }
        public List<int> getinfos()
        {
            List<int> infos = new List<int>();
            int a = 0;
            int b = 0;
            foreach (Entity player in AlklPlayer)
            {
                a = a + 1;
                if (player.GetField<int>("pKillsAlle") >= 5)
                {
                    b = b + 1;
                }
            }
            infos[0] = a;
            infos[1] = b;
            return infos;
        }
    }
}
