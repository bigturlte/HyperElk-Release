﻿
namespace HyperElk.Core
{
    public class RetributionPaladin : CombatRoutine
    {


        //Spells,Buffs,Debuffs
        private string CrusaderStrike = "Crusader Strike";
        private string Judgment = "Judgment";
        private string WakeofAshes = "Wake of Ashes";
        private string BladeofJustice = "Blade of Justice";
        private string Consecration = "Consecration";
        private string TemplarsVerdict = "Templar's Verdict";
        private string AvengingWrath = "Avenging Wrath";
        private string HammerofWrath = "Hammer of Wrath";
        private string DivineStorm = "Divine Storm";

        private string FinalReckoning = "Final Reckoning";
        private string Seraphim = "Seraphim";
        private string ExecutionSentence = "Execution Sentence";
        private string Crusade = "Crusade";
        private string HolyAvenger = "Holy Avenger";
        private string Rebuke = "Rebuke";

        private string WordOfGlory = "Word of Glory";
        private string FlashofLight = "Flash of Light";
        private string LayOnHands = "Lay on Hands";

        private string DivineShield = "Divine Shield";
        private string ShieldofVengeance = "Shield of Vengeance";

        private string CrusaderAura = "Crusader Aura";
        private string DevotionAura = "Devotion Aura";

        private string DivinePurpose = "Divine Purpose";
        private string EmpyreanPower = "Empyrean Power";
        private string Forbearance = "Forbearance";
        private string SelflessHealer = "Selfless Healer";
        private string VanquishersHammer = "Vanquisher's Hammer";
        private string DivineToll = "Divine Toll";
        private string AshenHallow = "Ashen Hallow";
        private string BlessingoftheSeasons = "328282";
        private string RingingClarity = "Ringing Clarity";
        private string Mindgames = "Mindgames";
        private string BlessingofProtection = "Blessing of Protection";


        private bool IsMouseover => API.ToggleIsEnabled("MO Heal");
        private bool UseSmallCD => API.ToggleIsEnabled("Small CDs");
        private bool HealFocus => API.ToggleIsEnabled("Focus Heal");
        //Misc
        private static bool PlayerHasBuff(string buff)
        {
            return API.PlayerHasBuff(buff, false, false);
        }
        private static bool TargetHasDebuff(string debuff)
        {
            return API.TargetHasDebuff(debuff, true, false);
        }
        private static bool Buff_or_CDmorethan(string spellname, int time)
        {
            return PlayerHasBuff(spellname) || API.SpellCDDuration(spellname) > time;
        }

        private int PlayerLevel => API.PlayerLevel;
        private int holy_power => API.PlayerCurrentHolyPower;
        private bool IsMelee => API.TargetRange < 6;
        private int time => API.PlayerTimeInCombat;
        private float gcd => API.SpellGCDTotalDuration;
        private bool HasDefenseBuff => API.PlayerHasBuff(ShieldofVengeance, false, false) || API.PlayerHasBuff(DivineShield, false, false);
        private float CrusaderStrikeCooldown => (6 / (1 + API.PlayerGetHaste / 100)) * 100;
        private float Crusader_Strike_Fractional => (API.SpellCharges(CrusaderStrike) * 100 + ((CrusaderStrikeCooldown - API.SpellChargeCD(CrusaderStrike)) / (CrusaderStrikeCooldown / 100)));
        private bool gcd_to_hpg => API.SpellCDDuration(CrusaderStrike) >= gcd && API.SpellCDDuration(BladeofJustice) >= gcd && API.SpellCDDuration(Judgment) >= gcd && (API.SpellCDDuration(HammerofWrath) >= gcd || API.TargetHealthPercent > 20) && API.SpellCDDuration(WakeofAshes) >= gcd;
        private bool Conduit_enabled (string conduit)
        {
            return API.PlayerIsConduitSelected(conduit);
        }

