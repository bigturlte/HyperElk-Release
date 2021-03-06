﻿
using System.Diagnostics;

namespace HyperElk.Core
{
    public class MMHunter : CombatRoutine
    {
        private readonly Stopwatch Trueshot_active = new Stopwatch();

        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        //Spells,Buffs,Debuffs
        private string Steady_Shot = "Steady Shot";
        private string Arcane_Shot = "Arcane Shot";

        private string Aimed_Shot = "Aimed Shot";
        private string Trueshot = "Trueshot";
        private string Rapid_Fire = "Rapid Fire";
        private string Bursting_Shot = "Bursting Shot";


        private string Mend_Pet = "Mend Pet";
        private string Kill_Shot = "Kill Shot";
        private string Multi_Shot = "Multi-Shot";
        private string Misdirection = "Misdirection";


        private string Exhilaration = "Exhilaration";
        private string Survival_of_the_Fittest = "Survival of the Fittest";
        private string Feign_Death = "Feign Death";
        private string Counter_Shot = "Counter Shot";

        private string Double_Tap = "Double Tap";
        private string Chimaera_Shot = "Chimaera Shot";
        private string A_Murder_of_Crows = "A Murder of Crows";
        private string Barrage = "Barrage";
        private string Volley = "Volley";
        private string Explosive_Shot = "Explosive Shot";
        private string Serpent_Sting = "Serpent Sting";
        private string Wild_Spirits = "Wild Spirits";
        private string Resonating_Arrow = "Resonating Arrow";
        private string Flayed_Shot = "Flayed Shot";
        private string Death_Chakram = "Death Chakram";


        private string Aspect_of_the_Turtle = "Aspect of the Turtle";
        private string Precise_Shots = "Precise Shots";
        private string Trick_Shots = "Trick Shots";
        private string Steady_Focus = "Steady Focus";
        private string Lock_and_Load = "Lock and Load";
        private string Dead_Eye = "Dead Eye";
        private string FlayersMark = "Flayer's Mark";
        private string Wild_Mark = "Wild Mark";
        private string HuntersMark = "Hunter's Mark";
        //Misc
        private int PlayerLevel => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 43;
        private bool isMOinRange => API.MouseoverRange <= 40;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        public bool NoCovReady => (API.SpellCDDuration(Wild_Spirits) >= gcd || API.SpellCDDuration(Wild_Spirits) == 0) && (API.SpellCDDuration(Flayed_Shot) >= gcd || API.SpellCDDuration(Flayed_Shot) == 0) && (API.SpellCDDuration(Death_Chakram) >= gcd || API.SpellCDDuration(Death_Chakram) == 0) && (API.SpellCDDuration(Resonating_Arrow) >= gcd || API.SpellCDDuration(Death_Chakram) == 0);

        //Talents
        private bool Talent_A_Murder_of_Crows => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_Serpent_Sting => API.PlayerIsTalentSelected(1, 2);
        private bool Talent_CarefulAim => API.PlayerIsTalentSelected(2, 1);
        private bool Talent_Barrage => API.PlayerIsTalentSelected(2, 2);
        private bool Talent_Explosive_Shot => API.PlayerIsTalentSelected(2, 3);
        private bool Talent_Streamline => API.PlayerIsTalentSelected(4, 2);
        private bool Talent_Chimaera_Shot => API.PlayerIsTalentSelected(4, 3);
        private bool Talent_Steady_Focus => API.PlayerIsTalentSelected(4, 1);
        private bool Talent_Dead_Eye => API.PlayerIsTalentSelected(6, 2);
        private bool Talent_Double_Tap => API.PlayerIsTalentSelected(6, 3);
        private bool Talent_Volley => API.PlayerIsTalentSelected(7, 3);



        //CBProperties
        string[] MisdirectionList = new string[] { "Off", "On AOE", "On" };
        string[] TrueshotList = new string[] { "always", "with Cooldowns" };
        string[] DoubleTapList = new string[] { "always", "with Cooldowns" };
        string[] AMurderofCrowsList = new string[] { "always", "with Cooldowns" };
        string[] VolleyList = new string[] { "always", "with Cooldowns", "On AOE", "never" };
        string[] BloodshedList = new string[] { "always", "with Cooldowns" };


        string[] LegendaryList = new string[] { "always", "with Cooldowns" };



