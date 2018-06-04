using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Text.RegularExpressions;
using System.Threading;
using Console = Colorful.Console;
using System.Drawing;

namespace CSVDatabaseReader
{
    class Program
    {
        static void Main(string[] args)
        {

            string[] Generations = new string[16]
            {
                "Blue and Red", "Yellow", "Gold and Silver", "Crystal", "Ruby and Sapphire", "Emerald", "FireRed and LeafGreen", "Diamond and Pearl", "Platinum", "Heartgold and Soulsilver", "Black and White", "Black 2 and White 2", "X and Y", "Omega Ruby and Alpha Sapphire", "Sun and Moon", "Ultra Sun and Ultra Moon"
            };
            Console.Title = "Veekun's CSV Database to Pokemon Unity's PokemonDatabase Converter ~ by Velorexe";
            Console.WriteAscii("VEEKUN TO PKUNITY", Color.FromArgb(66, 167, 199));
            Console.WriteLine("This tool is created by Velorexe for the Pokemon Unity project to easily convert the Veekun Pokemon Database to the format that is used in Pokemon Unity");
            Console.WriteLine("Please fill in the source path to the CSV Pokemon Database from Veekun. This should be a direct path to the directory.\nExample: C:/Users/Velorexe/Desktop/PokemonSprites/PokemonDatabase/Veekun Database/CSV\n");
            string SourcePath = Console.ReadLine();
            string[] csvFiles = Directory.GetFiles(SourcePath);
            csvFiles = csvFiles.Where(w => Path.GetExtension(w) == ".csv").ToArray();
            while (csvFiles.Length != 172)
            {
                Console.WriteLine("The folder should contain 172 CSV files, yours only contains " + csvFiles.Length + ". Please fill in the correct path.");
                SourcePath = Console.ReadLine();
                csvFiles = Directory.GetFiles(SourcePath);
                csvFiles = csvFiles.Where(w => Path.GetExtension(w) == ".csv").ToArray();
            }
            Console.WriteLine();
            Console.WriteLine($"{csvFiles.Length} csv files found.\n");
            Console.Clear();
            Console.WriteLine("Wich generation would you like to convert? Please press a key to load the generations. (Every key except 'Enter' please I haven't optimised the code yet)");
            Console.WriteLine("---------------------------");

            int SelectedItem = 0;
            var k = Console.ReadKey();
            while (k.Key != ConsoleKey.Enter)
            {
                if (k.Key == ConsoleKey.UpArrow)
                {
                    SelectedItem--;
                }
                else if (k.Key == ConsoleKey.DownArrow)
                {
                    SelectedItem++;
                }
                Console.Clear();
                Console.WriteLine("Wich generation would you like to convert?");
                Console.WriteLine("---------------------------");
                for (int i = 0; i < Generations.Length; i++)
                {
                    if (SelectedItem == Generations.Length)
                    {
                        SelectedItem = 0;
                    }
                    else if (SelectedItem == -1)
                    {
                        SelectedItem = Generations.Length - 1;
                    }
                    if (i == SelectedItem)
                    {
                        Console.ForegroundColor = Color.Blue;
                        Console.WriteLine(Generations[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(Generations[i]);
                    }
                }
                Console.WriteLine("---------------------------");
                k = Console.ReadKey();
            }
            int displayItem = SelectedItem;
            SelectedItem++;
            if (SelectedItem > 11)
            {
                SelectedItem = SelectedItem + 2;
            }
            Console.WriteLine("Converting now...\n");

            File.Delete(SourcePath + @"\POKEMONOUTPUT " + Generations[displayItem] + ".txt");
            StreamWriter Output = File.CreateText(SourcePath + @"\POKEMONOUTPUT " + Generations[displayItem] + ".txt");
            Output.Dispose();

            int PokemonCounter = 1;
            int MaxPoke = 0;
            string Generation = SelectedItem.ToString();

            /*
            Corresponding numbers with gens
            1 Red and blue
            2 Yellow
            3 Gold and Silver
            4 Crystal
            5 Sapphire
            6 Emerald
            7 FireRed and LeafGreen
            8 Diamond and Pearl
            9 Platinum
            10 Heartgold and Soulsilver
            11 Black And White
            14 Black 2 and White 2
            15 X and Y
            16 Omega Ruby and Alpha Sapphire
            17 Sun and Moon
            18 Ultra Sun and Ultra Moon
             */

            int Gen1 = 151;
            int Gen2 = 251;
            int Gen3 = 386;
            int Gen4 = 493;
            int Gen5 = 649;
            int Gen6 = 721;
            int Gen7 = 807;

            #region Generation
            switch (SelectedItem)
            {
                case (1):
                case (2):
                    MaxPoke = Gen1;
                    break;
                case (3):
                case (4):
                    MaxPoke = Gen2;
                    break;
                case (5):
                case (6):
                case (7):
                    MaxPoke = Gen3;
                    break;
                case (8):
                case (9):
                case (10):
                    MaxPoke = Gen4;
                    break;
                case (11):
                case (14):
                    MaxPoke = Gen5;
                    break;
                case (15):
                case (16):
                    MaxPoke = Gen6;
                    break;
                case (17):
                case (18):
                    MaxPoke = Gen7;
                    break;
            }
            #endregion

            int RegionDex = 1;
            Pokemon[] Pokemons = new Pokemon[MaxPoke];

            while (PokemonCounter < MaxPoke + 1)
            {
                string Ability1 = "";
                string Ability2 = "";
                string HiddenAbility = "";

                string Type1 = "";
                string Type2 = "";

                string EggGroup1 = "";
                string EggGroup2 = "";

                string GrowthRate = "";

                string Color_ID = "";

                int EvolutionID = 0;

                Dictionary<string, int> MoveLevelDictionary = new Dictionary<string, int>();
                List<string> TMList = new List<string>();

                string Entry = PokemonCounter.ToString();

                Pokemon Pokemon = new Pokemon();

                if (RegionDex == Gen1 || RegionDex == Gen2 || RegionDex == Gen3 || RegionDex == Gen4 || RegionDex == Gen5 || RegionDex == Gen6 || RegionDex == Gen7)
                {
                    RegionDex = 1;
                }
                Pokemon.RegionalDex = PokemonCounter.ToString();

                //Pokemon
                TextReader CsvReader = File.OpenText(csvFiles[129]);
                var csv = new CsvReader(CsvReader);

                Pokemon.ID = Entry;
                Pokemon.LightColor = "clear";
                Pokemon.Luminance = "0";
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == Entry)
                    {
                        Pokemon.NAME = UpperCaseFirst(csv.GetField<string>(1));
                        Pokemon.Weight = csv.GetField<double>(4) / 10.0 + "f";
                        Pokemon.Height = csv.GetField<double>(3) / 10.0 + "f";
                        Pokemon.EXPYield = csv.GetField<string>(5);
                    }
                }
                Console.WriteLine(($"Pokemon ID: {PokemonCounter}, Name: {Pokemon.NAME.ToUpper()}"));
                Console.Write("Progress |");

                //Pokemon_Types
                CsvReader = File.OpenText(csvFiles[155]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == Entry)
                    {
                        Type1 = csv.GetField<string>(1);
                        csv.Read();
                        if (csv.GetField<string>(0) == Entry)
                        {
                            Type2 = csv.GetField<string>(1);
                        }
                    }
                }
                Console.Write("+");

                //Types
                CsvReader = File.OpenText(csvFiles[163]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == Type1)
                    {
                        Type1 = csv.GetField<string>(1).ToUpper();
                        Pokemon.Type1 = Type1;
                    }
                    else if (csv.GetField<string>(0) == Type2)
                    {
                        Type2 = csv.GetField<string>(1).ToUpper();
                        Pokemon.Type2 = Type2;
                    }
                }
                if (Type2 == null || Type2 == "")
                {
                    Pokemon.Type2 = "NONE";
                }
                Console.Write("+");