        //finishers->add_action("variable,name=ds_castable,value=spell_targets.divine_storm>=2|buff.empyrean_power.up&debuff.judgment.down&buff.divine_purpose.down|spell_targets.divine_storm>=2&buff.crusade.up&buff.crusade.stack<10");
        private bool ds_castable => API.TargetUnitInRangeCount >= 2 || PlayerHasBuff(EmpyreanPower) && !API.TargetHasDebuff(Judgment) && !PlayerHasBuff(DivinePurpose) || API.TargetUnitInRangeCount >= 2 && PlayerHasBuff(Crusade) && API.PlayerBuffStacks(Crusade) < 10;


        //Talents
        private bool Talent_ExecutionSentence => API.PlayerIsTalentSelected(1, 3);
        private bool Talent_HolyAvenger => API.PlayerIsTalentSelected(5, 2);
        private bool Talent_Seraphim => API.PlayerIsTalentSelected(5, 3);
        private bool Talent_Crusade => API.PlayerIsTalentSelected(7, 2);
        private bool Talent_FinalReckoning => API.PlayerIsTalentSelected(7, 3);

        //CBProperties
        private bool Playeriscasting => API.PlayerCurrentCastTimeRemaining > 40;
        string[] AlwaysCooldownsList = new string[] { "always", "with Cooldowns", "on AOE" };

        private int FlashofLightLifePercent => percentListProp[CombatRoutine.GetPropertyInt("FOLOOCPCT")];
        private bool FLashofLightOutofCombat => CombatRoutine.GetPropertyBool("FOLOOC");
        private bool AutoAuraSwitch => CombatRoutine.GetPropertyBool("AURASWITCH");