        private int Survival_of_the_FittestLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Survival_of_the_Fittest)];
        private int ExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration)];
        private int PetExhilarationLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Exhilaration + "PET")];
        private int AspectoftheTurtleLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Aspect_of_the_Turtle)];
        private int FeignDeathLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Feign_Death)];
        private int MendPetLifePercent => percentListProp[CombatRoutine.GetPropertyInt(Mend_Pet)];
        private string UseMisdirection => MisdirectionList[CombatRoutine.GetPropertyInt(Misdirection)];
        private string UseDoubleTap => DoubleTapList[CombatRoutine.GetPropertyInt(Double_Tap)];
        private string UseTrueshot => TrueshotList[CombatRoutine.GetPropertyInt(Trueshot)];
        private string UseAMurderofCrows => AMurderofCrowsList[CombatRoutine.GetPropertyInt(A_Murder_of_Crows)];
        private string UseVolley => VolleyList[CombatRoutine.GetPropertyInt(Volley)];
        private bool UseCallPet => CombatRoutine.GetPropertyBool("CallPet");
        private bool Use_HuntersMark => CombatRoutine.GetPropertyBool("huntersmark");
        private bool SurgingShots_enabled => CombatRoutine.GetPropertyBool("SurgingShots");
        private bool eagletalons_true_focus_enabled => CombatRoutine.GetPropertyBool("eagletalons_true_focus");
        private bool AOESwitch_enabled => CombatRoutine.GetPropertyBool("AOE_Switch");
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];

        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];

        private bool LastSpell(string spellname, int spellid)
        {
            return API.LastSpellCastInGame == spellname || API.PlayerCurrentCastSpellID == spellid;
        }
        private float FocusRegen => (10f * (1f + API.PlayerGetHaste)) * (PlayerHasBuff(Trueshot) ? 15 / 10 : 1);
        private float FocusTimeToMax => (API.PlayerMaxFocus - API.PlayerFocus) * 100f / FocusRegen;
        private float AimedShotCastTime => ((250f / (1f + (API.PlayerGetHaste))) / (PlayerHasBuff(Trueshot) ? 2 : 1) * (PlayerHasBuff(Lock_and_Load) ? 0 : 1));
        private float RapidFireChannelTime => 200f / (1f + (API.PlayerGetHaste));
        private float SteadyShot_CastTime => 175f / (1f + (API.PlayerGetHaste));
        private float gcd => API.SpellGCDTotalDuration;
        private bool Playeriscasting => API.PlayerCurrentCastTimeRemaining > 40;
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private bool Race(string race)
        {
            return API.PlayerRaceName == race && PlayerRaceSettings == race;
        }
        private bool ca_active => (Talent_CarefulAim ? true : false) && (API.TargetHealthPercent > 70 ? true : false);

        private float AimedShotCooldown => (1200f / (1f + (API.PlayerGetHaste))) / (PlayerHasBuff(Trueshot) ? 22 / 10 : 1);
        private float FullRechargeTime(string spellname, float spellcooldown_max)
        {
            return (API.SpellMaxCharges(spellname) - API.SpellCharges(spellname)) * spellcooldown_max + API.SpellCDDuration(spellname);
        }

        public override void Initialize()
        {
            CombatRoutine.Name = "Marksman Hunter by Vec";
            API.WriteLog("Welcome to Marksman Hunter Rotation");
            API.WriteLog("Misdirection Macro : /cast [@focus,help][help][@pet,exists] Misdirection");
            API.WriteLog("Mend Pet Macro (revive/call): /cast [mod]Revive Pet; [@pet,dead]Revive Pet; [nopet]Call Pet 1; Mend Pet");
            API.WriteLog("Kill Shot Mouseover - /cast [@mouseover] Kill Shot");


            //Spells
            CombatRoutine.AddSpell(Steady_Shot, "D1");
            CombatRoutine.AddSpell(Arcane_Shot, "D2");
            CombatRoutine.AddSpell(Aimed_Shot, "D3");
            CombatRoutine.AddSpell(Trueshot, "Q");
            CombatRoutine.AddSpell(Rapid_Fire, "Q");
            CombatRoutine.AddSpell(Bursting_Shot, "D7");

            CombatRoutine.AddSpell(Kill_Shot, "D5");
            CombatRoutine.AddSpell(Multi_Shot, "D6");

            CombatRoutine.AddSpell(Counter_Shot, "F");
            CombatRoutine.AddSpell(Exhilaration, "F9");
            CombatRoutine.AddSpell(Survival_of_the_Fittest, "F8");
            CombatRoutine.AddSpell(Misdirection, "D4");

            CombatRoutine.AddSpell(Double_Tap, "D1");
            CombatRoutine.AddSpell(Chimaera_Shot, "D1");
            CombatRoutine.AddSpell(A_Murder_of_Crows, "D1");
            CombatRoutine.AddSpell(Barrage, "D1");
            CombatRoutine.AddSpell(Volley, "D1");
            CombatRoutine.AddSpell(Explosive_Shot, "D1");
            CombatRoutine.AddSpell(Serpent_Sting, "D1");
            CombatRoutine.AddSpell(Feign_Death, "F2");
            CombatRoutine.AddSpell(Aspect_of_the_Turtle, "G");

            CombatRoutine.AddSpell(Mend_Pet, "F5");

            CombatRoutine.AddSpell(Wild_Spirits, "F10");
            CombatRoutine.AddSpell(Resonating_Arrow, "F10");
            CombatRoutine.AddSpell(Flayed_Shot, "F10");
            CombatRoutine.AddSpell(Death_Chakram, "F10");
            CombatRoutine.AddSpell(HuntersMark, "F11");

            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");
            //Buffs

            CombatRoutine.AddBuff(Aspect_of_the_Turtle);
            CombatRoutine.AddBuff(Feign_Death);
            CombatRoutine.AddBuff(Misdirection);
            CombatRoutine.AddBuff(Precise_Shots);
            CombatRoutine.AddBuff(Trick_Shots);
            CombatRoutine.AddBuff(Steady_Focus);
            CombatRoutine.AddBuff(Trueshot);
            CombatRoutine.AddBuff(Double_Tap);
            CombatRoutine.AddBuff(Lock_and_Load);
            CombatRoutine.AddBuff(Dead_Eye);
            CombatRoutine.AddBuff(FlayersMark);
            CombatRoutine.AddBuff(Volley);
            //Debuffs

            CombatRoutine.AddDebuff(Serpent_Sting);

            CombatRoutine.AddDebuff(Wild_Mark);
            CombatRoutine.AddDebuff(Resonating_Arrow);
            CombatRoutine.AddDebuff(HuntersMark);
            //Macros
            CombatRoutine.AddMacro(Kill_Shot + "MO", "NumPad7");


            //Toggle
            CombatRoutine.AddToggle("Mouseover");
            AddProp("MouseoverInCombat", "Only Mouseover in combat", true, "Only Attack mouseover in combat to avoid stupid pulls", "Generic");

            //Settings
            CombatRoutine.AddProp(Survival_of_the_Fittest, "Use " + Survival_of_the_Fittest + " below:", percentListProp, "Life percent at which " + Survival_of_the_Fittest + " is used, set to 0 to disable", "Defense", 7);
            CombatRoutine.AddProp(Misdirection, "Use Misdirection", MisdirectionList, "Use " + Misdirection + " Off, On AOE, On", "Generic", 0);
            CombatRoutine.AddProp(Trueshot, "Use " + Trueshot, TrueshotList, "Use " + Trueshot + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Double_Tap, "Use " + Double_Tap, DoubleTapList, "Use " + Double_Tap + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(A_Murder_of_Crows, "Use " + A_Murder_of_Crows, AMurderofCrowsList, "Use " + A_Murder_of_Crows + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp(Volley, "Use " + Volley, VolleyList, "Use " + Volley + " always, with Cooldowns, On AOE, never", "Cooldowns", 0);

            CombatRoutine.AddProp("AOE_Switch", "AoE Switch", true, "Enable if you want to let the rotation switch ST/AOE", "Generic");
            CombatRoutine.AddProp("huntersmark", "Hunter's Mark", false, "Enable if you want to let the rotation use Hunter's Mark", "Generic");

            CombatRoutine.AddProp("SurgingShots", "Surging Shots", false, "Enable if you have Surging Shots", "Legendary");
            CombatRoutine.AddProp("eagletalons_true_focus", "eagletalons true focus", false, "Enable if you have eagletalons true focus", "Legendary");
            CombatRoutine.AddProp("CallPet", "Call/Ressurect Pet", false, "Should the rotation try to ressurect/call your Pet", "Pet");
            CombatRoutine.AddProp("Trinket1", "Use " + "Use Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp(Exhilaration, "Use " + Exhilaration + " below:", percentListProp, "Life percent at which " + Exhilaration + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Exhilaration + "PET", "Use " + Exhilaration + " below:", percentListProp, "Life percent at which " + Exhilaration + " is used to heal your pet, set to 0 to disable", "Pet", 2);
            CombatRoutine.AddProp(Aspect_of_the_Turtle, "Use " + Aspect_of_the_Turtle + " below:", percentListProp, "Life percent at which " + Aspect_of_the_Turtle + " is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(Feign_Death, "Use " + Feign_Death + " below:", percentListProp, "Life percent at which " + Feign_Death + " is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(Mend_Pet, "Use " + Mend_Pet + " below:", percentListProp, "Life percent at which " + Mend_Pet + " is used, set to 0 to disable", "Pet", 6);
            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
        }

        public override void Pulse()
        {
            // API.WriteLog("debug: " + ca_active);

            if (UseCallPet && (!API.PlayerHasPet || API.PetHealthPercent < 1))
            {
                API.CastSpell(Mend_Pet);
                return;
            }
            if (API.PlayerHasPet && API.PetHealthPercent >= 1 && API.PetHealthPercent <= MendPetLifePercent && API.CanCast(Mend_Pet))
            {
                API.CastSpell(Mend_Pet);
                return;
            }
        }
        public override void CombatPulse()
        {
            if (API.CanCast(HuntersMark) && Use_HuntersMark && !API.TargetHasDebuff(HuntersMark) && InRange)
            {
                API.CastSpell(HuntersMark);
                return;
            }
            if (API.FocusHealthPercent > 0 && API.CanCast(Misdirection) && !API.PlayerHasBuff(Misdirection) && PlayerLevel >= 21 && (UseMisdirection == "On" || (UseMisdirection == "On AOE" & IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)))
            {
                API.CastSpell(Misdirection);
                return;
            }

            if (API.CanCast(Exhilaration) && API.PlayerHealthPercent <= ExhilarationLifePercent && PlayerLevel >= 9)
            {
                API.CastSpell(Exhilaration);
                return;
            }
            if (API.CanCast(Survival_of_the_Fittest) && API.PlayerHealthPercent <= Survival_of_the_FittestLifePercent && PlayerLevel >= 9)
            {
                API.CastSpell(Survival_of_the_Fittest);
                return;
            }
            if (API.CanCast(Aspect_of_the_Turtle) && API.PlayerHealthPercent <= AspectoftheTurtleLifePercent && PlayerLevel >= 8)
            {
                API.CastSpell(Aspect_of_the_Turtle);
                return;
            }
            if (API.CanCast(Feign_Death) && API.PlayerHealthPercent <= FeignDeathLifePercent && PlayerLevel >= 6)
            {
                API.CastSpell(Feign_Death);
                return;
            }
            if (!Playeriscasting && !API.PlayerIsMounted && !API.PlayerHasBuff(Aspect_of_the_Turtle) && !API.PlayerHasBuff(Feign_Death))
            {
                if (isInterrupt && API.CanCast(Counter_Shot) && InRange && PlayerLevel >= 18)
                {
                    API.CastSpell(Counter_Shot);
                    return;
                }

                rotation();
                return;
            }
        }

        public override void OutOfCombatPulse()
        {
        }


        private void rotation()
        {
            if (API.LastSpellCastInGame == Trueshot)
            {
                API.WriteLog("Trueshot window open");
                Trueshot_active.Start();
            }
            if (Trueshot_active.ElapsedMilliseconds > 15000)
            {
                API.WriteLog("Trueshot window closed");
                Trueshot_active.Stop();
                Trueshot_active.Reset();
            }







            // actions.cds = berserking,if= buff.trueshot.up | target.time_to_die < 13
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Troll" && (PlayerHasBuff(Trueshot) || API.TargetTimeToDie < 1300))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            // actions.cds +=/ blood_fury,if= buff.trueshot.up | target.time_to_die < 16
            if (API.CanCast(RacialSpell1) && isRacial && PlayerRaceSettings == "Orc" && (PlayerHasBuff(Trueshot) || API.TargetTimeToDie < 1600))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            // actions.cds +=/ ancestral_call,if= buff.trueshot.up | target.time_to_die < 16
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Mag'har Orc" && (PlayerHasBuff(Trueshot) || API.TargetTimeToDie < 1600))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            // actions.cds +=/ fireblood,if= buff.trueshot.up | target.time_to_die < 9
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Dark Iron Dwarf" && (PlayerHasBuff(Trueshot) || API.TargetTimeToDie < 900))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            // actions.cds +=/ lights_judgment,if= buff.trueshot.down
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Lightforged Draenei" && !PlayerHasBuff(Trueshot))
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            // actions.cds +=/ bag_of_tricks,if= buff.trueshot.down
            if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Vulpera" && !PlayerHasBuff(Trueshot))
            {
                API.CastSpell(RacialSpell1);
                return;
            }

            // actions.cds +=/ potion,if= buff.trueshot.up & buff.bloodlust.up | buff.trueshot.up & target.health.pct < 20 | target.time_to_die < 26

            #region opener

            if (API.PlayerTimeInCombat <= 1700 && IsCooldowns && (!IsAOE || (AOESwitch_enabled && API.TargetUnitInRangeCount < AOEUnitNumber && IsAOE)))
            {
                if (API.CanCast(Double_Tap))
                {
                    API.CastSpell(Double_Tap);
                    return;
                }
                if (API.CanCast(Aimed_Shot) && API.SpellCharges(Aimed_Shot) >= 2 && !PlayerHasBuff(Trueshot))
                {
                    API.CastSpell(Aimed_Shot);
                    return;
                }
                if (Talent_Steady_Focus && API.CanCast(Steady_Shot) && API.LastSpellCastInGame != Steady_Shot && API.PlayerCurrentCastSpellID != 56641 && !PlayerHasBuff(Steady_Focus) && InRange)
                {
                    API.CastSpell(Steady_Shot);
                    return;
                }
                if (Talent_Steady_Focus && API.CanCast(Steady_Shot) && API.LastSpellCastInGame != Steady_Shot && API.PlayerCurrentCastSpellID == 56641 && API.PlayerBuffTimeRemaining(Steady_Focus) < 500 && InRange)
                {
                    API.CastSpell(Steady_Shot);
                    return;
                }
                if (Talent_Steady_Focus && API.CanCast(Steady_Shot) && API.LastSpellCastInGame == Steady_Shot && API.PlayerCurrentCastSpellID != 56641 && API.PlayerBuffTimeRemaining(Steady_Focus) < 500 && InRange)
                {
                    API.CastSpell(Steady_Shot);
                    return;
                }
                if (API.CanCast(Explosive_Shot) && API.PlayerFocus >= 20 && InRange && Talent_Explosive_Shot)
                {
                    API.CastSpell(Explosive_Shot);
                    return;
                }
                if (API.CanCast(Wild_Spirits) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Wild_Spirits);
                    return;
                }
                if (API.CanCast(Resonating_Arrow) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Resonating_Arrow);
                    return;
                }
                if (API.CanCast(Volley) && InRange)
                {
                    API.CastSpell(Volley);
                    return;
                }
                if (API.CanCast(Trueshot) && InRange && (API.TargetHasDebuff(Resonating_Arrow) || API.TargetHasDebuff(Wild_Mark) || (API.SpellCDDuration(Resonating_Arrow) > gcd && API.SpellCDDuration(Wild_Mark) > gcd)))
                {
                    API.CastSpell(Trueshot);
                    return;
                }
                if (API.CanCast(Aimed_Shot) && InRange && (API.PlayerHasBuff(Lock_and_Load) || !API.PlayerIsMoving) && API.PlayerFocus > 35)
                {
                    API.CastSpell(Aimed_Shot);
                    return;
                }
                if (API.CanCast(Aimed_Shot) && InRange && (API.PlayerHasBuff(Lock_and_Load) || !API.PlayerIsMoving) && API.PlayerFocus >= (PlayerHasBuff(Lock_and_Load) ? 0 : 35) && FullRechargeTime(Aimed_Shot, AimedShotCooldown) < gcd + AimedShotCastTime)
                {
                    API.CastSpell(Aimed_Shot);
                    return;
                }
                if (API.CanCast(Rapid_Fire) && !eagletalons_true_focus_enabled && InRange && API.PlayerFocus + FocusRegen * (gcd / 100) + 7 < API.PlayerMaxFocus && FullRechargeTime(Aimed_Shot, AimedShotCooldown) > RapidFireChannelTime)
                {
                    API.CastSpell(Rapid_Fire);
                    return;
                }
                if (API.CanCast(Arcane_Shot) && InRange && API.PlayerFocus > 50 && FullRechargeTime(Aimed_Shot, AimedShotCooldown) > gcd && !API.CanCast(Rapid_Fire))
                {
                    API.CastSpell(Arcane_Shot);
                    return;
                }
                if (API.CanCast(Steady_Shot) && InRange && API.PlayerFocus + (10 + (SteadyShot_CastTime / 100) * FocusRegen) < 120 && (!API.CanCast(Rapid_Fire) || PlayerHasBuff(Double_Tap)) && (!PlayerHasBuff(Precise_Shots) || PlayerHasBuff(Precise_Shots) && API.PlayerFocus < 20) && (FullRechargeTime(Aimed_Shot, AimedShotCooldown) > SteadyShot_CastTime || API.PlayerFocus < (PlayerHasBuff(Lock_and_Load) ? 0 : 35) || API.PlayerIsMoving))
                {
                    API.CastSpell(Steady_Shot);
                    return;
                }
            }
            #endregion
            //API.WriteLog("cov not ready: " + NoCovReady);
            if (!IsAOE || (AOESwitch_enabled && API.TargetUnitInRangeCount < AOEUnitNumber && IsAOE))
            {
                #region cooldowns
                if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && (UseTrinket1 == "With Cooldowns" && IsCooldowns || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell("Trinket1");
                }
                if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && (UseTrinket2 == "With Cooldowns" && IsCooldowns || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell("Trinket2");
                }
                #endregion
                #region ST

                if (PlayerHasBuff(Trueshot))
                {
                    if (Talent_Steady_Focus && API.CanCast(Steady_Shot) && API.LastSpellCastInGame != Steady_Shot && API.PlayerCurrentCastSpellID == 56641 && API.PlayerBuffTimeRemaining(Steady_Focus) < 500 && InRange)
                    {
                        API.CastSpell(Steady_Shot);
                        return;
                    }
                    if (API.CanCast(Aimed_Shot) && InRange && (API.PlayerHasBuff(Lock_and_Load) || !API.PlayerIsMoving) && API.PlayerFocus > 35)
                    {
                        API.CastSpell(Aimed_Shot);
                        return;
                    }
                    if (API.CanCast(Aimed_Shot) && InRange && (API.PlayerHasBuff(Lock_and_Load) || !API.PlayerIsMoving) && API.PlayerFocus >= (PlayerHasBuff(Lock_and_Load) ? 0 : 35) && FullRechargeTime(Aimed_Shot, AimedShotCooldown) < gcd + AimedShotCastTime)
                    {
                        API.CastSpell(Aimed_Shot);
                        return;
                    }
                    if (API.CanCast(Rapid_Fire) && !eagletalons_true_focus_enabled && InRange && API.PlayerFocus + FocusRegen * (gcd / 100) + 7 < API.PlayerMaxFocus && FullRechargeTime(Aimed_Shot, AimedShotCooldown) > RapidFireChannelTime)
                    {
                        API.CastSpell(Rapid_Fire);
                        return;
                    }
                    if (API.CanCast(Arcane_Shot) && InRange && API.PlayerFocus > 50 && FullRechargeTime(Aimed_Shot, AimedShotCooldown) > gcd && !API.CanCast(Rapid_Fire))
                    {
                        API.CastSpell(Arcane_Shot);
                        return;
                    }
                    if (API.CanCast(Steady_Shot) && InRange && API.PlayerFocus + (10 + (SteadyShot_CastTime / 100) * FocusRegen) < 120 && (!API.CanCast(Rapid_Fire) || PlayerHasBuff(Double_Tap)) && (!PlayerHasBuff(Precise_Shots) || PlayerHasBuff(Precise_Shots) && API.PlayerFocus < 20) && (FullRechargeTime(Aimed_Shot, AimedShotCooldown) > SteadyShot_CastTime || API.PlayerFocus < (PlayerHasBuff(Lock_and_Load) ? 0 : 35) || API.PlayerIsMoving))
                    {
                        API.CastSpell(Steady_Shot);
                        return;
                    }
                }
                if (!PlayerHasBuff(Trueshot))
                {
                    if (Talent_Steady_Focus && API.CanCast(Steady_Shot) && API.LastSpellCastInGame != Steady_Shot && API.PlayerCurrentCastSpellID == 56641 && API.PlayerBuffTimeRemaining(Steady_Focus) < 500 && InRange)
                    {
                        API.CastSpell(Steady_Shot);
                        return;
                    }
                    if (Talent_Steady_Focus && API.CanCast(Steady_Shot) && API.LastSpellCastInGame == Steady_Shot && API.PlayerCurrentCastSpellID != 56641 && API.PlayerBuffTimeRemaining(Steady_Focus) < 500 && InRange)
                    {
                        API.CastSpell(Steady_Shot);
                        return;
                    }
                    //actions.st +=/ kill_shot
                    if ((API.TargetHealthPercent <= 20 || PlayerHasBuff(FlayersMark)) && API.CanCast(Kill_Shot) && InRange && PlayerLevel >= 42 && API.PlayerFocus >= 10)
                    {
                        API.CastSpell(Kill_Shot);
                        return;
                    }
                    if (API.CanCast(Kill_Shot) && (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && (API.MouseoverHealthPercent <= 20 || PlayerHasBuff(FlayersMark))) && API.PlayerFocus >= 10 && PlayerLevel >= 42)
                    {
                        API.CastSpell(Kill_Shot + "MO");
                        return;
                    }
                    //actions.st +=/ double_tap,if= covenant.kyrian & cooldown.resonating_arrow.remains < gcd | !covenant.kyrian & !covenant.night_fae | covenant.night_fae & (cooldown.wild_spirits.remains < gcd | cooldown.trueshot.remains > 55) | target.time_to_die < 15
                    if (API.CanCast(Double_Tap) && (UseDoubleTap == "always" || (UseDoubleTap == "with Cooldowns" && IsCooldowns)) && InRange && Talent_Double_Tap && (API.SpellCDDuration(Aimed_Shot) < API.SpellCDDuration(Rapid_Fire))
         && (PlayerCovenantSettings == "Kyrian" && API.SpellCDDuration(Resonating_Arrow) < gcd || PlayerCovenantSettings != "Kyrian" && PlayerCovenantSettings != "Night Fae" || PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(Wild_Spirits) < gcd || API.SpellCDDuration(Trueshot) > 5500) || API.TargetTimeToDie < 1500))
                    {
                        API.CastSpell(Double_Tap);
                        return;
                    }
                    //actions.st = steady_shot,if= talent.steady_focus & (prev_gcd.1.steady_shot & buff.steady_focus.remains < 5 | buff.steady_focus.down)
                    if (Talent_Steady_Focus && API.CanCast(Steady_Shot) && API.LastSpellCastInGame != Steady_Shot && API.PlayerCurrentCastSpellID != 56641 && !PlayerHasBuff(Steady_Focus) && InRange)
                    {
                        API.CastSpell(Steady_Shot);
                        return;
                    }
                    //actions.st +=/ flare,if= tar_trap.up & runeforge.soulforge_embers
                    //actions.st +=/ tar_trap,if= runeforge.soulforge_embers & tar_trap.remains < gcd & cooldown.flare.remains < gcd
                    //actions.st +=/ explosive_shot
                    if (API.CanCast(Explosive_Shot) && API.PlayerFocus >= 20 && InRange && Talent_Explosive_Shot)
                    {
                        API.CastSpell(Explosive_Shot);
                        return;
                    }
                    //actions.st +=/ wild_spirits
                    if (API.CanCast(Wild_Spirits) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Wild_Spirits);
                        return;
                    }
                    //actions.st +=/ flayed_shot
                    if (API.CanCast(Flayed_Shot) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Flayed_Shot);
                        return;
                    }
                    //actions.st +=/ death_chakram,if= focus + cast_regen < focus.max
                    if (API.CanCast(Death_Chakram) && API.PlayerFocus + FocusRegen * gcd / 100 < API.PlayerMaxFocus && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Death_Chakram);
                        return;
                    }
                    //actions.st +=/ a_murder_of_crows
                    if (Talent_A_Murder_of_Crows && (UseAMurderofCrows == "always" || (UseAMurderofCrows == "with Cooldowns" && IsCooldowns)) && API.CanCast(A_Murder_of_Crows) && InRange && API.PlayerFocus >= 20)
                    {
                        API.CastSpell(A_Murder_of_Crows);
                        return;
                    }
                    //actions.st +=/ resonating_arrow
                    if (API.CanCast(Resonating_Arrow) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                    {
                        API.CastSpell(Resonating_Arrow);
                        return;
                    }
                    //actions.st +=/ volley,if= buff.precise_shots.down | !talent.chimaera_shot | active_enemies < 2
                    if (API.CanCast(Volley) && (PlayerHasBuff(Precise_Shots) || !Talent_Chimaera_Shot || API.TargetUnitInRangeCount < 2) && (UseVolley == "always" || (UseVolley == "with Cooldowns" && IsCooldowns) || (UseVolley == "On AOE" && IsAOE && API.TargetUnitInRangeCount >= AOEUnitNumber)) && InRange && Talent_Volley)
                    {
                        API.CastSpell(Volley);
                        return;
                    }
                    //actions.st +=/ trueshot,if= buff.precise_shots.down | buff.resonating_arrow.up | buff.wild_spirits.up | buff.volley.up & active_enemies > 1
                    if (API.CanCast(Trueshot) && NoCovReady && (UseTrueshot == "always" || (UseTrueshot == "with Cooldowns" && IsCooldowns)) && InRange &&
                        (!PlayerHasBuff(Precise_Shots) || API.TargetHasDebuff(Resonating_Arrow) || API.TargetHasDebuff(Wild_Mark) || PlayerHasBuff(Volley) && API.TargetRange > 1))
                    {
                        API.CastSpell(Trueshot);
                        return;
                    }
                    if (API.CanCast(Rapid_Fire) && !PlayerHasBuff(Double_Tap) && InRange && API.PlayerFocus + FocusRegen * (gcd / 100) + 7 < API.PlayerMaxFocus && FullRechargeTime(Aimed_Shot, AimedShotCooldown) >= RapidFireChannelTime)
                    {
                        API.CastSpell(Rapid_Fire);
                        return;
                    }
                    //actions.st +=/ aimed_shot,target_if = min:dot.serpent_sting.remains + action.serpent_sting.in_flight_to_target * 99,if= 
                    //buff.precise_shots.down | (buff.trueshot.up | full_recharge_time < gcd + cast_time) & (!talent.chimaera_shot | active_enemies < 2) | buff.trick_shots.remains > execute_time & active_enemies > 1
                    if (API.CanCast(Aimed_Shot) && InRange && (API.PlayerHasBuff(Lock_and_Load) || !API.PlayerIsMoving) && API.PlayerFocus >= (API.PlayerHasBuff(Lock_and_Load) ? 0 : 35) && !LastSpell(Aimed_Shot, 19434) &&
    (API.TargetDebuffRemainingTime(Serpent_Sting) > 200 || !Talent_Serpent_Sting) &&
    (!PlayerHasBuff(Precise_Shots) || (PlayerHasBuff(Trueshot) || FullRechargeTime(Aimed_Shot, AimedShotCooldown) < gcd + AimedShotCastTime) && (!Talent_Chimaera_Shot || API.TargetUnitInRangeCount < 2) || API.PlayerBuffTimeRemaining(Trick_Shots) > AimedShotCastTime && API.TargetUnitInRangeCount > 1))
                    {
                        API.CastSpell(Aimed_Shot);
                        return;
                    }
                    //actions.st +=/ rapid_fire,if= focus + cast_regen < focus.max & (buff.trueshot.down | !runeforge.eagletalons_true_focus) & (buff.double_tap.down | talent.streamline)
                    if (API.CanCast(Rapid_Fire) && InRange && (API.PlayerFocus + FocusRegen * (gcd / 100) + 7 < API.PlayerMaxFocus && (!PlayerHasBuff(Trueshot) || !eagletalons_true_focus_enabled) && (!PlayerHasBuff(Double_Tap) || Talent_Streamline)))
                    {
                        API.CastSpell(Rapid_Fire);
                        return;
                    }
                    //actions.st +=/ chimaera_shot,if= buff.precise_shots.up | focus > cost + action.aimed_shot.cost
                    if (Talent_Chimaera_Shot && API.CanCast(Chimaera_Shot) && InRange && (API.PlayerHasBuff(Precise_Shots) || API.PlayerFocus > 10 + (API.PlayerHasBuff(Lock_and_Load) ? 0 : 35)))
                    {
                        API.CastSpell(Chimaera_Shot);
                        return;
                    }
                    if (API.CanCast(Arcane_Shot) && (API.SpellCDDuration(Rapid_Fire) > gcd || PlayerHasBuff(Double_Tap)) && InRange && (API.PlayerHasBuff(Precise_Shots) || API.SpellCDDuration(Aimed_Shot) > gcd && API.PlayerFocus > 85))
                    {
                        API.CastSpell(Arcane_Shot);
                        return;
                    }
                    //actions.st +=/ arcane_shot,if= buff.precise_shots.up | focus > cost + action.aimed_shot.cost
                    if (API.CanCast(Arcane_Shot) && (API.SpellCDDuration(Rapid_Fire) > gcd || PlayerHasBuff(Double_Tap)) && InRange && (API.PlayerCurrentCastSpellID == 19434 || API.PlayerHasBuff(Precise_Shots) || (API.SpellCDDuration(Aimed_Shot) > gcd || API.PlayerIsMoving) && API.PlayerFocus > 85))
                    {
                        API.CastSpell(Arcane_Shot);
                        return;
                    }
                    //actions.st +=/ serpent_sting,target_if = min:remains,if= refreshable & target.time_to_die > duration
                    if (Talent_Serpent_Sting && API.CanCast(Serpent_Sting) && API.PlayerFocus > 10 && InRange && (!API.TargetHasDebuff(Serpent_Sting) || API.PlayerDebuffRemainingTime(Serpent_Sting) < 200) && API.TargetTimeToDie >= 1800)
                    {
                        API.CastSpell(Serpent_Sting);
                        return;
                    }
                    //actions.st +=/ barrage,if= active_enemies > 1
                    if (Talent_Barrage && API.CanCast(Barrage) && InRange && IsAOE && API.TargetUnitInRangeCount > 1 && API.PlayerFocus >= 30)
                    {
                        API.CastSpell(Barrage);
                        return;
                    }
                    //actions.st +=/ rapid_fire,if= focus + cast_regen < focus.max & (buff.double_tap.down | talent.streamline)
                    if (API.CanCast(Rapid_Fire) && InRange && (FocusRegen * (gcd / 100) + 7 < API.PlayerMaxFocus && (!PlayerHasBuff(Double_Tap) || Talent_Streamline)) && FullRechargeTime(Aimed_Shot, AimedShotCooldown) > RapidFireChannelTime)
                    {
                        API.CastSpell(Rapid_Fire);
                        return;
                    }
                    //actions.st +=/ steady_shot
                    if (API.CanCast(Steady_Shot) && InRange && API.PlayerFocus + (10 + (SteadyShot_CastTime / 100) * FocusRegen) < 120 && (!API.CanCast(Rapid_Fire) || PlayerHasBuff(Double_Tap)) && (!PlayerHasBuff(Precise_Shots) || PlayerHasBuff(Precise_Shots) && API.PlayerFocus < 20) && (FullRechargeTime(Aimed_Shot, AimedShotCooldown) > SteadyShot_CastTime || API.PlayerFocus < (PlayerHasBuff(Lock_and_Load) ? 0 : 35) || API.PlayerIsMoving))
                    {
                        API.CastSpell(Steady_Shot);
                        return;
                    }
                }


            }
            #endregion
            else
            {
                //actions.trickshots = steady_shot,if= talent.steady_focus & in_flight & buff.steady_focus.remains < 5
                /* if (Talent_Steady_Focus && API.CanCast(Steady_Shot) && API.LastSpellCastInGame != Steady_Shot && API.PlayerCurrentCastSpellID != 56641 && !PlayerHasBuff(Steady_Focus) && InRange)
                 {
                     API.CastSpell(Steady_Shot);
                     return;
                 }*/
                if (Talent_Steady_Focus && API.CanCast(Steady_Shot) && API.LastSpellCastInGame != Steady_Shot && API.PlayerCurrentCastSpellID == 56641 && API.PlayerBuffTimeRemaining(Steady_Focus) < 500 && InRange)
                {
                    API.CastSpell(Steady_Shot);
                    return;
                }
                //actions.trickshots +=/ double_tap,if= covenant.kyrian & cooldown.resonating_arrow.remains < gcd | !covenant.kyrian & !covenant.night_fae | covenant.night_fae & (cooldown.wild_spirits.remains < gcd | cooldown.trueshot.remains > 55) | target.time_to_die < 10
                if (API.CanCast(Double_Tap) && (UseDoubleTap == "always" || (UseDoubleTap == "with Cooldowns" && IsCooldowns)) && InRange && Talent_Double_Tap && (API.SpellCDDuration(Aimed_Shot) < API.SpellCDDuration(Rapid_Fire))
&& (PlayerCovenantSettings == "Kyrian" && API.SpellCDDuration(Resonating_Arrow) < gcd || PlayerCovenantSettings != "Kyrian" && PlayerCovenantSettings != "Night Fae" || PlayerCovenantSettings == "Night Fae" && (API.SpellCDDuration(Wild_Spirits) < gcd || API.SpellCDDuration(Trueshot) > 5500) || API.TargetTimeToDie < 1000))
                {
                    API.CastSpell(Double_Tap);
                    return;
                }
                //actions.trickshots +=/ tar_trap,if= runeforge.soulforge_embers & tar_trap.remains < gcd & cooldown.flare.remains < gcd
                //actions.trickshots +=/ flare,if= tar_trap.up & runeforge.soulforge_embers
                //actions.trickshots +=/ explosive_shot
                if (API.CanCast(Explosive_Shot) && API.PlayerFocus >= 20 && InRange && Talent_Explosive_Shot)
                {
                    API.CastSpell(Explosive_Shot);
                    return;
                }
                //actions.trickshots +=/ wild_spirits
                if (API.CanCast(Wild_Spirits) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Wild_Spirits);
                    return;
                }
                //actions.trickshots +=/ resonating_arrow
                if (API.CanCast(Resonating_Arrow) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Resonating_Arrow);
                    return;
                }
                //actions.trickshots +=/ volley
                if (API.CanCast(Volley) && (UseVolley == "always" || (UseVolley == "with Cooldowns" && IsCooldowns) || UseVolley == "On AOE") && InRange && Talent_Volley)
                {
                    API.CastSpell(Volley);
                    return;
                }
                //actions.trickshots +=/ barrage
                if (Talent_Barrage && API.CanCast(Barrage) && InRange && API.PlayerFocus >= 30)
                {
                    API.CastSpell(Barrage);
                    return;
                }
                //actions.trickshots +=/ trueshot
                if (API.CanCast(Trueshot) && NoCovReady && (UseTrueshot == "always" || (UseTrueshot == "with Cooldowns" && IsCooldowns)) && InRange)
                {
                    API.CastSpell(Trueshot);
                    return;
                }
                //actions.trickshots +=/ rapid_fire,if= buff.trick_shots.remains >= execute_time & runeforge.surging_shots & buff.double_tap.down
                if (API.CanCast(Rapid_Fire) && !LastSpell(Aimed_Shot, 19434) && !LastSpell(Rapid_Fire, 257044) && InRange && API.PlayerBuffTimeRemaining(Trick_Shots) >= RapidFireChannelTime && !PlayerHasBuff(Double_Tap) && SurgingShots_enabled)
                {
                    API.CastSpell(Rapid_Fire);
                    return;
                }
                //actions.trickshots +=/ aimed_shot,target_if = min:dot.serpent_sting.remains + action.serpent_sting.in_flight_to_target * 99,if= buff.trick_shots.remains >= execute_time & (buff.precise_shots.down | full_recharge_time < cast_time + gcd | buff.trueshot.up)
                if (API.CanCast(Aimed_Shot) && InRange && (API.PlayerHasBuff(Lock_and_Load) || !API.PlayerIsMoving) && API.PlayerFocus >= (API.PlayerHasBuff(Lock_and_Load) ? 0 : 35) && !LastSpell(Aimed_Shot, 19434) && !LastSpell(Rapid_Fire, 257044) &&
    (API.TargetDebuffRemainingTime(Serpent_Sting) > 200 || !Talent_Serpent_Sting) &&
    API.PlayerBuffTimeRemaining(Trick_Shots) >= AimedShotCastTime && (!PlayerHasBuff(Precise_Shots) || FullRechargeTime(Aimed_Shot, AimedShotCooldown) < AimedShotCastTime + gcd || PlayerHasBuff(Trueshot)))
                {
                    API.CastSpell(Aimed_Shot);
                    return;
                }
                //actions.trickshots +=/ death_chakram,if= focus + cast_regen < focus.max
                if (API.CanCast(Death_Chakram) && API.PlayerFocus + FocusRegen * (gcd / 100) < API.PlayerMaxFocus && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Death_Chakram);
                    return;
                }
                //actions.trickshots +=/ rapid_fire,if= buff.trick_shots.remains >= execute_time
                if (API.CanCast(Rapid_Fire) && !LastSpell(Aimed_Shot, 19434) && !LastSpell(Rapid_Fire, 257044) && InRange && API.PlayerBuffTimeRemaining(Trick_Shots) >= RapidFireChannelTime)
                {
                    API.CastSpell(Rapid_Fire);
                    return;
                }
                //actions.trickshots +=/ multishot,if= buff.trick_shots.down | buff.precise_shots.up & focus > cost + action.aimed_shot.cost & (!talent.chimaera_shot | active_enemies > 3)
                if (API.CanCast(Multi_Shot) && (!PlayerHasBuff(Trick_Shots) || !API.CanCast(Rapid_Fire)) && InRange && API.PlayerFocus >= 20 && (!API.PlayerHasBuff(Trick_Shots) || PlayerHasBuff(Precise_Shots) && API.PlayerFocus > 20 + (PlayerHasBuff(Lock_and_Load) ? 0 : 35)))
                {
                    API.CastSpell(Multi_Shot);
                    return;
                }
                //actions.trickshots +=/ chimaera_shot,if= buff.precise_shots.up & focus > cost + action.aimed_shot.cost & active_enemies < 4
                if (Talent_Chimaera_Shot && API.CanCast(Chimaera_Shot) && InRange && (API.PlayerHasBuff(Precise_Shots) && API.PlayerFocus > 10 + (PlayerHasBuff(Lock_and_Load) ? 0 : 35) && API.TargetUnitInRangeCount < 4))
                {
                    API.CastSpell(Chimaera_Shot);
                    return;
                }
                //actions.trickshots +=/ kill_shot,if= buff.dead_eye.down
                if (API.CanCast(Kill_Shot) && InRange && PlayerLevel >= 42 && !PlayerHasBuff(Dead_Eye))
                {
                    API.CastSpell(Kill_Shot);
                    return;
                }
                if (API.CanCast(Kill_Shot) && !PlayerHasBuff(Dead_Eye) && (IsMouseover && (!isMouseoverInCombat || API.MouseoverIsIncombat) && API.PlayerCanAttackMouseover && (API.MouseoverHealthPercent <= 20 || PlayerHasBuff(FlayersMark)) && API.PlayerFocus >= 10 && PlayerLevel >= 42))
                {
                    API.CastSpell(Kill_Shot + "MO");
                    return;
                }
                //actions.trickshots +=/ a_murder_of_crows
                if (Talent_A_Murder_of_Crows && (UseAMurderofCrows == "always" || (UseAMurderofCrows == "with Cooldowns" && IsCooldowns)) && API.CanCast(A_Murder_of_Crows) && InRange && API.PlayerFocus >= 20)
                {
                    API.CastSpell(A_Murder_of_Crows);
                    return;
                }
                //actions.trickshots +=/ flayed_shot
                if (API.CanCast(Flayed_Shot) && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && InRange)
                {
                    API.CastSpell(Flayed_Shot);
                    return;
                }
                //actions.trickshots +=/ serpent_sting,target_if = min:dot.serpent_sting.remains,if= refreshable
                if (Talent_Serpent_Sting && API.CanCast(Serpent_Sting) && API.PlayerFocus > 10 && InRange && (!API.TargetHasDebuff(Serpent_Sting) || API.TargetDebuffRemainingTime(Serpent_Sting) < 200) && API.TargetTimeToDie > 1800)
                {
                    API.CastSpell(Serpent_Sting);
                    return;
                }
                //actions.trickshots +=/ multishot,if= focus > cost + action.aimed_shot.cost
                if (API.CanCast(Multi_Shot) && InRange && API.PlayerFocus >= 20 && API.PlayerFocus > 20 + (PlayerHasBuff(Lock_and_Load) ? 0 : 35))
                {
                    API.CastSpell(Multi_Shot);
                    return;
                }
                //actions.trickshots +=/ steady_shot
                if (API.CanCast(Steady_Shot) && API.PlayerFocus <= 20 + (PlayerHasBuff(Lock_and_Load) ? 0 : 35) && InRange)
                {
                    API.CastSpell(Steady_Shot);
                    return;
                }
            }
        }
    }
}


