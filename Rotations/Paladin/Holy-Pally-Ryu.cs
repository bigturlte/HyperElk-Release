﻿using System.Linq;

namespace HyperElk.Core
{

    public class HolyPally : CombatRoutine
    {

        //Spell Strings
        private string DivineShield = "Divine Shield";
        private string DivineProtection = "Divine Protection";
        private string HolyShock = "Holy Shock";
        private string HolyShockHealing = "Holy Shock Healing";
        private string HolyLight = "Holy Light";
        private string FoL = "Flash of Light";
        private string WoG = "Word of Glory";
        private string BoS = "Blessing of Sacrifice";
        private string LoH = "Lay on Hands";
        private string BF = "Bestow Faith";
        private string LoD = "Light of Dawn";
        private string HoLI = "Holy Light /w Infusion of Light";
        private string FoLI = "Flash of Light /w Infusion of Light";
        private string BoV = "Beacon of Virtue";
        private string CrusaderStrike = "Crusader Strike";
        private string Judgment = "Judgment";
        private string AvengingWrath = "Avenging Wrath";
        private string HolyAvenger = "Holy Avenger";
        private string AuraMastery = "Aura Mastery";
        private string HammerofJustice = "Hammer of Justice";
        private string Cons = "Consecration";
        private string Seraphim = "Seraphim";
        private string HammerofWrath = "Hammer of Wrath";
        private string HolyPrism = "Holy Prism";
        private string LightsHammer = "Light's Hammer";
        private string Forbearance = "Forbearance";
        private string Infusion = "Infusion of Light";
        private string AvengingCrusader = "Avenging Crusader";
        private string PartySwap = "Party Swap";
        private string Fleshcraft = "Fleshcraft";
        private string DivineToll = "Divine Toll";
        private string DivineTollHealing = "Divine Toll Healing";
        private string VanqusihersHammer = "Vanquisher's Hammer";
        private string AshenHallow = "Ashen Hallow";
        private string LoTM = "Light of the Martyr";
        private string LoTMH = "Light of the Martyr Health";
        private string LoTMM = "Light of the Martyr Moving";
        private string AoE = "AOE";
        private string Party1 = "party1";
        private string Party2 = "party2";
        private string Party3 = "party3";
        private string Party4 = "party4";
        private string Player = "player";
        private string trinket1 = "trinket1";
        private string trinket2 = "trinket2";

        private string CrusaderAura = "Crusader Aura";
        private string DevotionAura = "Devotion Aura";


        //Talents
        bool CrusadersMight => API.PlayerIsTalentSelected(1, 1);
        bool BestowFaith => API.PlayerIsTalentSelected(1, 2);
        bool LightsHammerT => API.PlayerIsTalentSelected(1, 3);
        bool Savedbythelight => API.PlayerIsTalentSelected(2, 1);
        bool JudgementofLight => API.PlayerIsTalentSelected(2, 2);
        bool HolyPrismT => API.PlayerIsTalentSelected(2, 3);
        bool FistofJustice => API.PlayerIsTalentSelected(3, 1);
        bool Repentance => API.PlayerIsTalentSelected(3, 2);
        bool BlindingLight => API.PlayerIsTalentSelected(3, 3);
        bool UnbreakableSpirit => API.PlayerIsTalentSelected(4, 1);
        bool Calvalier => API.PlayerIsTalentSelected(4, 2);
        bool RuleofLaw => API.PlayerIsTalentSelected(4, 3);
        bool DivinePurpose => API.PlayerIsTalentSelected(5, 1);
        bool HolyAvengerT => API.PlayerIsTalentSelected(5, 2);
        bool SeraphimT => API.PlayerIsTalentSelected(5, 3);
        bool SancifiedWrath => API.PlayerIsTalentSelected(6, 1);
        bool AvengingCrusaderT => API.PlayerIsTalentSelected(6, 2);
        bool Awakening => API.PlayerIsTalentSelected(6, 3);
        bool GlimmerofLight => API.PlayerIsTalentSelected(7, 1);
        bool BeaconofFaith => API.PlayerIsTalentSelected(7, 2);
        bool BeaconofVirtue => API.PlayerIsTalentSelected(7, 3);