        private int LayOnHandsLifePercent => percentListProp[CombatRoutine.GetPropertyInt(LayOnHands)];
        private int BlessingofProtectionPercent => percentListProp[CombatRoutine.GetPropertyInt(BlessingofProtection)];
        private int ShieldofVengeanceLifePercent => percentListProp[CombatRoutine.GetPropertyInt(ShieldofVengeance)];
        private int DivineShieldLifePercent => percentListProp[CombatRoutine.GetPropertyInt(DivineShield)];
        private int WordOfGloryLifePercent => percentListProp[CombatRoutine.GetPropertyInt(WordOfGlory)];
        private int FlashofLightLifePercentProc => percentListProp[CombatRoutine.GetPropertyInt(FlashofLight)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseCovenant")];
        private string UseSeraphim => CDUsage[CombatRoutine.GetPropertyInt("UseSeraphim")];
        private string UseWakeofAshes => CDUsageWithAOE[CombatRoutine.GetPropertyInt("UseWakeofAshes")];
        private string UseTrinket1 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket1")];
        private string UseTrinket2 => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Trinket2")];


        public override void Initialize()
        {
            CombatRoutine.Name = "Retribution Paladin by Vec ";
            API.WriteLog("Welcome to Paladin Retribution Rotation");
            API.WriteLog("Macro required: /cast [@player] Final Reckoning");


            //Spells
            CombatRoutine.AddSpell(CrusaderStrike, "D1");
            CombatRoutine.AddSpell(Judgment, "D2");
            CombatRoutine.AddSpell(WakeofAshes, "D2");
            CombatRoutine.AddSpell(BladeofJustice, "D1");
            CombatRoutine.AddSpell(Consecration, "D4");
            CombatRoutine.AddSpell(TemplarsVerdict, "D3");
            CombatRoutine.AddSpell(DivineStorm, "D3");
            CombatRoutine.AddSpell(FinalReckoning, "D3");
            CombatRoutine.AddSpell(Seraphim, "D3");
            CombatRoutine.AddSpell(ExecutionSentence, "D3");
            CombatRoutine.AddSpell(Crusade, "D3");
            CombatRoutine.AddSpell(HolyAvenger, "D3");

            CombatRoutine.AddSpell(Rebuke, "F");

            CombatRoutine.AddSpell(CrusaderAura, "F5");
            CombatRoutine.AddSpell(DevotionAura, "F6");

            CombatRoutine.AddSpell(AvengingWrath, "F");
            CombatRoutine.AddSpell(HammerofWrath, "D7");

            CombatRoutine.AddSpell(LayOnHands, "F8");            
            CombatRoutine.AddSpell(ShieldofVengeance, "S");
            CombatRoutine.AddSpell(DivineShield, "F10");
            CombatRoutine.AddSpell(FlashofLight, "Q");
            CombatRoutine.AddSpell(WordOfGlory, "F7");
            CombatRoutine.AddSpell(DivineToll, "F8");
            CombatRoutine.AddSpell(AshenHallow, "F8");
            CombatRoutine.AddSpell(BlessingoftheSeasons, "F8");
            CombatRoutine.AddSpell(VanquishersHammer, "F8");
            CombatRoutine.AddSpell(BlessingofProtection, "F11");
            //Buffs
            CombatRoutine.AddBuff(Consecration);
            CombatRoutine.AddBuff(CrusaderAura);
            CombatRoutine.AddBuff(DevotionAura);
            CombatRoutine.AddBuff(AvengingWrath);
            CombatRoutine.AddBuff(Crusade);
            CombatRoutine.AddBuff(EmpyreanPower);
            CombatRoutine.AddBuff(DivinePurpose);
            CombatRoutine.AddBuff(ShieldofVengeance);
            CombatRoutine.AddBuff(DivineShield);
            CombatRoutine.AddBuff(SelflessHealer);
            CombatRoutine.AddBuff(VanquishersHammer);
            CombatRoutine.AddBuff(HolyAvenger);
            CombatRoutine.AddBuff(Seraphim);

            //Debuffs
            CombatRoutine.AddDebuff(Forbearance);
            CombatRoutine.AddDebuff(Judgment);
            CombatRoutine.AddDebuff(ExecutionSentence);
            CombatRoutine.AddDebuff(FinalReckoning);
            CombatRoutine.AddDebuff(Mindgames);

            CombatRoutine.AddMacro("Trinket1", "F9");
            CombatRoutine.AddMacro("Trinket2", "F10");
            CombatRoutine.AddMacro(LayOnHands + "MO", "F10");
            CombatRoutine.AddMacro(LayOnHands + "focus", "F10");
            CombatRoutine.AddMacro(WordOfGlory + "MO", "F10");
            CombatRoutine.AddMacro(WordOfGlory + "focus", "F10");
            CombatRoutine.AddMacro(BlessingofProtection+ "MO", "F11");
            CombatRoutine.AddMacro(BlessingofProtection + "focus", "F11");
            CombatRoutine.AddMacro(FlashofLight + "focus","F12");
            CombatRoutine.AddMacro(FlashofLight + "MO", "F12");

            CombatRoutine.AddConduit(RingingClarity);

            CombatRoutine.AddToggle("Small CDs");
            CombatRoutine.AddToggle("MO Heal");
            CombatRoutine.AddToggle("Focus Heal");
            //CBProperties
            CombatRoutine.AddProp("FOLOOCPCT", "Out of combat Life Percent", percentListProp, "Life percent at which Flash of Light is used out of combat to heal you between pulls", FlashofLight, 7);
            CombatRoutine.AddProp("FOLOOC", "Out of Combat Healing", true, "Should the bot use Flash of Light out of combat to heal you between pulls", FlashofLight);
            CombatRoutine.AddProp("RingingClarity", "Ringing Clarity", false, "Do you have Ringing Clarity?", "Conduits");
            CombatRoutine.AddProp(FlashofLight, "Selfless Healer Life Percent", percentListProp, "Life percent at which " + FlashofLight + " is used with selfless healer procs, set to 0 to disable", FlashofLight, 5);

            CombatRoutine.AddProp("UseCovenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + " always, with Cooldowns", "Covenant", 0);
            CombatRoutine.AddProp("UseSeraphim", "Use " + Seraphim, CDUsage, "Use " + Seraphim + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("UseWakeofAshes", "Use " + "Wake of Ashes", CDUsageWithAOE, "Use " + WakeofAshes + " always, with Cooldowns", "Cooldowns", 0);
            CombatRoutine.AddProp("Trinket1", "Use " + "Use Trinket 1", CDUsageWithAOE, "Use " + "Trinket 1" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("Trinket2", "Use " + "Trinket 2", CDUsageWithAOE, "Use " + "Trinket 2" + " always, with Cooldowns", "Trinkets", 0);
            CombatRoutine.AddProp("AURASWITCH", "Auto Aura Switch", true, "Auto Switch Aura between Crusader Aura and Devotion Aura", "Generic");

            CombatRoutine.AddProp(LayOnHands, LayOnHands + " Life Percent", percentListProp, "Life percent at which" + LayOnHands + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(BlessingofProtection, BlessingofProtection + " Life Percent", percentListProp, "Life percent at which" + BlessingofProtection + "is used, set to 0 to disable", "Defense", 2);
            CombatRoutine.AddProp(ShieldofVengeance, ShieldofVengeance + " Life Percent", percentListProp, "Life percent at which" + ShieldofVengeance + "is used, set to 0 to disable", "Defense", 6);
            CombatRoutine.AddProp(DivineShield, DivineShield + " Life Percent", percentListProp, "Life percent at which" + DivineShield + "is used, set to 0 to disable", "Defense", 3);
            CombatRoutine.AddProp(WordOfGlory, WordOfGlory + " Life Percent", percentListProp, "Life percent at which Word of Glory is used, set to 0 to disable", "Defense", 4);




        }

        public override void Pulse()
        {
            if (API.PlayerIsMounted)
            {
                if (AutoAuraSwitch && !API.SpellISOnCooldown(CrusaderAura) && PlayerLevel >= 21 && !PlayerHasBuff(CrusaderAura))
                {
                    API.CastSpell(CrusaderAura);
                    return;
                }
            }
            else
            {
                if (AutoAuraSwitch && !API.SpellISOnCooldown(DevotionAura) && PlayerLevel >= 21 && !PlayerHasBuff(DevotionAura))
                {
                    API.CastSpell(DevotionAura);
                    return;
                }
                if (API.PlayerHealthPercent <= FlashofLightLifePercentProc && !API.PlayerHasDebuff(Mindgames) && !API.SpellISOnCooldown(FlashofLight) && API.PlayerBuffStacks(SelflessHealer) >= 4 && PlayerLevel >= 4)
                {
                    API.CastSpell(FlashofLight);
                    return;
                }
            }

        }
        public override void CombatPulse()
        {
            if (isInterrupt && !API.SpellISOnCooldown(Rebuke) && IsMelee && PlayerLevel >= 27)
            {
                API.CastSpell(Rebuke);
                return;
            }
            #region healing

            if (API.PlayerHealthPercent <= LayOnHandsLifePercent && !API.SpellISOnCooldown(LayOnHands) && PlayerLevel >= 9 && !API.PlayerHasDebuff(Forbearance, false, false))
            {
                API.CastSpell(LayOnHands);
                return;
            }
            if (API.PlayerHealthPercent <= DivineShieldLifePercent && !API.SpellISOnCooldown(DivineShield) && PlayerLevel >= 10 && !HasDefenseBuff && !API.PlayerHasDebuff(Forbearance, false, false))
            {
                API.CastSpell(DivineShield);
                return;
            }
            if (API.PlayerHealthPercent <= ShieldofVengeanceLifePercent && !API.SpellISOnCooldown(ShieldofVengeance) && PlayerLevel >= 26 && !HasDefenseBuff)
            {
                API.CastSpell(ShieldofVengeance);
                return;
            }
            if (API.SpellIsCanbeCast(WordOfGlory) && !API.PlayerHasDebuff(Mindgames) && API.PlayerHealthPercent <= WordOfGloryLifePercent && !API.SpellISOnCooldown(WordOfGlory) && PlayerLevel >= 7)
            {
                API.CastSpell(WordOfGlory);
                return;
            }
            if (HealFocus)
            {
                if (!API.MacroIsIgnored(FlashofLight + "focus") && API.FocusHealthPercent <= FlashofLightLifePercentProc && !API.FocusHasDebuff(Mindgames) && !API.SpellISOnCooldown(FlashofLight) && API.FocusRange <= 40 && API.PlayerBuffStacks(SelflessHealer) >= 4 && PlayerLevel >= 4)
                {
                    API.CastSpell(FlashofLight + "focus");
                    return;
                }
                if (!API.MacroIsIgnored(LayOnHands + "focus") && API.FocusHealthPercent <= LayOnHandsLifePercent && API.FocusRange <= 40 && !API.SpellISOnCooldown(LayOnHands) && PlayerLevel >= 9 && !API.FocusHasDebuff(Forbearance, false, false))
                {
                    API.CastSpell(LayOnHands + "focus");
                    return;
                }
                if (!API.MacroIsIgnored(WordOfGlory + "focus") && API.SpellIsCanbeCast(WordOfGlory) && API.FocusRange <= 40 && !API.FocusHasDebuff(Mindgames) && API.FocusHealthPercent <= WordOfGloryLifePercent && !API.SpellISOnCooldown(WordOfGlory) && PlayerLevel >= 7)
                {
                    API.CastSpell(WordOfGlory + "focus");
                    return;
                }
                if (!API.MacroIsIgnored(BlessingofProtection + "focus") && API.FocusRange <= 40 && !API.FocusHasDebuff(Mindgames) && API.FocusHealthPercent <= BlessingofProtectionPercent && !API.SpellISOnCooldown(BlessingofProtection) && PlayerLevel >= 10)
                {
                    API.CastSpell(BlessingofProtection + "focus");
                    return;
                }
            }
            if (IsMouseover)
            {
                if (!API.MacroIsIgnored(FlashofLight + "MO") && API.MouseoverHealthPercent <= FlashofLightLifePercentProc && !API.MouseoverHasDebuff(Mindgames) && !API.SpellISOnCooldown(FlashofLight) && API.PlayerBuffStacks(SelflessHealer) >= 4 && PlayerLevel >= 4)
                {
                    API.CastSpell(FlashofLight + "MO");
                    return;
                }
                if (!API.MacroIsIgnored(LayOnHands + "MO") && API.MouseoverHealthPercent <= LayOnHandsLifePercent && API.MouseoverRange <= 40 && !API.SpellISOnCooldown(LayOnHands) && PlayerLevel >= 9 && !API.MouseoverHasDebuff(Forbearance, false, false))
                {
                    API.CastSpell(LayOnHands + "MO");
                    return;
                }
                if (!API.MacroIsIgnored(WordOfGlory + "MO") && API.SpellIsCanbeCast(WordOfGlory) && API.MouseoverRange <= 40 && !API.MouseoverHasDebuff(Mindgames) && API.MouseoverHealthPercent <= WordOfGloryLifePercent && !API.SpellISOnCooldown(WordOfGlory) && PlayerLevel >= 7)
                {
                    API.CastSpell(WordOfGlory + "MO");
                    return;
                }
                if (!API.MacroIsIgnored(BlessingofProtection + "MO") && API.SpellIsCanbeCast(BlessingofProtection) && API.MouseoverRange <= 40 && API.MouseoverHealthPercent <= BlessingofProtectionPercent && !API.SpellISOnCooldown(BlessingofProtection) && PlayerLevel >= 10)
                {
                    API.CastSpell(BlessingofProtection + "MO");
                    return;
                }
            }
            #endregion
            rotation();
            return;
        }

        public override void OutOfCombatPulse()
        {
            if (FLashofLightOutofCombat && API.PlayerHealthPercent <= FlashofLightLifePercent && !API.PlayerIsMoving && !API.SpellISOnCooldown(FlashofLight) && PlayerLevel >= 4)
            {
                API.CastSpell(FlashofLight);
                return;
            }
        }
        private void rotation()
        {
            
            if (isRacial)
            {
                //actions.cooldowns +=/ lights_judgment,if= spell_targets.lights_judgment >= 2 )
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Lightforged Draenei" && IsMelee && API.PlayerUnitInMeleeRangeCount >=2)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
                //actions.cooldowns +=/ fireblood,if= buff.avenging_wrath.up | buff.crusade.up & buff.crusade.stack = 10
                if (API.CanCast(RacialSpell1) && PlayerRaceSettings == "Dark Iron Dwarf" && (PlayerHasBuff(AvengingWrath) || PlayerHasBuff(Crusade) && API.PlayerBuffStacks(Crusade) == 10)&& IsMelee)
                {
                    API.CastSpell(RacialSpell1);
                    return;
                }
            }
            //API.WriteLog("conduit "+ API.CanCast(HammerofWrath));
            if (IsCooldowns)
            {
                //cds->add_action(this, "Avenging Wrath", "if=(holy_power>=4&time<5|holy_power>=3&time>5|talent.holy_avenger.enabled&cooldown.holy_avenger.remains=0)&time_to_hpg=0");
                if (!API.SpellISOnCooldown(AvengingWrath) && !PlayerHasBuff(AvengingWrath) && !Talent_Crusade && IsMelee && (holy_power >= 4 && time < 500 || holy_power >= 3 && time > 500) && PlayerLevel >= 37)
                {
                    API.CastSpell(AvengingWrath);
                    return;
                }
                //cds->add_talent(this, "Crusade", "if=(holy_power>=4&time<5|holy_power>=3&time>5|talent.holy_avenger.enabled&cooldown.holy_avenger.remains=0)&time_to_hpg=0");
                if (!API.SpellISOnCooldown(Crusade) && !PlayerHasBuff(Crusade) && IsMelee && Talent_Crusade && (holy_power >= 4 && API.PlayerTimeInCombat < 500 || holy_power >= 3 && API.PlayerTimeInCombat > 500))
                {
                    API.CastSpell(Crusade);
                    return;
                }
                //cds->add_talent(this, "Holy Avenger", "if=time_to_hpg=0&((buff.avenging_wrath.up|buff.crusade.up)|(buff.avenging_wrath.down&cooldown.avenging_wrath.remains>40|buff.crusade.down&cooldown.crusade.remains>40))");
                if (!API.SpellISOnCooldown(HolyAvenger) && IsMelee && Talent_HolyAvenger && ((PlayerHasBuff(AvengingWrath) || PlayerHasBuff(Crusade)) || API.SpellCDDuration(AvengingWrath) > API.TargetTimeToDie || API.TargetTimeToDie < API.SpellCDDuration(Crusade)))
                {
                    API.CastSpell(HolyAvenger);
                    return;
                }              
            }
            //Final Reckoning with at least 3HP, and if if Avenging Wrath / Crusade are active OR remain on cooldown for greater than 10 seconds.
            if (Talent_FinalReckoning && (IsCooldowns || UseSmallCD) && !gcd_to_hpg && IsMelee && !API.SpellISOnCooldown(FinalReckoning) && API.TargetRange < 30 && API.PlayerCurrentHolyPower >= 3 && (!IsCooldowns || Buff_or_CDmorethan(AvengingWrath, 1000) || Buff_or_CDmorethan(Crusade, 1000)) && (PlayerHasBuff(Seraphim) || !Talent_Seraphim))
            {
                API.CastSpell(FinalReckoning);
                return;
            }
            //generators->add_action("divine_toll,if=!debuff.judgment.up&(!raid_event.adds.exists|raid_event.adds.in>30)&(holy_power<=2|holy_power<=4&(cooldown.blade_of_justice.remains>gcd*2|debuff.execution_sentence.up|debuff.final_reckoning.up))&(!talent.final_reckoning.enabled|cooldown.final_reckoning.remains>gcd*10)&(!talent.execution_sentence.enabled|cooldown.execution_sentence.remains>gcd*10)");
            if (API.CanCast(DivineToll) && (holy_power <= 4 - (Conduit_enabled(RingingClarity)? 2:0))  && (!Conduit_enabled(RingingClarity) || !IsAOE || API.TargetUnitInRangeCount < AOEUnitNumber && IsAOE || ( Conduit_enabled(RingingClarity) || API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE)&& holy_power <= 2) && !TargetHasDebuff(Judgment) && (!Talent_ExecutionSentence || TargetHasDebuff(ExecutionSentence)) && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && (IsCooldowns || UseSmallCD) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && API.TargetRange <= 30)
            {
                API.CastSpell(DivineToll);
                return;
            }
            //cds->add_action("ashen_hallow");
            if (!API.SpellISOnCooldown(AshenHallow) && !API.PlayerIsMoving && API.TargetRange <= 30 && PlayerCovenantSettings == "Venthyr" && (UseCovenant == "With Cooldowns" && (IsCooldowns || UseSmallCD) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
            {
                API.CastSpell(AshenHallow);
                return;
            }
            if (!API.SpellISOnCooldown(VanquishersHammer) && API.TargetRange <= 30 && PlayerCovenantSettings == "Necrolord" && (UseCovenant == "With Cooldowns" && (IsCooldowns || UseSmallCD) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
            {
                API.CastSpell(VanquishersHammer);
                return;
            }
            if (!API.SpellISOnCooldown(BlessingoftheSeasons) && PlayerCovenantSettings == "Night Fae" && (UseCovenant == "With Cooldowns" && (IsCooldowns || UseSmallCD) || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
            {
                API.CastSpell(BlessingoftheSeasons);
                return;
            }
            if (API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0 && (UseTrinket1 == "With Cooldowns" && IsCooldowns || UseTrinket1 == "On Cooldown" || UseTrinket1 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee)
            {
                API.CastSpell("Trinket1");
                return;
            }
            if (API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0 && (UseTrinket2 == "With Cooldowns" && IsCooldowns || UseTrinket2 == "On Cooldown" || UseTrinket2 == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE) && IsMelee)
            {
                API.CastSpell("Trinket2");
                return;
            }
            //Seraphim if Avenging Wrath / Crusade are active OR remain on cooldown for greater than 25 seconds.
            if (Talent_Seraphim && API.CanCast(Seraphim) && UseSeraphim != "Not Used" && (UseSeraphim == "On Cooldown" || UseSeraphim == "With Cooldowns" && (IsCooldowns|| UseSmallCD)) && IsMelee && (!IsCooldowns || Buff_or_CDmorethan(AvengingWrath, 2500) || Buff_or_CDmorethan(Crusade, 2500)))
            {
                API.CastSpell(Seraphim);
                return;
            }
            //Execution Sentence if Avenging Wrath / Crusade are active OR remain on cooldown for greater than 10 seconds.
            if (API.PlayerIsTalentSelected(1, 3) && (!Talent_FinalReckoning ||TargetHasDebuff(FinalReckoning) ) && (IsCooldowns || UseSmallCD) && (holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && !API.SpellISOnCooldown(ExecutionSentence) && API.TargetRange < 30 && (!IsCooldowns || Buff_or_CDmorethan(AvengingWrath, 1000) || Buff_or_CDmorethan(Crusade, 1000)))
            {
                API.CastSpell(ExecutionSentence);
                return;
            }
            //Templar's Verdict with 5HP.
            if (!API.SpellISOnCooldown(TemplarsVerdict) && (holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && holy_power >= 5 && IsMelee && PlayerLevel >= 10)
            {
                if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.PlayerLevel >= 23)
                {
                    API.CastSpell(DivineStorm);
                    return;
                }
                else
                {
                    API.CastSpell(TemplarsVerdict);
                    return;
                }
            }
            //Wake of Ashes at 0HP OR at 2HP or less if Blade of Justice remains on CD for greater than 2 GCDs. saved for Execution Sentence and/or Final Reckoning.
            if (!API.SpellISOnCooldown(WakeofAshes) && (API.PlayerCurrentHolyPower == 0 || (API.PlayerCurrentHolyPower <= 2 && API.SpellCDDuration(BladeofJustice) > 2 * gcd)) && (!IsCooldowns || (!Talent_FinalReckoning || TargetHasDebuff(FinalReckoning)) && (TargetHasDebuff(ExecutionSentence) || !Talent_ExecutionSentence)) && IsMelee && PlayerLevel >= 39 && (UseWakeofAshes == "With Cooldowns" && (IsCooldowns || UseSmallCD) || UseWakeofAshes == "On Cooldown" || UseWakeofAshes == "on AOE" && API.TargetUnitInRangeCount >= AOEUnitNumber && IsAOE))
            {
                API.CastSpell(WakeofAshes);
                return;
            }
            //Blade of Justice at 3HP or less.
            if (!API.SpellISOnCooldown(BladeofJustice) && API.TargetRange <= 12 && API.PlayerCurrentHolyPower <= 3 && PlayerLevel >= 19)
            {
                API.CastSpell(BladeofJustice);
                return;
            }
            //Hammer of Wrath at 4HP or less.
            if (API.CanCast(HammerofWrath) && holy_power <= 4 && API.TargetRange <= 30 && PlayerLevel >= 46)
            {
                API.CastSpell(HammerofWrath);
                return;
            }
            //Judgment at 4HP or less and the Judgment debuff is not up.
            if (API.CanCast(Judgment) && !TargetHasDebuff(Judgment) && holy_power <= 4 && API.TargetRange <= 30 && PlayerLevel >= 3)
            {
                API.CastSpell(Judgment);
                return;
            }
            //Templar's Verdict if Avenging Wrath/Crusade are active, target is below 20% health, or with a Divine Purpose proc. Divine Storm with Empyrean Power proc.
            if (!API.SpellISOnCooldown(TemplarsVerdict) && (holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && (PlayerHasBuff(AvengingWrath) || PlayerHasBuff(Crusade) || API.TargetHealthPercent < 20 || PlayerHasBuff(DivinePurpose)) && IsMelee && PlayerLevel >= 10)
            {
                if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.PlayerLevel >= 23)
                {
                    API.CastSpell(DivineStorm);
                    return;
                }
                else
                {
                    API.CastSpell(TemplarsVerdict);
                    return;
                }
            }
            //Divine Storm with Empyrean Power proc.
            if (!API.SpellISOnCooldown(DivineStorm) && PlayerHasBuff(EmpyreanPower) && IsMelee && PlayerLevel >= 23)
            {
                API.CastSpell(DivineStorm);
                return;
            }
            //Crusader Strike at 2 Charges and at 4HP or less.
            if (!API.SpellISOnCooldown(CrusaderStrike) && API.SpellCharges(CrusaderStrike) == 2 && holy_power <= 4 && IsMelee && holy_power <= 4)
            {
                API.CastSpell(CrusaderStrike);
                return;
            }
            //Templar's Verdict at 4HP or less.
            if (!API.SpellISOnCooldown(TemplarsVerdict) && (holy_power >= 3 || PlayerHasBuff(DivinePurpose)) && holy_power <= 4 && IsMelee && PlayerLevel >= 10)
            {
                if (IsAOE && API.PlayerUnitInMeleeRangeCount >= AOEUnitNumber && API.PlayerLevel >= 23)
                {
                    API.CastSpell(DivineStorm);
                    return;
                }
                else
                {
                    API.CastSpell(TemplarsVerdict);
                    return;
                }
            }
            //Crusader Strike regardless of charges at 4HP or less.
            if (!API.SpellISOnCooldown(CrusaderStrike) && holy_power <= 4 && IsMelee && holy_power <= 4)
            {
                API.CastSpell(CrusaderStrike);
                return;
            }
            //actions.generators +=/ arcane_torrent,if= holy_power <= 4
            if (!API.SpellISOnCooldown(RacialSpell1) && isRacial && PlayerRaceSettings == "Blood Elf" && holy_power <= 4 && IsMelee && holy_power <= 4)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
            //Consecration if all HP builders are on CD for greater than or equal to 1 GCD.
            if (!API.SpellISOnCooldown(Consecration) && IsMelee && gcd_to_hpg)
            {
                API.CastSpell(Consecration);
                return;
            }
            if (!API.SpellISOnCooldown(RacialSpell1) && isRacial && PlayerRaceSettings == "Tauren" && IsMelee && holy_power <= 4)
            {
                API.CastSpell(RacialSpell1);
                return;
            }
        }
    }
}