                //Pokemon_Abilities
                CsvReader = File.OpenText(csvFiles[130]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == Entry && csv.GetField<int>(2) == 1)
                    {
                        HiddenAbility = csv.GetField<string>(1);
                    }
                    else if (csv.GetField<string>(0) == Entry && csv.GetField<string>(2) != "1")
                    {
                        Ability1 = csv.GetField<string>(1);
                        csv.Read();
                        if (csv.GetField<string>(0) == Entry && csv.GetField<string>(2) != "1")
                        {
                            Ability2 = csv.GetField<string>(1);
                        }
                        else if (csv.GetField<string>(0) == Entry && csv.GetField<string>(2) == "1")
                        {
                            HiddenAbility = csv.GetField<string>(1);
                        }
                        csv.Read();
                        if (csv.GetField<string>(0) != Entry)
                        {
                            Ability2 = "NONE";
                            Pokemon.Ability2 = Ability2;
                        }
                        else if (csv.GetField<string>(0) == Entry && csv.GetField<string>(2) == "1")
                        {
                            HiddenAbility = csv.GetField<string>(1);
                        }
                    }
                }
                Console.Write("+");

                //Abilities
                CsvReader = File.OpenText(csvFiles[0]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == Ability1)
                    {
                        Pokemon.Ability1 = "" + UpperCaseFirst(csv.GetField<string>(1)) + "";
                    }
                    else if (csv.GetField<string>(0) == Ability2 && Ability2 != "NONE")
                    {
                        Pokemon.Ability2 = "" + UpperCaseFirst(csv.GetField<string>(1)) + "";
                    }
                    else if (csv.GetField<string>(0) == HiddenAbility)
                    {
                        Pokemon.HiddenAbility = "" + UpperCaseFirst(csv.GetField<string>(1)) + "";
                    }
                }

                if (Pokemon.HiddenAbility == "" || Pokemon.HiddenAbility == null)
                {
                    Pokemon.HiddenAbility = "NONE";
                }
                Console.Write("+");

                //Pokemon_Species
                CsvReader = File.OpenText(csvFiles[149]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == Entry)
                    {
                        Pokemon.MaleRatio = (100.0 - (100.0 / 8.0 * csv.GetField<double>(8))).ToString() + "f";
                        Pokemon.CatchRate = csv.GetField<string>(9);
                        Pokemon.BaseFriendship = csv.GetField<string>(10);
                        Pokemon.HatchTime = (257 * csv.GetField<int>(12)).ToString();
                        GrowthRate = csv.GetField<string>(14);
                        Color_ID = csv.GetField<string>(5);
                        if (csv.GetField<string>(4) != "")
                        {
                            string test = csv.GetField<string>(4);
                            EvolutionID = csv.GetField<int>(4);
                        }
                    }
                }
                Console.Write("+");

                //Pokemon_colors
                CsvReader = File.OpenText(csvFiles[131]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == Color_ID)
                    {
                        Pokemon.PokedexColor = csv.GetField<string>(1).ToUpper();
                    }
                }
                Console.Write("+");

                //Growth_Rates
                CsvReader = File.OpenText(csvFiles[67]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == GrowthRate)
                    {
                        Pokemon.LevelingRate = csv.GetField<string>(1).ToUpper();
                        Pokemon.LevelingRate = Pokemon.LevelingRate.Replace("-", "");
                        Pokemon.LevelingRate = Pokemon.LevelingRate.Replace("SLOWTHENVERYFAST", "FLUCTUATING");
                        if (Pokemon.LevelingRate == "MEDIUM")
                        {
                            Pokemon.LevelingRate = "MEDIUMFAST";
                        }
                        Pokemon.LevelingRate = Pokemon.LevelingRate.Replace("FASTTHENVERYSLOW", "ERRATIC");
                    }
                }
                Console.Write("+");

                //Pokemon_Egg_Groups
                CsvReader = File.OpenText(csvFiles[134]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == Entry)
                    {
                        EggGroup1 = csv.GetField<string>(1);
                        csv.Read();
                        if (csv.GetField<string>(0) == Entry)
                        {
                            EggGroup2 = csv.GetField<string>(1);
                        }
                        else
                        {
                            EggGroup2 = "NONE";
                            Pokemon.EggGroup2 = EggGroup2;
                        }
                    }
                }
                Console.Write("+");

                //Egg_groups
                CsvReader = File.OpenText(csvFiles[49]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == EggGroup1)
                    {
                        if (csv.GetField<string>(1) == "ground")
                        {
                            Pokemon.EggGroup1 = "FIELD";
                        }
                        else if (csv.GetField<string>(1) == "plant")
                        {
                            Pokemon.EggGroup1 = "GRASS";
                        }
                        else if (csv.GetField<string>(1) == "humanshape")
                        {
                            Pokemon.EggGroup1 = "HUMANLIKE";
                        }
                        else if (csv.GetField<string>(1) == "indeterminate")
                        {
                            Pokemon.EggGroup1 = "AMORPHOUS";
                        }
                        else if (csv.GetField<string>(1) == "no-eggs")
                        {
                            Pokemon.EggGroup1 = "UNDISCOVERED";
                        }
                        else
                        {
                            Pokemon.EggGroup1 = csv.GetField<string>(1).ToUpper();
                        }
                    }
                    else if (csv.GetField<string>(0) == EggGroup2 && EggGroup2 != "NONE")
                    {
                        if (csv.GetField<string>(1) == "ground")
                        {
                            Pokemon.EggGroup2 = "FIELD";
                        }
                        else if (csv.GetField<string>(1) == "plant")
                        {
                            Pokemon.EggGroup2 = "GRASS";
                        }
                        else if (csv.GetField<string>(1) == "humanshape")
                        {
                            Pokemon.EggGroup2 = "HUMANLIKE";
                        }
                        else if (csv.GetField<string>(1) == "indeterminate")
                        {
                            Pokemon.EggGroup2 = "AMORPHOUS";
                        }
                        else if (csv.GetField<string>(1) == "no-eggs")
                        {
                            Pokemon.EggGroup2 = "UNDISCOVERED";
                        }
                        else
                        {
                            Pokemon.EggGroup2 = csv.GetField<string>(1).ToUpper();
                        }
                    }
                }
                Pokemon.EggGroup1 = Pokemon.EggGroup1.Replace("-", "");
                if (EggGroup2 != null)
                {
                    Pokemon.EggGroup2 = Pokemon.EggGroup2.Replace("-", "");
                }
                else
                {
                    Pokemon.EggGroup2 = "NONE";
                }
                Console.Write("+");

                //Pokemon_Stats
                CsvReader = File.OpenText(csvFiles[154]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == Entry && csv.GetField<string>(1) == "1")
                    {
                        Pokemon.BSHP = csv.GetField<string>(2);
                        Pokemon.EYHP = csv.GetField<string>(3);
                        csv.Read();
                        if (csv.GetField<string>(0) == Entry && csv.GetField<string>(1) == "2")
                        {
                            Pokemon.BSATK = csv.GetField<string>(2);
                            Pokemon.EYATK = csv.GetField<string>(3);
                        }
                        csv.Read();
                        if (csv.GetField<string>(0) == Entry && csv.GetField<string>(1) == "3")
                        {
                            Pokemon.BSDEF = csv.GetField<string>(2);
                            Pokemon.EYDEF = csv.GetField<string>(3);
                        }
                        csv.Read();
                        if (csv.GetField<string>(0) == Entry && csv.GetField<string>(1) == "4")
                        {
                            Pokemon.BSSPA = csv.GetField<string>(2);
                            Pokemon.EYSPA = csv.GetField<string>(3);
                        }
                        csv.Read();
                        if (csv.GetField<string>(0) == Entry && csv.GetField<string>(1) == "5")
                        {
                            Pokemon.BSSPD = csv.GetField<string>(2);
                            Pokemon.EYSPD = csv.GetField<string>(3);
                        }
                        csv.Read();
                        if (csv.GetField<string>(0) == Entry && csv.GetField<string>(1) == "6")
                        {
                            Pokemon.BSSPE = csv.GetField<string>(2);
                            Pokemon.EYSPE = csv.GetField<string>(3);
                        }
                    }
                }
                Console.Write("+");

                //Pokemon_Species_Names
                CsvReader = File.OpenText(csvFiles[152]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == Entry && csv.GetField<string>(1) == "9")
                    {
                        string Species = csv.GetField<string>(3);
                        Species = Species.Replace(" PokÃ©mon", "");
                        Species = Species.Replace(" Pokémon", "");
                        Pokemon.Species = Species;
                    }
                }
                Console.Write("+");

                //Pokemon_Species_Flavor_Text
                CsvReader = File.OpenText(csvFiles[151]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == Entry && csv.GetField<string>(1) == "23" && csv.GetField<string>(2) == "9")
                    {
                        string PokedexEntry = csv.GetField<string>(3);
                        PokedexEntry = PokedexEntry.Replace("\n", "");
                        PokedexEntry = PokedexEntry.Replace("\r", "");
                        PokedexEntry = PokedexEntry.Replace(".", ". ");
                        PokedexEntry = PokedexEntry.Replace("  ", " ");
                        PokedexEntry = PokedexEntry.Replace("   ", " ");
                        PokedexEntry = PokedexEntry.Remove(PokedexEntry.Length - 1);
                        Pokemon.Description = PokedexEntry;
                    }
                }
                Console.Write("+");

                //Pokemon_Moves
                List<string> Moves = new List<string>();
                List<int> Level = new List<int>();
                CsvReader = File.OpenText(csvFiles[144]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    if (csv.GetField<string>(0) == Entry && csv.GetField<string>(1) == Generation && csv.GetField<string>(3) == "1" && csv.GetField<string>(4) != "")
                    {
                        Level.Add(csv.GetField<int>(4));
                        Moves.Add(csv.GetField<string>(2));
                    }
                    else if (csv.GetField<string>(0) == Entry && csv.GetField<string>(1) == Generation && csv.GetField<string>(3) == "4")
                    {
                        TMList.Add(csv.GetField<string>(2));
                    }
                }
                string[] TMArray = TMList.ToArray();
                Console.Write("+");

                //Move_Names
                string TM = "";
                string LevelString = "";

                CsvReader = File.OpenText(csvFiles[114]);
                csv = new CsvReader(CsvReader);
                while (csv.Read())
                {
                    for (int i = 0; i < TMArray.Length; i++)
                    {
                        if (csv.GetField<string>(0) == TMArray[i] && csv.GetField<string>(1) == "9")
                        {
                            string TMMove = csv.GetField<string>(2);
                            TMMove = TMMove.Replace(' ', '_');
                            TMMove = TMMove.Replace('-', '_');
                            TM = TM +
                            $"\tnew PokemonMoveset" +
                            $"\n\t(" +
                                $"\tmoveId: Moves.{TMMove}," +
                                $"\n\t\tmethod: LearnMethod.machine," +
                            $"\n\t),\n";
                        }
                    }
                    for (int i = 0; i < Moves.Count; i++)
                    {
                        if (csv.GetField<string>(0) == Moves[i] && csv.GetField<string>(1) == "9")
                        {
                            Moves[i] = csv.GetField<string>(2);
                        }
                    }
                }
                for (int i = 0; i < Moves.Count; i++)
                {
                    try
                    {
                        MoveLevelDictionary.Add(Moves[i], Level[i]);
                    }
                    catch
                    {
                        i++;
                    }
                }

                var items = from KeyValuePair in MoveLevelDictionary orderby KeyValuePair.Value ascending select KeyValuePair;

                foreach (KeyValuePair<string, int> pair in items)
                {
                    string moveId = pair.Key.Replace(' ', '_');
                    moveId = moveId.Replace('-', '_');
                    LevelString = LevelString +
                        (
                        $"\tnew PokemonMoveset" +
                        $"\n\t(" +
                            $"\tmoveId: Moves.{moveId}," +
                            $"\n\t\tmethod: LearnMethod.levelup," +
                            $"\n\t\tlevel: {pair.Value}" +
                        $"\n\t),\n");
                }
                if (TM != "")
                {
                    TM = TM.Remove(TM.Length - 1);
                }
                TM = TM.Replace("\n", System.Environment.NewLine);
                LevelString = LevelString.Replace("\n", System.Environment.NewLine);
                LevelString = LevelString.Remove(LevelString.Length - 1);
                Pokemon.LevelMoves = LevelString;
                Pokemon.TMMoves = TM;
                Console.Write("+");

                //Adding the Pokemon to the array
                Pokemons[PokemonCounter - 1] = Pokemon;

                //Counters
                PokemonCounter++;
                RegionDex++;
                Console.WriteLine("+|");
            }
            Console.WriteLine("Done with basics, adding evolutions..");
            int progress = Convert.ToInt32(Generation) / 10;
            for (int i = 0; i < Pokemons.Length; i++)
            {
                EvolutionMethod(i + 1, csvFiles, Pokemons[i].NAME, Pokemons[i], Generation, Pokemons);
            }

            using (Output = File.AppendText(SourcePath + @"\POKEMONOUTPUT " + Generations[displayItem] + ".txt"))
            {
                foreach(Pokemon poke in Pokemons)
                {
                    string OutputText = poke.ToString();
                    OutputText = OutputText.Replace("\n", System.Environment.NewLine);
                    Output.WriteLine(OutputText);
                    Output.Write("\n");
                }
            }
            
            Console.WriteLine($"Database is done writing, your file can be found in {SourcePath}/POKEMONOUTPUT.TXT");
            Console.ReadKey();
        }

        //Method to make the first letter of a string uppercase
        static string UpperCaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        //Method for evolution
        public static void EvolutionMethod(int counter, string[] csvFiles, string name, Pokemon pokemon, string gen, Pokemon[] pokemons)
        {
            string MethodCode = "";
            string Item = "";
            TextReader CsvReader = File.OpenText(csvFiles[135]);
            CsvReader csv = new CsvReader(CsvReader);
            while (csv.Read())
            {
                string test = csv.GetField<string>(1);
                if(csv.GetField<string>(1) == (counter).ToString())
                {
                    #region Happiness Evolution
                    if(csv.GetField<string>(8) == "night" && csv.GetField<string>(11) != "")
                    {
                        MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.HappinessNight),";
                    }
                    else if(csv.GetField<string>(8) == "day" && csv.GetField<string>(11) != "")
                    {
                        MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.HappinessDay),";
                    }
                    else if(csv.GetField<string>(11) != "")
                    {
                        MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Happiness),";
                    }
                    #endregion

                    #region Item Evolution
                    if (csv.GetField<string>(2) == "3" && csv.GetField<string>(5) == "1" && csv.GetField<string>(3) != "")
                    {
                        Item = csv.GetField<string>(3);
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if(csv2.GetField<string>(0) == Item)
                            {
                                string tempItem = csv2.GetField<string>(1);
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.ItemFemale, {tempItem}),";
                                break;
                            }
                        }
                    }
                    else if (csv.GetField<string>(2) == "3" && csv.GetField<string>(5) == "2" && csv.GetField<string>(3) != "")
                    {
                        Item = csv.GetField<string>(3);
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.GetField<string>(0) == Item)
                            {
                                string tempItem = csv2.GetField<string>(1);
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.ItemMale, {tempItem}),";
                                break;
                            }
                        }
                    }
                    else if (csv.GetField<string>(2) == "3" && csv.GetField<string>(3) != "")
                    {
                        Item = csv.GetField<string>(3);
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.GetField<string>(0) == Item)
                            {
                                string tempItem = csv2.GetField<string>(1);
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Item, {tempItem}),";
                                break;
                            }
                        }
                    }
                    #endregion

                    #region Location Evolution
                    if(csv.GetField<string>(6) != "")
                    {
                        string LocationId = csv.GetField<string>(6);
                        TextReader CsvReader2 = File.OpenText(csvFiles[91]);
                        CsvReader csv2 = new CsvReader(CsvReader);
                        while (csv.Read())
                        {
                            if(csv.GetField<string>(0) == LocationId)
                            {
                                MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Location, //location: map: {csv.GetField<string>(1)}\n),";
                                break;
                            }
                        }
                    }
                    #endregion

                    #region Trade Evolution
                    if(csv.GetField<string>(17) != "" && csv.GetField<string>(2) == "2")
                    {
                        string EvolutionSpecies = csv.GetField<string>(17);
                        TextReader CsvReader2 = File.OpenText(csvFiles[129]);
                        var csv2 = new CsvReader(CsvReader2);

                        while (csv.Read())
                        {
                            if (csv.GetField<string>(0) == EvolutionSpecies)
                            {
                                MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.TradeSpecies, {csv.GetField<string>(1).ToUpper()}),";
                                break;
                            }
                        }
                    }
                    else if(csv.GetField<string>(2) == "2" && csv.GetField<string>(7) != "" && csv.GetField<string>(17) == "")
                    {
                        Item = csv.GetField<string>(7);
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.GetField<string>(0) == Item)
                            {
                                string tempItem = csv2.GetField<string>(1);
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.TradeItem, {tempItem}),";
                                break;
                            }
                        }
                    }
                    else if(csv.GetField<string>(2) == "2" && csv.GetField<string>(7) == "" && csv.GetField<string>(17) == "")
                    {
                        MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Trade) ";
                    }
                    #endregion

                    #region Hold Item
                    if(csv.GetField<string>(2) == "1" && csv.GetField<string>(7) != "" && csv.GetField<string>(8) == "day")
                    {
                        Item = csv.GetField<string>(7);
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.GetField<string>(0) == Item)
                            {
                                string tempItem = csv2.GetField<string>(1);
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.HoldItemDay, {tempItem}),";
                                break;
                            }
                        }
                    }
                    else if(csv.GetField<string>(2) == "1" && csv.GetField<string>(7) != "" && csv.GetField<string>(8) == "night")
                    {
                        Item = csv.GetField<string>(7);
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.GetField<string>(0) == Item)
                            {
                                string tempItem = csv2.GetField<string>(1);
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.HoldItemNight, {tempItem}),";
                                break;
                            }
                        }
                    }
                    else if(csv.GetField<string>(2) == "1" && csv.GetField<string>(7) != "")
                    {
                        Item = csv.GetField<string>(7);
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.GetField<string>(0) == Item)
                            {
                                string tempItem = csv2.GetField<string>(1);
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.HoldItem, {tempItem}),";
                                break;
                            }
                        }
                    }
                    #endregion

                    #region Beauty
                    if(csv.GetField<string>(2) == "1" && csv.GetField<string>(12) != "")
                    {
                        MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Beauty, {csv.GetField<string>(12)}),";
                    }
                    #endregion

                    #region Move
                    if (csv.GetField<string>(2) == "1" && csv.GetField<string>(9) != "" && csv.GetField<string>(15) == "")
                    {
                        string tempMove = csv.GetField<string>(9);
                        TextReader CsvReader2 = File.OpenText(csvFiles[114]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.GetField<string>(0) == tempMove && csv.GetField<string>(1) == gen)
                            {
                                tempMove = csv2.GetField<string>(2);
                                tempMove = tempMove.Replace(' ', '_');
                                tempMove = tempMove.Replace('-', '_');
                                MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Move, {tempMove}),";
                                break;
                            }
                        }
                    }
                    else if(csv.GetField<string>(2) == "1" && csv.GetField<string>(10) != "")
                    {
                        string tempType = csv.GetField<string>(10);
                        TextReader CsvReader2 = File.OpenText(csvFiles[114]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.GetField<string>(0) == tempType && csv2.GetField<string>(1) == gen)
                            {
                                tempType = csv2.GetField<string>(1);
                                tempType = tempType.Replace(' ', '_');
                                tempType = tempType.Replace('-', '_');
                                MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Move, {tempType}),";
                                break;
                            }
                        }
                    }
                    #endregion

                    #region Party
                    if(csv.GetField<string>(2) == "1" && csv.GetField<string>(15) != "")
                    {
                        string EvolutionSpecies = csv.GetField<string>(15);
                        TextReader CsvReader2 = File.OpenText(csvFiles[129]);
                        var csv2 = new CsvReader(CsvReader2);

                        while (csv2.Read())
                        {
                            if (csv2.GetField<string>(0) == EvolutionSpecies)
                            {
                                MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Party, {csv.GetField<string>(1).ToUpper()}),";
                                break;
                            }
                        }
                    }
                    #endregion

                    #region Attack > Defense > Equal =
                    if(csv.GetField<string>(2) == "1" && csv.GetField<string>(14) == "1")           //Attack Greater Than Defense (Attack > Defense)    1
                    {
                        MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.AttackGreater),";
                    }
                    else if (csv.GetField<string>(2) == "1" && csv.GetField<string>(14) == "-1")    //Defense Greater Than Attack (Attack < Defense)    -1
                    {
                        MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.DefenseGreater),";
                    }
                    else if (csv.GetField<string>(2) == "1" && csv.GetField<string>(14) == "-1")    //Attack Equal To Attack (Attack = Defense)         0
                    {
                        MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.AtkDefEqual),";
                    }
                    #endregion

                    #region Silcoon
                    if(csv.GetField<string>(1) == "266" && csv.GetField<string>(2) == "1")
                    {
                        MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Silcoon),";
                    }
                    #endregion

                    #region Cascoon
                    if(csv.GetField<string>(1) == "268" && csv.GetField<string>(2) == "1")
                    {
                        MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Cascoon),";
                    }
                    #endregion

                    #region Ninjask
                    if(csv.GetField<string>(1) == "291" && csv.GetField<string>(2) == "1")
                    {
                        MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Ninjask),";
                    }
                    #endregion

                    #region Shedinja
                    if(csv.GetField<string>(1) == "292" && csv.GetField<string>(2) == "1")
                    {
                        MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Shedinja),";
                    }
                    #endregion

                    #region Level
                    if(csv.GetField<string>(2) == "1" && csv.GetField<string>(3) == "" && csv.GetField<string>(4) != "" && csv.GetField<string>(11) == "" && csv.GetField<string>(19) == "0")
                    {
                        if(csv.GetField<string>(5) == "1")
                        {
                            MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.LevelFemale, {csv.GetField<string>(4)}),";
                        }
                        else if(csv.GetField<string>(5) == "2")
                        {
                            MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.LevelMale, {csv.GetField<string>(4)}),";
                        }
                        else
                        {
                            MethodCode = MethodCode + $"new PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Level, {csv.GetField<string>(4)}),";
                        }
                    }
                    #endregion
                    break;
                }
            }
            CsvReader = File.OpenText(csvFiles[149]);
            csv = new CsvReader(CsvReader);
            while (csv.Read())
            {
                if (csv.GetField<string>(0) == counter.ToString() && csv.GetField<string>(3) != "")
                {
                    if (csv.GetField<int>(3) < pokemons.Length)
                    {
                        string output;
                        if (MethodCode != "")
                        {
                            MethodCode = MethodCode.Remove(MethodCode.Length - 1);
                        }
                        output = MethodCode;
                        pokemons[csv.GetField<int>(3) - 1].PokemonEvolution = pokemons[csv.GetField<int>(3) - 1].PokemonEvolution + output;
                        break;
                    }
                }
            }
        }

        /*
         * 1 female
         * 2 male
         * 3 genderless
        */
        public enum evolutionMethod
        {
            //Level,
            //LevelMale,
            //LevelFemale,
            //Item,
            //ItemMale,
            //ItemFemale,
            //Trade,
            //TradeItem,
            //TradeSpecies,
            //Happiness,
            //HappinessDay,
            //HappinessNight,
            //Hatred,
            //HoldItem,
            //HoldItemDay,
            //HoldItemNight,
            //Beauty,
            //Move,
            //Party,
            //Type,
            //Location,
            //Weather,
            //AttackGreater,  //1
            //DefenseGreater, //-1
            //AtkDefEqual,    //0
            //Silcoon,
            //Cascoon,
            //Ninjask,
            //Shedinja,
        }
    }

    /*
     * Okay so we have the Species Id in evolution
     * We take that value (wich is the entry number)
     * we got into Pokemon Species and take the evolved from value
     * Add the method to the Pokemon and at the end override the ToString method
    */
}