        //CBProperties
        int[] numbList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100 };
        int[] numbPartyList = new int[] { 0, 1, 2, 3, 4, 5, };
        int[] numbRaidList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 33, 35, 36, 37, 38, 39, 40 };
        private string[] units = { "player", "party1", "party2", "party3", "party4" };
        private string[] raidunits = { "raid1", "raid2", "raid3", "raid4", "raid5", "raid6", "raid7", "raid8", "raid9", "raid8", "raid9", "raid10", "raid11", "raid12", "raid13", "raid14", "raid16", "raid17", "raid18", "raid19", "raid20", "raid21", "raid22", "raid23", "raid24", "raid25", "raid26", "raid27", "raid28", "raid29", "raid30", "raid31", "raid32", "raid33", "raid34", "raid35", "raid36", "raid37", "raid38", "raid39", "raid40" };


        private int UnitBelowHealthPercentRaid(int HealthPercent) => raidunits.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercentParty(int HealthPercent) => units.Count(p => API.UnitHealthPercent(p) <= HealthPercent && API.UnitHealthPercent(p) > 0);
        private int UnitBelowHealthPercent(int HealthPercent) => API.PlayerIsInRaid ? UnitBelowHealthPercentRaid(HealthPercent) : UnitBelowHealthPercentParty(HealthPercent);

        private bool DPSHealthCheck => UnitBelowHealthPercent(40) >= 2;
        private bool LoDAoE => UnitBelowHealthPercent(LoDLifePercent) >= AoENumber;
        private bool BoVAoE => UnitBelowHealthPercent(BoVLifePercent) >= AoENumber;
        private bool HPAoE => UnitBelowHealthPercent(HPLifePercent) >= AoENumber;
        private bool DTAoE => UnitBelowHealthPercent(DTLifePercent) >= AoENumber;

        private bool LodParty(int i)
        {

            return API.UnitHealthPercent(units[i]) <= LoDLifePercent && units.Count(p => API.UnitHealthPercent(p) <= LoDLifePercent) >= 3;
        }

        private bool LoDParty1 => LodParty(1);
        private bool LoDRaid(int i)
        {

            return API.UnitHealthPercent(units[i]) <= LoDLifePercent && raidunits.Count(p => API.UnitHealthPercent(p) <= LoDLifePercent) >= 3;
        }

        private bool LoDRaid1 => LoDRaid(1);
        private bool BoVParty(int i)
        {

            return API.UnitHealthPercent(units[i]) <= BoVLifePercent && units.Count(p => API.UnitHealthPercent(p) <= BoVLifePercent) >= 3;
        }

        private bool BoVParty1 => BoVParty(1);
        private bool BoVRaid(int i)
        {

            return API.UnitHealthPercent(units[i]) <= BoVLifePercent && raidunits.Count(p => API.UnitHealthPercent(p) <= BoVLifePercent) >= 3;
        }

        private bool BoVRaid1 => BoVRaid(1);
        private bool HPParty(int i)
        {

            return API.UnitHealthPercent(units[i]) <= HPLifePercent && units.Count(p => API.UnitHealthPercent(p) <= HPLifePercent) >= 3;
        }

        private bool HPParty1 => HPParty(1);
        private bool HPRaid(int i)
        {

            return API.UnitHealthPercent(units[i]) <= HPLifePercent && raidunits.Count(p => API.UnitHealthPercent(p) <= HPLifePercent) >= 3;

        }
        private bool HPRaid1 => HPRaid(1);
        private bool DTParty(int i)
        {

            return API.UnitHealthPercent(units[i]) <= DTLifePercent && units.Count(p => API.UnitHealthPercent(p) <= DTLifePercent) >= 3;
        }

        private bool DTParty1 => DTParty(1);
        private bool DTRaid(int i)
        {

            return API.UnitHealthPercent(units[i]) <= DTLifePercent && raidunits.Count(p => API.UnitHealthPercent(p) <= DTLifePercent) >= 3;

        }
        private bool DTRaid1 => DTRaid(1);
        private bool PlayerSwap => API.UnitHealthPercent(Player) <= PartySwapPercent;
        private bool Party1Swap => API.UnitHealthPercent(Party1) <= PartySwapPercent;
        private bool Party2Swap => API.UnitHealthPercent(Party2) <= PartySwapPercent;
        private bool Party3Swap => API.UnitHealthPercent(Party3) <= PartySwapPercent;
        private bool Party4Swap => API.UnitHealthPercent(Party4) <= PartySwapPercent;
        public bool isMouseoverInCombat => CombatRoutine.GetPropertyBool("MouseoverInCombat");
        private int DivineShieldLifePercent => numbList[CombatRoutine.GetPropertyInt(DivineShield)];
        private int DivineProtectionLifePercent => numbList[CombatRoutine.GetPropertyInt(DivineProtection)];
        private int HolyShockLifePercent => numbList[CombatRoutine.GetPropertyInt(HolyShock)];
        private int HolyLightLifePercent => numbList[CombatRoutine.GetPropertyInt(HolyLight)];
        private int FoLLifePercent => numbList[CombatRoutine.GetPropertyInt(FoL)];
        private int WoGLifePercent => numbList[CombatRoutine.GetPropertyInt(WoG)];
        private int BoSLifePercent => numbList[CombatRoutine.GetPropertyInt(BoS)];
        private int FolILifePercent => numbList[CombatRoutine.GetPropertyInt(FoLI)];
        private int HoLILifePercent => numbList[CombatRoutine.GetPropertyInt(HoLI)];
        private int LoHLifePercent => numbList[CombatRoutine.GetPropertyInt(LoH)];
        private int BFLifePercent => numbList[CombatRoutine.GetPropertyInt(BF)];
        private int LoDLifePercent => numbList[CombatRoutine.GetPropertyInt(LoD)];
        private int BoVLifePercent => numbList[CombatRoutine.GetPropertyInt(BoV)];
        private int HPLifePercent => numbList[CombatRoutine.GetPropertyInt(HolyPrism)];
        private int PartySwapPercent => numbList[CombatRoutine.GetPropertyInt(PartySwap)];
        private int DTLifePercent => numbList[CombatRoutine.GetPropertyInt(DivineToll)];
        private int LoTMHealthPercent => numbList[CombatRoutine.GetPropertyInt(LoTMH)];
        private int LoTMLifePercent => numbList[CombatRoutine.GetPropertyInt(LoTM)];
        private int LoTMMovingLifePercent => numbList[CombatRoutine.GetPropertyInt(LoTMM)];
        private int FleshcraftPercentProc => percentListProp[CombatRoutine.GetPropertyInt(Fleshcraft)];
        private int AoENumber => numbPartyList[CombatRoutine.GetPropertyInt(AoE)];
        private string UseCovenant => CDUsageWithAOE[CombatRoutine.GetPropertyInt("Use Covenant")];
        //General
        private int Level => API.PlayerLevel;
        private bool InRange => API.TargetRange <= 40;
        private bool IsMelee => API.TargetRange < 6;

        private bool IsMoMelee = API.MouseoverRange < 6;
       // private bool NotCasting => !API.PlayerIsCasting;
        private bool NotChanneling => !API.PlayerIsChanneling;
        private int Trinket1Usage => CombatRoutine.GetPropertyInt("Trinket1");
        private int Trinket2Usage => CombatRoutine.GetPropertyInt("Trinket2");
        private bool IsMouseover => API.ToggleIsEnabled("Mouseover");
        private bool DTHealing => CombatRoutine.GetPropertyBool(DivineTollHealing);
        private bool HSHealing => CombatRoutine.GetPropertyBool(HolyShockHealing);
        private bool IsOOC => CombatRoutine.GetPropertyBool("OOC");
        private bool AutoAuraSwitch => CombatRoutine.GetPropertyBool("Aura Switch");
        
        //Spell Check Bools
        private bool LoTMCheck => API.CanCast(LoTM)  && InRange && !API.PlayerCanAttackTarget && !API.PlayerIsTargetTarget && (API.PlayerIsMoving && API.TargetHealthPercent <= LoTMMovingLifePercent || API.TargetHealthPercent <= LoTMLifePercent) && API.PlayerHealthPercent >= LoTMHealthPercent && API.TargetHealthPercent != 0;
        private bool LoTMCheckMO => API.CanCast(LoTM) && IsMouseover && InRange && !API.PlayerCanAttackMouseover && !API.PlayerIsTargetTarget && (API.PlayerIsMoving && API.MouseoverHealthPercent <= LoTMMovingLifePercent || API.MouseoverHealthPercent <= LoTMLifePercent) && API.PlayerHealthPercent >= LoTMHealthPercent && API.MouseoverHealthPercent != 0;
        private bool HolyShockCheck => API.CanCast(HolyShock) && InRange  && API.TargetHealthPercent <= HolyShockLifePercent && API.TargetHealthPercent != 0 && !API.PlayerCanAttackTarget;
        private bool HolyShockCheckMO => API.CanCast(HolyShock) && InRange && IsMouseover && API.MouseoverHealthPercent <= HolyShockLifePercent && API.MouseoverHealthPercent != 0 && !API.PlayerCanAttackMouseover;
        private bool HolyLightCheck => API.CanCast(HolyLight) && InRange  && API.TargetHealthPercent <= HolyLightLifePercent && API.TargetHealthPercent != 0 && !API.PlayerIsMoving && !API.PlayerCanAttackTarget;
        private bool HolyLightCheckMO => API.CanCast(HolyLight) && InRange && IsMouseover && API.MouseoverHealthPercent <= HolyLightLifePercent && API.MouseoverHealthPercent != 0 && !API.PlayerIsMoving && !API.PlayerCanAttackMouseover;
        private bool HolyLightInfusionCheck => API.CanCast(HolyLight) && API.PlayerHasBuff(Infusion) && InRange  && API.TargetHealthPercent <= HoLILifePercent && API.TargetHealthPercent != 0 && !API.PlayerIsMoving && !API.PlayerCanAttackTarget;
        private bool HolyLightInfusionCheckMO => API.CanCast(HolyLight) && API.PlayerHasBuff(Infusion) && IsMouseover && InRange && API.MouseoverHealthPercent <= HoLILifePercent && API.MouseoverHealthPercent != 0 && !API.PlayerIsMoving && !API.PlayerCanAttackMouseover;
        private bool FlashofLightCheck => API.CanCast(FoL) && InRange  && API.TargetHealthPercent <= FoLLifePercent && API.TargetHealthPercent != 0 && !API.PlayerIsMoving && !API.PlayerCanAttackTarget;
        private bool FlashofLightCheckMO => API.CanCast(FoL) && InRange && IsMouseover && API.MouseoverHealthPercent <= FoLLifePercent && API.MouseoverHealthPercent != 0 && !API.PlayerIsMoving && !API.PlayerCanAttackMouseover;
        private bool FlashofLightInfusionCheck => API.CanCast(FoL) && API.PlayerHasBuff(Infusion) && InRange  && API.TargetHealthPercent <= FolILifePercent && API.TargetHealthPercent != 0 && !API.PlayerIsMoving && !API.PlayerCanAttackTarget;
        private bool FlashofLightInfusionCheckMO => API.CanCast(FoL) && API.PlayerHasBuff(Infusion) && IsMouseover && InRange && API.MouseoverHealthPercent <= FolILifePercent && API.MouseoverHealthPercent != 0 && !API.PlayerIsMoving && !API.PlayerCanAttackMouseover;
        private bool WoGCheck => API.CanCast(WoG) && InRange  && (API.PlayerCurrentHolyPower >= 3 || API.PlayerCurrentHolyPower == 5) && API.TargetHealthPercent <= WoGLifePercent && API.TargetHealthPercent != 0 && !API.PlayerCanAttackTarget;
        private bool WoGCheckMO => API.CanCast(WoG) && InRange && IsMouseover && (API.PlayerCurrentHolyPower >= 3 || API.PlayerCurrentHolyPower == 5) && API.MouseoverHealthPercent <= WoGLifePercent && API.MouseoverHealthPercent != 0 && !API.PlayerCanAttackMouseover;
        private bool BFCheck => API.CanCast(BF) && BestowFaith && InRange  && API.TargetHealthPercent <= BFLifePercent && API.TargetHealthPercent != 0 && !API.PlayerCanAttackTarget;
        private bool BFCheckMO => API.CanCast(BF) && BestowFaith && InRange && IsMouseover && API.MouseoverHealthPercent <= BFLifePercent && API.MouseoverHealthPercent != 0 && !API.PlayerCanAttackMouseover;
        private bool LoDCheck => API.CanCast(LoD) && (IsMouseover || !IsMouseover) && (API.PlayerCurrentHolyPower >= 3 || API.PlayerCurrentHolyPower == 5) && (API.TargetRange <= 15 || API.MouseoverRange <= 15) && LoDAoE && (API.TargetHealthPercent > 0 || API.MouseoverHealthPercent > 0) && (API.PlayerCanAttackTarget || !API.PlayerCanAttackTarget || API.PlayerCanAttackMouseover || !API.PlayerCanAttackMouseover);
        private bool BoVCheck => API.CanCast(BoV) && BeaconofVirtue  && InRange && BoVAoE && API.TargetHealthPercent != 0 && !API.PlayerCanAttackTarget;
        private bool BoVCheckMO => API.CanCast(BoV) && BeaconofVirtue && InRange && IsMouseover && BoVAoE && API.MouseoverHealthPercent != 0 && !API.PlayerCanAttackMouseover;
        private bool DTCheck => API.CanCast(DivineToll)  && DTAoE && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackTarget && API.TargetHealthPercent != 0;
        private bool DTCheckMO => API.CanCast(DivineToll) && IsMouseover && DTHealing && DTAoE && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !API.PlayerCanAttackMouseover;
        private bool HolyPrismCheck => API.CanCast(HolyPrism)  && HolyPrismT && InRange && HPAoE && API.TargetHealthPercent != 0 && !API.PlayerCanAttackTarget;
        private bool HolyPrismCheckMO => API.CanCast(HolyPrism) && HolyPrismT && InRange && IsMouseover && HPAoE && API.MouseoverHealthPercent != 0 && !API.PlayerCanAttackMouseover;
        private bool LoHCheck => API.CanCast(LoH) && InRange  && API.TargetHealthPercent <= LoHLifePercent && API.TargetHealthPercent != 0 && !API.TargetHasBuff(Forbearance) && !API.PlayerCanAttackTarget;
        private bool LoHCheckMO => API.CanCast(LoH) && InRange && IsMouseover && API.MouseoverHealthPercent <= LoHLifePercent && API.MouseoverHealthPercent != 0 && !API.TargetHasBuff(Forbearance) && !API.PlayerCanAttackMouseover;
        private bool BoSCheck => API.CanCast(BoS) && InRange  && API.TargetHealthPercent <= BoSLifePercent && API.TargetHealthPercent != 0 && !API.PlayerCanAttackTarget && !API.PlayerIsTargetTarget;
        private bool BoSCheckMO => API.CanCast(BoS) && InRange && IsMouseover && API.MouseoverHealthPercent <= BoSLifePercent && API.MouseoverHealthPercent != 0 && !API.PlayerCanAttackTarget && !API.PlayerIsTargetTarget;
        //  public bool isInterrupt => CombatRoutine.GetPropertyBool("KICK") && API.TargetCanInterrupted && API.TargetIsCasting && (API.TargetIsChanneling ? API.TargetElapsedCastTime >= interruptDelay : API.TargetCurrentCastTimeRemaining <= interruptDelay);
        //  public int interruptDelay => random.Next((int)(CombatRoutine.GetPropertyInt("KICKTime") * 0.9), (int)(CombatRoutine.GetPropertyInt("KICKTime") * 1.1));

        public override void Initialize()
        {
            CombatRoutine.Name = "Holy Pally by Ryu";
            API.WriteLog("Welcome to Holy Pally by Ryu");
            API.WriteLog("Be advised this a Beta Rotation: Out of Combat Healing is turned on by default. Please use your Pause key when not in a party.");
            API.WriteLog("All Talents expect PVP Talents and Row 3 talents are supported. All Cooldowns are associated with Cooldown toggle.");
            API.WriteLog("Will add settings for CDs soon, however all CDs will be used when CD on one. If you wish to contorl when to use them, please use the toggle off fuction and use /break marco to cast them when needed");
            API.WriteLog("If you use Lights Hammer Macro, you need to use an /addonname break to stop the rotation to be able to cast it.");
            API.WriteLog("Maunual targeting and Mouseover Supported. You need to create /cast [@mouseover] xxxx where xxx is each of the spells that have MO in the bindings in order for Mouseover to work");
            API.WriteLog("Venthyr and Night Fae Cov's are not supported. You can create a /xxx break marco to use those abilties when you would like at this time.");
            //Buff
            CombatRoutine.AddBuff(Infusion);
            CombatRoutine.AddBuff(AvengingWrath);
            CombatRoutine.AddBuff(AvengingCrusader);
            CombatRoutine.AddBuff(Forbearance);
            CombatRoutine.AddBuff(CrusaderAura);
            CombatRoutine.AddBuff(DevotionAura);

            //Debuff
            CombatRoutine.AddDebuff(Forbearance);
            CombatRoutine.AddDebuff(Cons);

            //Spell
            CombatRoutine.AddSpell(HolyShock, "D2");
            CombatRoutine.AddSpell(FoL, "D4");
            CombatRoutine.AddSpell(HolyLight, "D3");
            CombatRoutine.AddSpell(CrusaderStrike, "D6");
            CombatRoutine.AddSpell(Judgment, "D5");
            CombatRoutine.AddSpell(LoD, "R");
            CombatRoutine.AddSpell(AvengingWrath, "F2");
            CombatRoutine.AddSpell(HolyAvenger, "F8");
            CombatRoutine.AddSpell(AuraMastery, "T");
            CombatRoutine.AddSpell(BoS, "F1");
            CombatRoutine.AddSpell(LoH, "D8");
            CombatRoutine.AddSpell(BoV, "None");
            CombatRoutine.AddSpell(DivineShield, "D0");
            CombatRoutine.AddSpell(BF, "None");
            CombatRoutine.AddSpell(HammerofJustice, "F");
            CombatRoutine.AddSpell(Cons, "None");
            CombatRoutine.AddSpell(DivineProtection, "None");
            CombatRoutine.AddSpell(HammerofWrath, "None");
            CombatRoutine.AddSpell(Seraphim, "None");
            CombatRoutine.AddSpell(AvengingCrusader, "F2");
            CombatRoutine.AddSpell(HolyPrism, "None");
            CombatRoutine.AddSpell(WoG, "None");
            CombatRoutine.AddSpell(LoTM);
            CombatRoutine.AddSpell(CrusaderAura);
            CombatRoutine.AddSpell(DevotionAura);
            // CombatRoutine.AddSpell(Player);
            // CombatRoutine.AddSpell(Party1);
            // CombatRoutine.AddSpell(Party2);
            // CombatRoutine.AddSpell(Party3);
            // CombatRoutine.AddSpell(Party4);
            CombatRoutine.AddSpell(Fleshcraft);
            CombatRoutine.AddSpell(DivineToll);
            CombatRoutine.AddSpell(VanqusihersHammer);
            CombatRoutine.AddSpell(AshenHallow);

            //Mouseover
            CombatRoutine.AddMacro(HolyLight + "MO", "None");
            CombatRoutine.AddMacro(FoL + "MO", "None");
            CombatRoutine.AddMacro(HolyShock + "MO", "None");
            CombatRoutine.AddMacro(BoS + "MO", "None");
            CombatRoutine.AddMacro(LoH + "MO", "None");
            CombatRoutine.AddMacro(BoV + "MO", "None");
            CombatRoutine.AddMacro(HolyPrism + "MO", "None");
            CombatRoutine.AddMacro(LoD + "MO", "R");
            CombatRoutine.AddMacro(CrusaderStrike + "MO");
            CombatRoutine.AddMacro(Judgment + "MO");
            CombatRoutine.AddMacro(HammerofWrath + "MO");
            CombatRoutine.AddMacro(DivineToll + "MO");
            CombatRoutine.AddToggle("Mouseover");
            CombatRoutine.AddMacro(LoTM + "MO");
            CombatRoutine.AddMacro(WoG + "MO");
            CombatRoutine.AddMacro(trinket1);
            CombatRoutine.AddMacro(trinket2);


            //Prop
            CombatRoutine.AddProp(DivineShield, DivineShield + " Life Percent", numbList, "Life percent at which" + DivineShield + "is used, set to 0 to disable", "Defense", 40);
            CombatRoutine.AddProp(DivineProtection, DivineProtection + " Life Percent", numbList, "Life percent at which" + DivineProtection + "is used, set to 0 to disable", "Defense", 50);
            CombatRoutine.AddProp(Fleshcraft, "Fleshcraft", percentListProp, "Life percent at which " + Fleshcraft + " is used, set to 0 to disable set 100 to use it everytime", "Defense", 0);
            CombatRoutine.AddProp("Use Covenant", "Use " + "Covenant Ability", CDUsageWithAOE, "Use " + "Covenant" + "On Cooldown, with Cooldowns, On AOE, Not Used", "Cooldowns", 0);
            CombatRoutine.AddProp("Aura Switch", "Auto Aura Switch", false, "Auto Switch Aura between Crusader Aura|Devotion Aura", "Generic");

            CombatRoutine.AddProp("OOC", "Healing out of Combat", true, "Heal out of combat", "Healing");
            CombatRoutine.AddProp(PartySwap, PartySwap + " Life Percent", numbList, "Life percent at which" + PartySwap + "is used, set to 0 to disable", "Healing", 0);
            CombatRoutine.AddProp(HolyShock, HolyShock + " Life Percent", numbList, "Life percent at which" + HolyShock + "is used, set to 0 to disable", "Healing", 95);
            CombatRoutine.AddProp(HolyShockHealing, HolyShock, true, "If Divine Toll should be on Healing, if for both, change to false, set to true by default for healing", "Healing");
            CombatRoutine.AddProp(HolyLight, HolyLight + " Life Percent", numbList, "Life percent at which" + HolyLight + "is used, set to 0 to disable", "Healing", 85);
            CombatRoutine.AddProp(LoTM, LoTM + " Life Percent", numbList, "Life percent at which" + LoTM + "is used, set to 0 to disable", "Healing", 60);
            CombatRoutine.AddProp(LoTMH, LoTM + " Player Health Percent", numbList, "Player Health percent at which" + LoTM + "is used, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(LoTMM, LoTM + " Player Health Percent", numbList, "Target Health percent at which" + LoTM + "is used while moving, set to 0 to disable", "Healing", 85);
            CombatRoutine.AddProp(HoLI, HoLI + " Life Percent", numbList, "Life percent at which" + HoLI + "is used, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(FoL, FoL + " Life Percent", numbList, "Life percent at which" + FoL + "is used, set to 0 to disable", "Healing", 75);
            CombatRoutine.AddProp(FoLI, FoLI + " Life Percent", numbList, "Life percent at which" + FoLI + "is used, set to 0 to disable", "Healing", 90);
            CombatRoutine.AddProp(WoG, WoG + " Life Percent", numbList, "Life percent at which" + WoG + "is used, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(BoS, BoS + " Life Percent", numbList, "Life percent at which" + BoS + "is used, set to 0 to disable", "Healing", 20);
            CombatRoutine.AddProp(LoH, LoH + " Life Percent", numbList, "Life percent at which" + LoH + "is used, set to 0 to disable", "Healing", 10);
            CombatRoutine.AddProp(BF, BF + " Life Percent", numbList, "Life percent at which" + BF + "is used, set to 0 to disable", "Healing", 95);
            CombatRoutine.AddProp(LoD, LoD + " Life Percent", numbList, "Life percent at which" + LoD + "is used when three members are at life percent, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(BoV, BoV + " Life Percent", numbList, "Life percent at which" + BoV + "is used when three members are at life percent, set to 0 to disable", "Healing", 75);
            CombatRoutine.AddProp(HolyPrism, HolyPrism + " Life Percent", numbList, "Life percent at which" + HolyPrism + "is used when three members are at life percent, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(DivineToll, DivineToll + " Life Percent", numbList, "Life percent at which" + DivineToll + "is used when three members are at life percent, set to 0 to disable", "Healing", 80);
            CombatRoutine.AddProp(DivineTollHealing, DivineToll, true, "If Divine Toll should be on Healing, if for both, change to false, set to true by default for healing", "Healing");
            CombatRoutine.AddProp(AoE, "Number of units for AoE Healing ", numbPartyList, " Units for AoE Healing", "Healing", 3);

            CombatRoutine.AddProp("Trinket1", "Trinket1 usage", CDUsage, "When should trinket1 be used", "Trinket", 0);
            CombatRoutine.AddProp("Trinket2", "Trinket2 usage", CDUsage, "When should trinket1 be used", "Trinket", 0);

            //                 if (PlayerSwap && API.TargetHealthPercent <= PartySwapPercent)
            //{
            //    API.CastSpell(Player);
            //    API.WriteLog("Player Health : " + API.UnitHealthPercent(Player));
            //}
            //if (Party1Swap && API.TargetHealthPercent <= PartySwapPercent)
            // {
            //     API.CastSpell(Party1);
            //     API.WriteLog("Party1 Health : " + API.UnitHealthPercent(Party1));
            // }
            // if (Party2Swap && API.TargetHealthPercent <= PartySwapPercent)
            // {
            //     API.CastSpell(Party2);
            //     API.WriteLog("Party2 Health : " + API.UnitHealthPercent(Party2));
            // }
            // if (Party3Swap && API.TargetHealthPercent <= PartySwapPercent)
            // {
            //     API.CastSpell(Party3);
            //     API.WriteLog("Party3 Health : " + API.UnitHealthPercent(Party3));
            // }
            // if (Party4Swap && API.TargetHealthPercent <= PartySwapPercent)
            // {
            //  API.CastSpell(Party4);
            //   API.WriteLog("Party4 Health : " + API.UnitHealthPercent(Party4));
            //}
        }

        public override void Pulse()
        {
            if (!API.PlayerIsMounted && (IsOOC || API.PlayerIsInCombat))
            {
                if (API.PlayerIsMounted)
                {
                    if (AutoAuraSwitch && API.CanCast(CrusaderAura) && Level >= 21 && !API.PlayerHasBuff(CrusaderAura))
                    {
                        API.CastSpell(CrusaderAura);
                        return;
                    }
                }
                else
                {

                    if (AutoAuraSwitch && API.CanCast(DevotionAura) && Level >= 21 && !API.PlayerHasBuff(DevotionAura))
                    {
                        API.CastSpell(DevotionAura);
                        return;
                    }
                }
                if (Trinket1Usage == 1 && IsCooldowns && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                    API.CastSpell(trinket1);
                if (Trinket1Usage == 2 && API.PlayerTrinketIsUsable(1) && API.PlayerTrinketRemainingCD(1) == 0)
                    API.CastSpell(trinket1);
                if (Trinket2Usage == 1 && IsCooldowns && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                    API.CastSpell(trinket2);
                if (Trinket2Usage == 2 && API.PlayerTrinketIsUsable(2) && API.PlayerTrinketRemainingCD(2) == 0)
                    API.CastSpell(trinket2);
                if (API.CanCast(AvengingWrath) && !API.PlayerHasBuff(AvengingWrath) && InRange && IsCooldowns)
                {
                    API.CastSpell(AvengingWrath);
                    return;
                }
                if (API.CanCast(AvengingCrusader) && AvengingCrusaderT && !API.PlayerHasBuff(AvengingCrusader) && InRange && IsCooldowns)
                {
                    API.CastSpell(AvengingCrusader);
                    return;
                }
                if (API.CanCast(HolyAvenger) && HolyAvengerT && InRange && IsCooldowns)
                {
                    API.CastSpell(HolyAvenger);
                    return;
                }
                if (API.CanCast(Seraphim) && SeraphimT && InRange && IsCooldowns && API.PlayerCurrentHolyPower >= 3)
                {
                    API.CastSpell(Seraphim);
                    return;
                }
                if (API.CanCast(DivineShield) && API.PlayerHealthPercent <= DivineShieldLifePercent && API.PlayerHealthPercent != 0 && !API.PlayerHasDebuff(Forbearance))
                {
                    API.CastSpell(DivineShield);
                    return;
                }
                if (API.CanCast(DivineProtection) && API.PlayerHealthPercent <= DivineProtectionLifePercent && API.PlayerHealthPercent != 0)
                {
                    API.CastSpell(DivineProtection);
                    return;
                }
                if (DTCheck)
                {
                    API.CastSpell(DivineToll);
                    return;
                }
                if (BoVCheck)
                {
                    API.CastSpell(BoV);
                    return;
                }
                if (HolyPrismCheck)
                {
                    API.CastSpell(HolyPrism);
                    return;
                }
                if (LoDCheck)
                {
                    API.CastSpell(LoD);
                    return;
                }
                if (LoHCheck)
                {
                    API.CastSpell(LoH);
                    return;
                }
                if (BoSCheck)
                {
                    API.CastSpell(BoS);
                    return;
                }
                if (WoGCheck)
                {
                    API.CastSpell(WoG);
                    return;
                }
                if (HolyShockCheck)
                {
                    API.CastSpell(HolyShock);
                    return;
                }
                if (FlashofLightCheck)
                {
                    API.CastSpell(FoL);
                    return;
                }
                if (HolyLightCheck)
                {
                    API.CastSpell(HolyLight);
                    return;
                }
                if (FlashofLightInfusionCheck)
                {
                    API.CastSpell(FoL);
                    return;
                }
                if (HolyLightInfusionCheck)
                {
                    API.CastSpell(HolyLight);
                    return;
                }
                if (BFCheck)
                {
                    API.CastSpell(BF);
                    return;
                }
                if (LoTMCheck)
                {
                    API.CastSpell(LoTM);
                    return;
                }
                /// Mouseover
                if (BoVCheckMO)
                {
                    API.CastSpell(BoV + "MO");
                    return;
                }
                if (HolyPrismCheckMO)
                {
                    API.CastSpell(HolyPrism + "MO");
                    return;
                }
                if (DTCheckMO)
                {
                    API.CastSpell(DivineToll + "MO");
                    return;
                }
                if (LoHCheckMO)
                {
                    API.CastSpell(LoH + "MO");
                    return;
                }
                if (BoSCheckMO)
                {
                    API.CastSpell(BoS + "MO");
                    return;
                }
                if (WoGCheckMO)
                {
                    API.CastSpell(WoG + "MO");
                    return;
                }
                if (HolyShockCheckMO)
                {
                    API.CastSpell(HolyShock + "MO");
                    return;
                }
                if (FlashofLightCheckMO)
                {
                    API.CastSpell(FoL + "MO");
                    return;
                }
                if (HolyLightCheckMO)
                {
                    API.CastSpell(HolyLight + "MO");
                    return;
                }
                if (FlashofLightInfusionCheckMO)
                {
                    API.CastSpell(FoL + "MO");
                    return;
                }
                if (HolyLightInfusionCheckMO)
                {
                    API.CastSpell(HolyLight + "MO");
                    return;
                }
                if (BFCheckMO)
                {
                    API.CastSpell(BF + "MO");
                    return;
                }
                if (LoTMCheckMO)
                {
                    API.CastSpell(LoTM + "MO");
                    return;
                }
            }
        }
        public override void CombatPulse()
        {
            if (API.CanCast(Fleshcraft) && PlayerCovenantSettings == "Necrolord" && API.PlayerHealthPercent <= FleshcraftPercentProc)
            {
                API.CastSpell(Fleshcraft);
                return;
            }
            if (API.CanCast(Judgment) && InRange && API.PlayerCanAttackTarget)
            {
                API.CastSpell(Judgment);
                return;
            }
            if (API.CanCast(Judgment) && IsMouseover && InRange && API.PlayerCanAttackMouseover && API.MouseoverIsIncombat)
            {
                API.CastSpell(Judgment + "MO");
                return;
            }
            if (API.CanCast(CrusaderStrike)  && CrusadersMight && API.SpellISOnCooldown(HolyShock) && IsMelee && API.PlayerCanAttackTarget && !DPSHealthCheck)
            {
                API.CastSpell(CrusaderStrike);
                return;
            }
            if (API.CanCast(CrusaderStrike) && IsMouseover && CrusadersMight && API.SpellISOnCooldown(HolyShock) && IsMoMelee && API.PlayerCanAttackMouseover && API.MouseoverIsIncombat && !DPSHealthCheck)
            {
                API.CastSpell(CrusaderStrike + "MO");
                return;
            }
            if (API.CanCast(CrusaderStrike)  && Level <= 25 && !CrusadersMight && IsMelee && API.PlayerCanAttackTarget && !DPSHealthCheck)
            {
                API.CastSpell(CrusaderStrike);
                return;
            }
            if (API.CanCast(CrusaderStrike) && IsMouseover && Level <= 25 && !CrusadersMight && IsMoMelee && API.PlayerCanAttackMouseover && API.MouseoverIsIncombat && !DPSHealthCheck)
            {
                API.CastSpell(CrusaderStrike + "MO");
                return;
            }
            if (API.CanCast(HolyShock) && !HSHealing && InRange && API.PlayerCanAttackTarget && !DPSHealthCheck)
            {
                API.CastSpell(HolyShock);
                return;
            }
            if (API.CanCast(HolyShock) && !HSHealing && IsMouseover && InRange && API.PlayerCanAttackMouseover && API.MouseoverIsIncombat && !DPSHealthCheck)
            {
                API.CastSpell(HolyShock + "MO");
                return;
            }
            if (API.CanCast(DivineToll) && !DTHealing && PlayerCovenantSettings == "Kyrian"  && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && API.PlayerCanAttackTarget)
            {
                API.CastSpell(DivineToll);
                return;
            }
            if (API.CanCast(DivineToll) && IsMouseover && !DTHealing && PlayerCovenantSettings == "Kyrian" && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && API.PlayerCanAttackMouseover && API.MouseoverIsIncombat)
            {
                API.CastSpell(DivineToll + "MO");
                return;
            }
            if (API.CanCast(VanqusihersHammer) && PlayerCovenantSettings == "Necrolord"  && (UseCovenant == "With Cooldowns" && IsCooldowns || UseCovenant == "On Cooldown" || UseCovenant == "on AOE" && IsAOE) && NotChanneling && !DPSHealthCheck)
            {
                API.CastSpell(VanqusihersHammer);
                return;
            }
            if (API.CanCast(HammerofWrath) && InRange && API.TargetHealthPercent <= 20  && Level >= 46 || (Level >= 58 && API.PlayerHasBuff(AvengingWrath) && InRange) && !DPSHealthCheck)
            {
                API.CastSpell(HammerofWrath);
                return;
            }
            if (API.CanCast(HammerofWrath) && InRange && IsMouseover && API.MouseoverHealthPercent <= 20 && Level >= 46 || (Level >= 58 && API.PlayerHasBuff(AvengingWrath) && InRange) && API.PlayerCanAttackMouseover && API.MouseoverIsIncombat && !DPSHealthCheck)
            {
                API.CastSpell(HammerofWrath + "MO");
                return;
            }
            if (API.CanCast(Cons) && API.PlayerUnitInMeleeRangeCount >= 2  && IsMelee && !API.TargetHasDebuff(Cons) && !API.PlayerIsMoving && !DPSHealthCheck)
            {
                API.CastSpell(Cons);
                return;
            }
            if (API.CanCast(Cons) && API.PlayerUnitInMeleeRangeCount >= 2 && IsMouseover && IsMoMelee && !API.MouseoverHasDebuff(Cons) && !API.PlayerIsMoving && API.PlayerCanAttackMouseover && API.MouseoverIsIncombat && !DPSHealthCheck)
            {
                API.CastSpell(Cons + "MO");
                return;
            }

        }

        public override void OutOfCombatPulse()
        {

        }
        private void rotation()
        {

        }
    }    
} 




