using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CsvHelper;
using PokemonUnity.Monster;

namespace PokemonUnity.Editor
{
    public class Pokemon : PokemonUnity.Monster.Pokemon.PokemonData
    {
        ///<summary>Int value doesnt matter, ex `.ToString(1);`</summary>
        public override string ToString()
        {
            return string.Format("new PokemonData({0} {1} {2} {3} {4} {5} {6} {7} {8} {9} " +
                                                "{10} {11} {12} {13} {14} {15} {16} {17} {18} {19} " +
                                                "{20} {21} {22} {23} {24} {25} {26} {27} {28} {29} " +
                                                "{30} {31}),",
                ID == Pokemons.NONE ? ""			: "\nId: Pokemons." + ID.ToString(),
                RegionalDex <= 0 ? ""				: ",\n//regionalDex: new int[]{" + RegionalDex.ToString() + "}",
                Type[0] == Types.NONE ? ""			: ",\ntype1: Types." + Type[0].ToString(),
                Type[1] == Types.NONE ? ""			: ",\ntype2: Types." + Type[1].ToString(),
                Ability[0] == Abilities.NONE ? ""	: ",\nability1: Abilities." + Ability[0],
                Ability[1] == Abilities.NONE ? ""	: ",\nability2: Abilities." + Ability[1],
                Ability[2] == Abilities.NONE ? ""	: ",\nhiddenAbility: Abilities." + Ability[2],
                /*GenderEnum == null ? ""			: */",\nmaleRatio: " + GenderEnum,
                CatchRate <= 0 ? ""					: ",\ncatchRate: " + CatchRate,
                EggGroup[0] == EggGroups.NONE ? ""	: ",\neggGroup1: EggGroups." + EggGroup[0],
                EggGroup[1] == EggGroups.NONE ? ""	: ",\neggGroup2: EggGroups." + EggGroup[1],
                HatchTime <= 0 ? ""					: ",\nhatchTime: " + HatchTime,
                Height <= 0 ? ""					: ",\nheight: " + Height,
                Weight <= 0 ? ""					: ",\nweight: " + Weight,
                /*GrowthRate == null ? ""			: */",\nlevelingRate: LevelingRate." + GrowthRate.ToString(),//\n" +
                PokedexColor == Color.NONE ? ""		: ",\npokedexColor: Color." + PokedexColor.ToString(),
                BaseFriendship <= 0 ? ""			: ",\nbaseFriendship: " + BaseFriendship,
                BaseExpYield <= 0 ? ""				: ",\nbaseExpYield: " + BaseExpYield.ToString(),
                BaseStatsHP  <= 0 ? ""				: ",\nbaseStatsHP: " + BaseStatsHP.ToString(),
                BaseStatsATK <= 0 ? ""				: ",baseStatsATK: "  + BaseStatsATK.ToString(),
                BaseStatsDEF <= 0 ? ""				: ",baseStatsDEF: "  + BaseStatsDEF.ToString(),
                BaseStatsSPA <= 0 ? ""				: ",baseStatsSPA: "  + BaseStatsSPA.ToString(),
                BaseStatsSPD <= 0 ? ""				: ",baseStatsSPD: "  + BaseStatsSPD.ToString(),
                BaseStatsSPE <= 0 ? ""				: ",baseStatsSPE: "  + BaseStatsSPE.ToString(),//\n" +
                evYieldHP  <= 0 ? ""				: ",\nevHP: "  + evYieldHP.ToString(),
                evYieldATK <= 0 ? ""				: ",\nevATK: " + evYieldATK.ToString(),
                evYieldDEF <= 0 ? ""				: ",\nevDEF: " + evYieldDEF.ToString(),
                evYieldSPA <= 0 ? ""				: ",\nevSPA: " + evYieldSPA.ToString(),
                evYieldSPD <= 0 ? ""				: ",\nevSPD: " + evYieldSPD.ToString(),
                evYieldSPE <= 0 ? ""				: ",\nevSPE: " + evYieldSPE.ToString(),//\n" +
                //NAME == null? "" : "$"luminance: "+Luminance,\n" +
				//ToDo: Fix Moves and Evolution...
                ",\nmovesetmoves: new PokemonMoveset[] " +
                //$"\n" +
                "{\n" +
                    //trimend goes on last value, but there's also a comma before every value so it's fine
                    //it balances out the extra, and also if the value is null, you're not left with additional
                    //or dangling commas to trigger any errors.
                    "\n"+//Moves.Replace("'","").TrimEnd(',') + "\n" +
                "}",
                //$"\n" +
                ""//PokemonEvolution == null ? "" : ",\nevolution: new IPokemonEvolution[] {" + PokemonEvolution.TrimEnd(',') + "\n}"
                //,HeldItem == null? "" : "heldItem: "+HeldItem +","
             );
        }
        public XmlElement ToXML()
        {
			XmlDocument xmlDoc = new XmlDocument();
			XmlElement pkmn = xmlDoc.CreateElement("Pokemon");
			//XmlAttribute name = xmlDoc.CreateAttribute("name");
			//name.Value = "John Doe";
			//XmlAttribute value = xmlDoc.CreateAttribute("value");
			//value.Value = "99";
			//pkmn.SetAttributeNode(name);
			//pkmn.SetAttributeNode(value);
			XmlAttribute name = xmlDoc.CreateAttribute("Speces");	name	.Value = ((int)ID).ToString();// == Pokemons.NONE ? ""			: "\nId: Pokemons." + ID.ToString();
			XmlAttribute dex = xmlDoc.CreateAttribute("ReDex");		dex		.Value = RegionalDex.ToString();// <= 0 ? ""				: ",\n//regionalDex: new int[]{" + RegionalDex.ToString() + "}";
			XmlAttribute t1 = xmlDoc.CreateAttribute("Type1");		t1		.Value = ((int)Type[0]).ToString();// == Types.NONE ? ""			: ",\ntype1: Types." + Type[0].ToString();
			XmlAttribute t2 = xmlDoc.CreateAttribute("Type2");		t2		.Value = ((int)Type[1]).ToString();// == Types.NONE ? ""			: ",\ntype2: Types." + Type[1].ToString();
			XmlAttribute a1 = xmlDoc.CreateAttribute("Ability1");	a1		.Value = ((int)Ability[0]).ToString();// == Abilities.NONE ? ""	: ",\nability1: Abilities." + Ability[0];
			XmlAttribute a2 = xmlDoc.CreateAttribute("Ability2");	a2		.Value = ((int)Ability[1]).ToString();// == Abilities.NONE ? ""	: ",\nability2: Abilities." + Ability[1];
			XmlAttribute ah = xmlDoc.CreateAttribute("Ability3");	ah		.Value = ((int)Ability[2]).ToString();// == Abilities.NONE ? ""	: ",\nhiddenAbility: Abilities." + Ability[2];
			XmlAttribute male = xmlDoc.CreateAttribute("Gender");	male	.Value = ((int)GenderEnum).ToString();// == null ? ""			: */",\nmaleRatio: " + GenderEnum;
			XmlAttribute rate = xmlDoc.CreateAttribute("Catch");	rate	.Value = CatchRate.ToString();// <= 0 ? ""				: ",\ncatchRate: " + CatchRate;
			XmlAttribute egg1 = xmlDoc.CreateAttribute("Egg1");		egg1	.Value = ((int)EggGroup[0]).ToString();// == EggGroups.NONE ? ""	: ",\neggGroup1: EggGroups." + EggGroup[0];
			XmlAttribute egg2 = xmlDoc.CreateAttribute("Egg2");		egg2	.Value = ((int)EggGroup[1]).ToString();// == EggGroups.NONE ? ""	: ",\neggGroup2: EggGroups." + EggGroup[1];
			XmlAttribute time = xmlDoc.CreateAttribute("Hatch");	time	.Value = HatchTime.ToString();// <= 0 ? ""				: ",\nhatchTime: " + HatchTime;
			XmlAttribute h  = xmlDoc.CreateAttribute("Height");		h		.Value = Height.ToString();// <= 0 ? ""					: ",\nheight: " + Height;
			XmlAttribute w  = xmlDoc.CreateAttribute("Weight");		w		.Value = Weight.ToString();// <= 0 ? ""					: ",\nweight: " + Weight;
			XmlAttribute lvr = xmlDoc.CreateAttribute("Growth");	lvr		.Value = ((int)GrowthRate).ToString();// == null ? ""			: */",\nlevelingRate: LevelingRate." + GrowthRate.ToString();
			XmlAttribute clr = xmlDoc.CreateAttribute("Color");		clr		.Value = ((int)PokedexColor).ToString();// == Color.NONE ? ""	: ",\npokedexColor: Color." + PokedexColor.ToString();
			XmlAttribute frnd = xmlDoc.CreateAttribute("Friend");	frnd	.Value = BaseFriendship.ToString();// <= 0 ? ""			: ",\nbaseFriendship: " + BaseFriendship;
			XmlAttribute exp = xmlDoc.CreateAttribute("Exp");		exp		.Value = BaseExpYield.ToString();// <= 0 ? ""				: ",\nbaseExpYield: " + BaseExpYield.ToString();
			XmlAttribute hp   = xmlDoc.CreateAttribute("HP");		hp		.Value = BaseStatsHP .ToString();// <= 0 ? ""				: ",\nbaseStatsHP: " + BaseStatsHP.ToString();
			XmlAttribute atk  = xmlDoc.CreateAttribute("ATK");		atk		.Value = BaseStatsATK.ToString();// <= 0 ? ""				: ",baseStatsATK: "  + BaseStatsATK.ToString();
			XmlAttribute def  = xmlDoc.CreateAttribute("DEF");		def		.Value = BaseStatsDEF.ToString();// <= 0 ? ""				: ",baseStatsDEF: "  + BaseStatsDEF.ToString();
			XmlAttribute spa  = xmlDoc.CreateAttribute("SPA");		spa		.Value = BaseStatsSPA.ToString();// <= 0 ? ""				: ",baseStatsSPA: "  + BaseStatsSPA.ToString();
			XmlAttribute spd  = xmlDoc.CreateAttribute("SPD");		spd		.Value = BaseStatsSPD.ToString();// <= 0 ? ""				: ",baseStatsSPD: "  + BaseStatsSPD.ToString();
			XmlAttribute spe  = xmlDoc.CreateAttribute("SPE");		spe		.Value = BaseStatsSPE.ToString();// <= 0 ? ""				: ",baseStatsSPE: "  + BaseStatsSPE.ToString();
			XmlAttribute ehp  = xmlDoc.CreateAttribute("eHP");		ehp		.Value = evYieldHP .ToString();// <= 0 ? ""				: ",\nevHP: "  + evYieldHP.ToString();
			XmlAttribute eatk = xmlDoc.CreateAttribute("eATK");		eatk	.Value = evYieldATK.ToString();// <= 0 ? ""				: ",\nevATK: " + evYieldATK.ToString();
			XmlAttribute edef = xmlDoc.CreateAttribute("eDEF");		edef	.Value = evYieldDEF.ToString();// <= 0 ? ""				: ",\nevDEF: " + evYieldDEF.ToString();
			XmlAttribute espa = xmlDoc.CreateAttribute("eSPA");		espa	.Value = evYieldSPA.ToString();// <= 0 ? ""				: ",\nevSPA: " + evYieldSPA.ToString();
			XmlAttribute espd = xmlDoc.CreateAttribute("eSPD");		espd	.Value = evYieldSPD.ToString();// <= 0 ? ""				: ",\nevSPD: " + evYieldSPD.ToString();
			XmlAttribute espe = xmlDoc.CreateAttribute("eSPE");		espe	.Value = evYieldSPE.ToString();// <= 0 ? ""				: ",\nevSPE: " + evYieldSPE.ToString();//\n" +
			XmlAttribute gen = xmlDoc.CreateAttribute("GenerationId");		gen		.Value = GenerationId.ToString();
			XmlAttribute shape = xmlDoc.CreateAttribute("ShapeId");			shape	.Value = ((int)ShapeId).ToString();
			XmlAttribute habit = xmlDoc.CreateAttribute("HabitatId");		habit	.Value = ((int)HabitatId).ToString();
			XmlAttribute rare = xmlDoc.CreateAttribute("Rarity");			rare	.Value = ((int)Rarity).ToString();
			XmlAttribute dflt = xmlDoc.CreateAttribute("IsBaseForm");		dflt	.Value = IsBaseForm == true ? "1"	: "0";
			XmlAttribute baby = xmlDoc.CreateAttribute("IsBaby");			baby	.Value = IsBaby == true ? "1"	: "0";
			XmlAttribute swch = xmlDoc.CreateAttribute("SwitchableForm");	swch	.Value = FormSwitchable == true ? "1"	: "0";
			XmlAttribute diff = xmlDoc.CreateAttribute("GenderDiff");		diff	.Value = HasGenderDiff == true ? "1"	: "0";
			//XmlAttribute sngl = xmlDoc.CreateAttribute("SingleGender");		sngl	.Value = IsSingleGendered == true ? "1"	: "0";
            //   //NAME == null? "" : "$"luminance: "+Luminance,\n" +
            //   ",\nmovesetmoves: new PokemonMoveset[] " +
            //   //$"\n" +
            //   "{\n" +
            //       //trimend goes on last value, but there's also a comma before every value so it's fine
            //       //it balances out the extra, and also if the value is null, you're not left with additional
            //       //or dangling commas to trigger any errors.
            //       Moves.Replace("'","").TrimEnd(',') + "\n" +
            //   "}",
            //   //$"\n" +
            //   PokemonEvolution == null ? "" : ",\nevolution: new IPokemonEvolution[] {" + PokemonEvolution.TrimEnd(',') + "\n}"
            //   //,HeldItem == null? "" : "heldItem: "+HeldItem +","
            //);
			pkmn.SetAttributeNode(name	);	
			pkmn.SetAttributeNode(dex	);	
			pkmn.SetAttributeNode(t1	);	
			pkmn.SetAttributeNode(t2	);	
			pkmn.SetAttributeNode(a1	);	
			pkmn.SetAttributeNode(a2	);	
			pkmn.SetAttributeNode(ah	);	
			pkmn.SetAttributeNode(male	);	
			pkmn.SetAttributeNode(rate	);	
			pkmn.SetAttributeNode(egg1	);	
			pkmn.SetAttributeNode(egg2	);	
			pkmn.SetAttributeNode(time	);	
			pkmn.SetAttributeNode(h		);	
			pkmn.SetAttributeNode(w		);	
			pkmn.SetAttributeNode(lvr	);	
			pkmn.SetAttributeNode(clr	);	
			pkmn.SetAttributeNode(frnd	);	
			pkmn.SetAttributeNode(exp	);	
			pkmn.SetAttributeNode(hp	);	
			pkmn.SetAttributeNode(atk	);	
			pkmn.SetAttributeNode(def	);	
			pkmn.SetAttributeNode(spa	);	
			pkmn.SetAttributeNode(spd	);	
			pkmn.SetAttributeNode(spe	);	
			pkmn.SetAttributeNode(ehp	);	
			pkmn.SetAttributeNode(eatk	);	
			pkmn.SetAttributeNode(edef	);	
			pkmn.SetAttributeNode(espa	);	
			pkmn.SetAttributeNode(espd	);
			pkmn.SetAttributeNode(espe	);
			pkmn.SetAttributeNode(gen	);
			pkmn.SetAttributeNode(shape	);
			pkmn.SetAttributeNode(habit	);
			pkmn.SetAttributeNode(rare	);
			pkmn.SetAttributeNode(dflt	);
			pkmn.SetAttributeNode(baby	);
			pkmn.SetAttributeNode(swch	);
			pkmn.SetAttributeNode(diff	);
			//pkmn.SetAttributeNode(sngl	);
			return pkmn;
        }
    }
}