﻿using System;
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

namespace PokemonUnity.Editor
{
    class Program
    {
		#region CSV Variables
		static string SourcePath;
		static string[] csvFiles;
		static TextReader CsvReader;
		static CsvReader csv;
		#endregion	

		#region Pokemon Variables
		static string RegionalDex { get; set; }     //Done
		static string ID { get; set; }              //Done
		static string NAME { get; set; }            //Done
		static string Type1 { get; set; }           //Done
		static string Type2 { get; set; }           //Done
		static string Ability1 { get; set; }        //Done
		static string Ability2 { get; set; }        //Done
		static string HiddenAbility { get; set; }   //Done
		static string MaleRatio { get; set; }       //Done
		static string CatchRate { get; set; }       //Done
		static string EggGroup1 { get; set; }       //Done
		static string EggGroup2 { get; set; }       //Done
		static string HatchTime { get; set; }       //Done
		static string Height { get; set; }          //Done
		static string Weight { get; set; }          //Done
		static string EXPYield { get; set; }        //Done
		static string LevelingRate { get; set; }    //Done
		static string EYHP { get; set; }            //Done
		static string EYATK { get; set; }           //Done
		static string EYDEF { get; set; }           //Done
		static string EYSPA { get; set; }           //Done
		static string EYSPD { get; set; }           //Done
		static string EYSPE { get; set; }           //Done
		static string PokedexColor { get; set; }    //Done
		static string BaseFriendship { get; set; }  //Done
		static string Species { get; set; }         //Done
		static string Description { get; set; }     //Done
		static string BSHP { get; set; }            //Done
		static string BSATK { get; set; }           //Done
		static string BSDEF { get; set; }           //Done
		static string BSSPA { get; set; }           //Done
		static string BSSPD { get; set; }           //Done
		static string BSSPE { get; set; }           //Done
		static string Luminance { get; set; }       //not needed
		static string LightColor { get; set; }      //Done
		static string Moves { get; set; }           //Replaced with one string, easier this way
		static string PokemonEvolution { get; set; }//Done
		static string LevelEvolution { get; set; }  //Done
		static string Forms { get; set; }           //manually
		static string HeldItem { get; set; }        //manually
		static bool IsBaseForm { get; set; }		//
		static bool IsBaby { get; set; }			//
		static bool FormSwitchable { get; set; }	//
		static bool HasGenderDiff { get; set; }		//
		static int EvolutionID { get; set; }		//
		static string GenerationId { get; set; }	//
		static string HabitatId { get; set; }		//
		static string ShapeId { get; set; }			//
		static string Color_ID { get; set; }		//
		static string GenderEnum { get; set; }		//
		static string GrowthRate { get; set; }		//
		#endregion

		#region Misc Variables for Loop
		static string Generation;
		static string Entry;
		static string[] Generations = new string[]
        {
            "Blue and Red", "Yellow", "Gold and Silver", "Crystal", "Ruby and Sapphire", "Emerald", "FireRed and LeafGreen", "Diamond and Pearl", "Platinum", "Heartgold and Soulsilver", "Black and White", "Black 2 and White 2", "X and Y", "Omega Ruby and Alpha Sapphire", "Sun and Moon", "Ultra Sun and Ultra Moon", "All/Everything"
        };
        static int displayItem;
		#endregion

		static void Main(string[] args)
        {
            Console.Title = "Veekun's CSV Database to Pokemon Unity's PokemonDatabase Converter ~ by Velorexe";
            Console.WriteAscii("VEEKUN TO PKUNITY", System.Drawing.Color.FromArgb(66, 167, 199));
            Console.WriteLine("This tool is created by Velorexe for the Pokemon Unity project to easily convert the Veekun Pokemon Database to the format that is used in Pokemon Unity");
            Console.WriteLine("Please fill in the source path to the CSV Pokemon Database from Veekun. This should be a direct path to the directory.\nExample: C:/Users/Velorexe/Desktop/PokemonSprites/PokemonDatabase/Veekun Database/CSV\n");
            SourcePath = Console.ReadLine();
            csvFiles = Directory.GetFiles(SourcePath);
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
            Console.WriteLine("Which generation would you like to convert? Please press a key to load the generations. (Every key except 'Enter' please I haven't optimised the code yet)");
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
                Console.WriteLine("Which generation would you like to convert?");
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
                        Console.ForegroundColor = System.Drawing.Color.Blue;
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
            displayItem = SelectedItem;
            SelectedItem++;
            if (SelectedItem > 11)
            {
                SelectedItem = SelectedItem + 2;
            }
            Console.WriteLine("Converting now...\n");

            int PokemonCounter = 1;
            int MaxPoke = 0;
            Generation = SelectedItem.ToString();

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
                default:
                    //ToDo: get # of rows in csv
                    //MaxPoke = csv.Length;
                    break;
            }
            #endregion

            int RegionDex = 1;
            Pokemon[] pokemons = new Pokemon[MaxPoke];

            while (PokemonCounter < MaxPoke + 1)
            {
                Entry = PokemonCounter.ToString();

                Pokemon pokemon = new Pokemon();

                //ToDo: ForEach on RegionDex -> ToArray(); Use data from CSV
                //if (RegionDex == Gen1 || RegionDex == Gen2 || RegionDex == Gen3 || RegionDex == Gen4 || RegionDex == Gen5 || RegionDex == Gen6 || RegionDex == Gen7)
                //{
                //    RegionDex = 1;
                //}

                //LightColor = "clear";
                //Luminance = "0";

                //Pokemon
                csv_Pokemon();

				//Pokemon_Types
				csv_Pokemon_Types();

				//Types
				csv_Types();

				//Pokemon_Abilities
				csv_Pokemon_Abilities();

				//Abilities
				csv_Abilities();

				//Pokemon_Species
				csv_Pokemon_Species();

				//Pokemon_habitats
				//csv_Pokemon_habitats();

				//Pokemon_colors
				csv_Pokemon_colors();

				//Growth_Rates
				csv_Growth_Rates();

				//Pokemon_Egg_Groups
				csv_Pokemon_Egg_Groups();

				//Egg_groups
				csv_Egg_groups();

				//Pokemon_Stats
				csv_Pokemon_Stats();

				//Pokemon_Species_Names
				csv_Pokemon_Species_Names();

				//Pokemon_Species_Flavor_Text
				csv_Pokemon_Species_Flavor_Text();

				//Pokemon_Moves
				csv_Pokemon_Moves();

				//Move_Names
				//csv_Move_Names();

				//Pokemon_Items
				csv_Pokemon_Items();

				//Item_Names
				//csv_Item_Names();

                //Adding the Pokemon to the array
                pokemons[PokemonCounter - 1] = pokemon;

                //Counters
                PokemonCounter++;
                RegionDex++;
                Console.WriteLine("+|");
            }
            Console.WriteLine("Done with basics, adding evolutions..");
            int progress = Convert.ToInt32(Generation) / 10;
            for (int i = 0; i < pokemons.Length; i++)
            {
				EvolutionMethod(i + 1, csvFiles, pokemons[i].ID.ToString(), pokemons[i], Generation, pokemons);
            }

			OutputCsharp(pokemons);
			OutputXML(pokemons);
            
            Console.WriteLine($"Database is done writing, your file can be found in {SourcePath}/POKEMONOUTPUT.TXT");
            Console.ReadKey();
        }

		/// <summary>
		/// Method to make the first letter of a string uppercase
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
        static string UpperCaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

		/// <summary>
		/// Method for evolution
		/// </summary>
		/// <param name="counter"></param>
		/// <param name="csvFiles"></param>
		/// <param name="name"></param>
		/// <param name="pokemon"></param>
		/// <param name="gen"></param>
		/// <param name="pokemons"></param>
        public static void EvolutionMethod(int counter, string[] csvFiles, string name, Pokemon pokemon, string gen, Pokemon[] pokemons)
        {
            string MethodCode = "";
            string Item = "";
            CsvReader = File.OpenText(csvFiles[135]);
            csv = new CsvReader(CsvReader);
            while (csv.Read())
            {
                if(csv.Context.Record[1] == (counter).ToString())
                {
                    #region Happiness Evolution
                    if(csv.Context.Record[8] == "night" && !string.IsNullOrEmpty(csv.Context.Record[11]))
                    {
                        MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.HappinessNight, {csv.Context.Record[11]}),";
                    }
                    else if(csv.Context.Record[8] == "day" && !string.IsNullOrEmpty(csv.Context.Record[11]))
                    {
                        MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.HappinessDay, {csv.Context.Record[11]}),";
                    }
                    else if(!string.IsNullOrEmpty(csv.Context.Record[11]))
                    {
                        MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.Happiness, {csv.Context.Record[11]}),";
                    }
                    #endregion

                    #region Item Evolution
                    if (csv.Context.Record[2] == "3" && csv.Context.Record[5] == "1" && !string.IsNullOrEmpty(csv.Context.Record[3]))
                    {
                        Item = csv.Context.Record[3];
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if(csv2.Context.Record[0] == Item)
                            {
                                string tempItem = csv2.Context.Record[1];
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Items>(Pokemons.{name.ToUpper()}, EvolutionMethod.ItemFemale, Items.{tempItem.ToUpper()}),";
                                break;
                            }
                        }
                    }
                    else if (csv.Context.Record[2] == "3" && csv.Context.Record[5] == "2" && !string.IsNullOrEmpty(csv.Context.Record[3]))
                    {
                        Item = csv.Context.Record[3];
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.Context.Record[0] == Item)
                            {
                                string tempItem = csv2.Context.Record[1];
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Items>(Pokemons.{name.ToUpper()}, EvolutionMethod.ItemMale, Items.{tempItem.ToUpper()}),";
                                break;
                            }
                        }
                    }
                    else if (csv.Context.Record[2] == "3" && !string.IsNullOrEmpty(csv.Context.Record[3]))
                    {
                        Item = csv.Context.Record[3];
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.Context.Record[0] == Item)
                            {
                                string tempItem = csv2.Context.Record[1];
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Items>(Pokemons.{name.ToUpper()}, EvolutionMethod.Item, Items.{tempItem.ToUpper()}),";
                                break;
                            }
                        }
                    }
                    #endregion

                    #region Location Evolution
                    if(!string.IsNullOrEmpty(csv.Context.Record[6]))
                    {
                        string LocationId = csv.Context.Record[6];
                        TextReader CsvReader2 = File.OpenText(csvFiles[91]);
                        CsvReader csv2 = new CsvReader(CsvReader);
                        while (csv2.Read())
                        {
                            if(csv2.Context.Record[0] == LocationId)
                            {
                                MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Location, /*location: map: {csv.Context.Record[1]}*/),";
                                break;
                            }
                        }
                    }
                    #endregion

                    #region Trade Evolution
                    //string test2 = csv.Context.Record[17];
                    //string test = csv.Context.Record[2];
                    if(!string.IsNullOrEmpty(csv.Context.Record[17]) && csv.Context.Record[2] == "2")
                    {
                        string EvolutionSpecies = csv.Context.Record[17];
                        TextReader CsvReader2 = File.OpenText(csvFiles[129]);
                        var csv2 = new CsvReader(CsvReader2);

                        while (csv2.Read())
                        {
                            if (csv2.Context.Record[0] == EvolutionSpecies)
                            {
                                MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Pokemons>(Pokemons.{name.ToUpper()}, EvolutionMethod.TradeSpecies, Pokemons.{csv.Context.Record[1].ToUpper()}),";
                                break;
                            }
                        }
                    }
                    else if(csv.Context.Record[2] == "2" && !string.IsNullOrEmpty(csv.Context.Record[7]) && csv.Context.Record[17] == "")
                    {
                        Item = csv.Context.Record[7];
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.Context.Record[0] == Item)
                            {
                                string tempItem = csv2.Context.Record[1];
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Items>(Pokemons.{name.ToUpper()}, EvolutionMethod.TradeItem, Items.{tempItem.ToUpper()}),";
                                break;
                            }
                        }
                    }
                    else if(csv.Context.Record[2] == "2" && csv.Context.Record[7] == "" && csv.Context.Record[17] == "")
                    {
                        MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Trade), ";
                    }
                    #endregion

                    #region Hold Item
                    if(csv.Context.Record[2] == "1" && !string.IsNullOrEmpty(csv.Context.Record[7]) && csv.Context.Record[8] == "day")
                    {
                        Item = csv.Context.Record[7];
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.Context.Record[0] == Item)
                            {
                                string tempItem = csv2.Context.Record[1];
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Items>(Pokemons.{name.ToUpper()}, EvolutionMethod.HoldItemDay, Items.{tempItem.ToUpper()}),";
                                break;
                            }
                        }
                    }
                    else if(csv.Context.Record[2] == "1" && !string.IsNullOrEmpty(csv.Context.Record[7]) && csv.Context.Record[8] == "night")
                    {
                        Item = csv.Context.Record[7];
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.Context.Record[0] == Item)
                            {
                                string tempItem = csv2.Context.Record[1];
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Items>(Pokemons.{name.ToUpper()}, EvolutionMethod.HoldItemNight, Items.{tempItem.ToUpper()}),";
                                break;
                            }
                        }
                    }
                    else if(csv.Context.Record[2] == "1" && !string.IsNullOrEmpty(csv.Context.Record[7]))
                    {
                        Item = csv.Context.Record[7];
                        TextReader CsvReader2 = File.OpenText(csvFiles[69]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.Context.Record[0] == Item)
                            {
                                string tempItem = csv2.Context.Record[1];
                                tempItem = tempItem.Replace('-', '_');
                                tempItem = tempItem.Replace(' ', '_');
                                MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Items>(Pokemons.{name.ToUpper()}, EvolutionMethod.HoldItem, Items.{tempItem.ToUpper()}),";
                                break;
                            }
                        }
                    }
                    #endregion

                    #region Beauty
                    if(csv.Context.Record[2] == "1" && !string.IsNullOrEmpty(csv.Context.Record[12]))
                    {
                        MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.Beauty, {csv.Context.Record[12]}),";
                    }
                    #endregion

                    #region Move
                    if (csv.Context.Record[2] == "1" && !string.IsNullOrEmpty(csv.Context.Record[9]) && csv.Context.Record[15] == "")
                    {
                        string tempMove = csv.Context.Record[9];
                        TextReader CsvReader2 = File.OpenText(csvFiles[114]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.Context.Record[0] == tempMove && csv.Context.Record[1] == gen)
                            {
                                tempMove = csv2.Context.Record[2];
                                tempMove = tempMove.Replace(' ', '_');
                                tempMove = tempMove.Replace('-', '_');
                                MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Moves>(Pokemons.{name.ToUpper()}, EvolutionMethod.Move, Moves.{tempMove.ToUpper()}),";
                                break;
                            }
                        }
                    }
                    else if(csv.Context.Record[2] == "1" && !string.IsNullOrEmpty(csv.Context.Record[10]))
                    {
                        string tempType = csv.Context.Record[10];
                        TextReader CsvReader2 = File.OpenText(csvFiles[114]);
                        CsvReader csv2 = new CsvReader(CsvReader2);
                        while (csv2.Read())
                        {
                            if (csv2.Context.Record[0] == tempType && csv2.Context.Record[1] == gen)
                            {
                                tempType = csv2.Context.Record[1];
                                tempType = tempType.Replace(' ', '_');
                                tempType = tempType.Replace('-', '_');
                                MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Types>(Pokemons.{name.ToUpper()}, EvolutionMethod.Move, Types.{tempType.ToUpper()}),";
                                break;
                            }
                        }
                    }
                    #endregion

                    #region Party
                    if(csv.Context.Record[2] == "1" && !string.IsNullOrEmpty(csv.Context.Record[15]))
                    {
                        string EvolutionSpecies = csv.Context.Record[15];
                        TextReader CsvReader2 = File.OpenText(csvFiles[129]);
                        var csv2 = new CsvReader(CsvReader2);

                        while (csv2.Read())
                        {
                            if (csv2.Context.Record[0] == EvolutionSpecies)
                            {
                                MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Pokemons>(Pokemons.{name.ToUpper()}, EvolutionMethod.Party, Pokemons.{csv.Context.Record[1].ToUpper()}),";
                                break;
                            }
                        }
                    }
                    #endregion

                    #region Attack > Defense > Equal =
                    if(csv.Context.Record[2] == "1" && csv.Context.Record[14] == "1")           //Attack Greater Than Defense (Attack > Defense)    1
                    {
                        MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.AttackGreater),";
                    }
                    else if (csv.Context.Record[2] == "1" && csv.Context.Record[14] == "-1")    //Defense Greater Than Attack (Attack < Defense)    -1
                    {
                        MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.DefenseGreater),";
                    }
                    else if (csv.Context.Record[2] == "1" && csv.Context.Record[14] == "0")    //Attack Equal To Attack (Attack = Defense)         0
                    {
                        MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.AtkDefEqual),";
                    }
                    #endregion

                    #region Silcoon
                    if(csv.Context.Record[1] == "266" && csv.Context.Record[2] == "1")
                    {
                        MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Silcoon),";
                    }
                    #endregion

                    #region Cascoon
                    if(csv.Context.Record[1] == "268" && csv.Context.Record[2] == "1")
                    {
                        MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Cascoon),";
                    }
                    #endregion

                    #region Ninjask
                    if(csv.Context.Record[1] == "291" && csv.Context.Record[2] == "1")
                    {
                        MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Ninjask),";
                    }
                    #endregion

                    #region Shedinja
                    if(csv.Context.Record[1] == "292" && csv.Context.Record[2] == "1")
                    {
                        MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Shedinja),";
                    }
                    #endregion

                    #region Level
                    if(csv.Context.Record[2] == "1" && csv.Context.Record[3] == "" && !string.IsNullOrEmpty(csv.Context.Record[4]) && csv.Context.Record[11] == "" && csv.Context.Record[19] == "0")
                    {
                        if(csv.Context.Record[5] == "1")
                        {
                            MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.LevelFemale, {csv.Context.Record[4]}),";
                        }
                        else if(csv.Context.Record[5] == "2")
                        {
                            MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.LevelMale, {csv.Context.Record[4]}),";
                        }
                        else
                        {
                            MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.Level, {csv.Context.Record[4]}),";
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
                if (csv.Context.Record[1] == counter.ToString() && !string.IsNullOrEmpty(csv.Context.Record[3]))
                {
                    if (int.Parse(csv.Context.Record[3]) < pokemons.Length)
                    {
                        string output;
                        /*if (MethodCode != "")
                        {
                            MethodCode = MethodCode.Remove(MethodCode.Length - 1);
                        }*/
                        //Cleaner to remove only what you need it to; just in case a problem occurs, it's prevented and prepared.
                        output = MethodCode.Replace('-', '_').Trim(new char[] { ',', ' ', '\r' });//.Replace("\n", System.Environment.NewLine);
                        //ToDo: Fix Evolution...
						//pokemons[int.Parse(csv.Context.Record[3]) - 1].PokemonEvolution = pokemons[int.Parse(csv.Context.Record[3]) - 1].PokemonEvolution + output;
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

		public static void OutputCsharp(Pokemon[] pokemons)
		{
            File.Delete(SourcePath + @"\POKEMONOUTPUT " + Generations[displayItem] + ".txt");
            StreamWriter Output = File.CreateText(SourcePath + @"\POKEMONOUTPUT " + Generations[displayItem] + ".txt");
            Output.Dispose();
            using (Output = File.AppendText(SourcePath + @"\POKEMONOUTPUT " + Generations[displayItem] + ".txt"))
            {
                foreach(Pokemon poke in pokemons)
                {
                    string OutputText = poke.ToString();
                    OutputText = OutputText.Replace("\n", System.Environment.NewLine);
                    Output.WriteLine(OutputText);
                    Output.Write("\n");
                }
            }
		}

		#region CSV Read Commands
		/// <summary>
		/// Pokemon
		/// </summary>
		static void csv_Pokemon()
		{
			CsvReader = File.OpenText(csvFiles[129]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				if (csv.Context.Record[0] == Entry)
				{
					ID = csv.Context.Record[0];
					Species = csv.Context.Record[3];
					NAME = UpperCaseFirst(csv.Context.Record[1]);
					Weight = csv.GetField<double>(4) / 10.0 + "f";
					Height = csv.GetField<double>(3) / 10.0 + "f";
					EXPYield = csv.Context.Record[5];
					RegionalDex = csv.Context.Record[6];
					IsBaseForm = csv.Context.Record[7] == "1";
				}
			}
			Console.WriteLine($"Pokemon ID: {ID}, Name: {NAME.ToUpper()}");
			Console.Write("Progress |");
		}
		/// <summary>
		/// Pokemon_Types
		/// </summary>
		static void csv_Pokemon_Types()
		{
			CsvReader = File.OpenText(csvFiles[155]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				if (csv.Context.Record[0] == Entry)
				{
					Type1 = csv.Context.Record[1];
					csv.Read();
					if (csv.Context.Record[0] == Entry)
					{
						Type2 = csv.Context.Record[1];
					}
				}
			}
			Console.Write("+");
		}
		/// <summary>
		/// Types
		/// </summary>
		static void csv_Types()
		{
			CsvReader = File.OpenText(csvFiles[163]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				if (csv.Context.Record[0] == Type1)
				{
					Type1 = csv.Context.Record[1].ToUpper();
					Type1 = Type1;
				}
				else if (csv.Context.Record[0] == Type2)
				{
					Type2 = csv.Context.Record[1].ToUpper();
					Type2 = Type2;
				}
			}
			if (Type2 == null || Type2 == "")
			{
				Type2 = "NONE";
			}
			Console.Write("+");
		}
		/// <summary>
		/// Pokemon_Abilities
		/// </summary>
		static void csv_Pokemon_Abilities()
		{
			CsvReader = File.OpenText(csvFiles[130]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				//Is hidden
				if (csv.Context.Record[0] == Entry && int.Parse(csv.Context.Record[2]) == 1)
				{
					HiddenAbility = csv.Context.Record[1];
				}
				else if (csv.Context.Record[0] == Entry && csv.Context.Record[2] != "1")
				{
					Ability1 = csv.Context.Record[1];
					csv.Read();
					if (csv.Context.Record[0] == Entry && csv.Context.Record[2] != "1")
					{
						Ability2 = csv.Context.Record[1];
					}
					else if (csv.Context.Record[0] == Entry && csv.Context.Record[2] == "1")
					{
						HiddenAbility = csv.Context.Record[1];
					}
					csv.Read();
					if (csv.Context.Record[0] != Entry)
					{
						Ability2 = "NONE";
						Ability2 = Ability2;
					}
					else if (csv.Context.Record[0] == Entry && csv.Context.Record[2] == "1")
					{
						HiddenAbility = csv.Context.Record[1];
					}
				}
			}
			Console.Write("+");
		}
		/// <summary>
		/// Abilities
		/// </summary>
		static void csv_Abilities()
		{
			CsvReader = File.OpenText(csvFiles[0]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				if (csv.Context.Record[0] == Ability1)
				{
					Ability1 = UpperCaseFirst(csv.Context.Record[1]);
					Ability1 = Ability1.ToUpper().Replace(' ', '_').Replace('-', '_');
				}
				else if (csv.Context.Record[0] == Ability2 && Ability2 != "NONE")
				{
					Ability2 = UpperCaseFirst(csv.Context.Record[1]);
					Ability2 = Ability2.ToUpper().Replace(' ', '_').Replace('-', '_');
				}
				else if (csv.Context.Record[0] == HiddenAbility)
				{
					HiddenAbility = UpperCaseFirst(csv.Context.Record[1]);
					HiddenAbility = HiddenAbility.ToUpper().Replace(' ', '_').Replace('-', '_');
				}
			}

			if (HiddenAbility == "" || HiddenAbility == null)
			{
				HiddenAbility = "NONE";
				HiddenAbility = HiddenAbility.Replace(' ', '_').Replace('-', '_').ToUpper();
			}
			Console.Write("+");
		}
		/// <summary>
		/// Pokemon_Species
		/// </summary>
		static void csv_Pokemon_Species()
		{
			CsvReader = File.OpenText(csvFiles[149]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				if (csv.Context.Record[0] == Entry)
				{
					GenerationId = csv.Context.Record[2];
					HabitatId = csv.Context.Record[7];
					IsBaby = csv.Context.Record[11] == "1";
					HasGenderDiff = csv.Context.Record[13] == "1";
					FormSwitchable = csv.Context.Record[15] == "1";
					ShapeId = csv.Context.Record[6];
					GenderEnum = csv.Context.Record[8];
					MaleRatio = (100.0 - (100.0 / 8.0 * csv.GetField<double>(8))).ToString() + "f";
					CatchRate = csv.Context.Record[9];
					BaseFriendship = csv.Context.Record[10];
					HatchTime = int.Parse(csv.Context.Record[12]).ToString(); //(257 * int.Parse(csv.Context.Record[12])).ToString();
					GrowthRate = csv.Context.Record[14];
					Color_ID = csv.Context.Record[5];
					if (!string.IsNullOrEmpty(csv.Context.Record[3]))
					{
						//string test = csv.Context.Record[4];
						//ToDo: Evolution Id requires evolving FROM not evolving TO
						EvolutionID = int.Parse(csv.Context.Record[3]);
					}
				}
			}
			Console.Write("+");
		}
		/// <summary>
		/// Pokemon_habitats
		/// </summary>
		static void csv_Pokemon_habitats()
		{
			//CsvReader = File.OpenText(csvFiles[144]);
			//csv = new CsvReader(CsvReader);
			//while (csv.Read())
			//{
			//    if (csv.Context.Record[0] == Entry && csv.Context.Record[0] == HabitatId)
			//    {
			//        HabitatId = csv.Context.Record[1].Replace('-', '_').ToUpper();
			//    }
			//}
			//Console.Write("+");
		}
		/// <summary>
		/// Pokemon_colors
		/// </summary>
		static void csv_Pokemon_colors()
		{
			CsvReader = File.OpenText(csvFiles[131]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				if (csv.Context.Record[0] == Color_ID)
				{
					PokedexColor = csv.Context.Record[1].ToUpper();
				}
			}
			Console.Write("+");
		}
		/// <summary>
		/// Growth_Rates
		/// </summary>
		static void csv_Growth_Rates()
		{
			CsvReader = File.OpenText(csvFiles[67]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				if (csv.Context.Record[0] == GrowthRate)
				{
					LevelingRate = csv.Context.Record[1].ToUpper();
					LevelingRate = LevelingRate.Replace("-", "");
					LevelingRate = LevelingRate.Replace("SLOWTHENVERYFAST", "FLUCTUATING");
					if (LevelingRate == "MEDIUM")
					{
						LevelingRate = "MEDIUMFAST";
					}
					LevelingRate = LevelingRate.Replace("FASTTHENVERYSLOW", "ERRATIC");
				}
			}
			Console.Write("+");
		}
		/// <summary>
		/// Pokemon_Egg_Groups
		/// </summary>
		static void csv_Pokemon_Egg_Groups()
		{
			CsvReader = File.OpenText(csvFiles[134]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				if (csv.Context.Record[0] == Entry)
				{
					EggGroup1 = csv.Context.Record[1];
					csv.Read();
					//got a null error on last value
					if ((!string.IsNullOrEmpty(csv.Context.RawRecord) /*|| !string.IsNullOrEmpty(csv.Context.Record[0])*/) && csv.Context.Record[0] == Entry)
					{
						EggGroup2 = csv.Context.Record[1];
					}
					else
					{
						EggGroup2 = "NONE";
						EggGroup2 = EggGroup2;
					}
				}
			}
			Console.Write("+");
		}
		/// <summary>
		/// Egg_groups
		/// </summary>
		static void csv_Egg_groups()
		{
			CsvReader = File.OpenText(csvFiles[49]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				if (csv.Context.Record[0] == EggGroup1)
				{
					if (csv.Context.Record[1] == "ground")
					{
						EggGroup1 = "FIELD";
					}
					else if (csv.Context.Record[1] == "plant")
					{
						EggGroup1 = "GRASS";
					}
					else if (csv.Context.Record[1] == "humanshape")
					{
						EggGroup1 = "HUMANLIKE";
					}
					else if (csv.Context.Record[1] == "indeterminate")
					{
						EggGroup1 = "AMORPHOUS";
					}
					else if (csv.Context.Record[1] == "no-eggs")
					{
						EggGroup1 = "UNDISCOVERED";
					}
					else
					{
						EggGroup1 = csv.Context.Record[1].ToUpper();
					}
				}
				else if (csv.Context.Record[0] /*csv.Context.Record[0]*/ == EggGroup2 && EggGroup2 != "NONE")
				{
					if (csv.Context.Record[1] == "ground")
					{
						EggGroup2 = "FIELD";
					}
					else if (csv.Context.Record[1] == "plant")
					{
						EggGroup2 = "GRASS";
					}
					else if (csv.Context.Record[1] == "humanshape")
					{
						EggGroup2 = "HUMANLIKE";
					}
					else if (csv.Context.Record[1] == "indeterminate")
					{
						EggGroup2 = "AMORPHOUS";
					}
					else if (csv.Context.Record[1] == "no-eggs")
					{
						EggGroup2 = "UNDISCOVERED";
					}
					else
					{
						EggGroup2 = csv.Context.Record[1].ToUpper();
					}
				}
			}
			EggGroup1 = EggGroup1.Replace("-", "");
			if (EggGroup2 != null)
			{
				EggGroup2 = EggGroup2.Replace("-", "");
			}
			else
			{
				EggGroup2 = "NONE";
			}
			Console.Write("+");
		}
		/// <summary>
		/// Pokemon_Stats
		/// </summary>
		static void csv_Pokemon_Stats()
		{
			CsvReader = File.OpenText(csvFiles[154]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				if (csv.Context.Record[0] == Entry && csv.Context.Record[1] == "1")
				{
					BSHP = csv.Context.Record[2];
					EYHP = csv.Context.Record[3];
					csv.Read();
					if (csv.Context.Record[0] == Entry && csv.Context.Record[1] == "2")
					{
						BSATK = csv.Context.Record[2];
						EYATK = csv.Context.Record[3];
					}
					csv.Read();
					if (csv.Context.Record[0] == Entry && csv.Context.Record[1] == "3")
					{
						BSDEF = csv.Context.Record[2];
						EYDEF = csv.Context.Record[3];
					}
					csv.Read();
					if (csv.Context.Record[0] == Entry && csv.Context.Record[1] == "4")
					{
						BSSPA = csv.Context.Record[2];
						EYSPA = csv.Context.Record[3];
					}
					csv.Read();
					if (csv.Context.Record[0] == Entry && csv.Context.Record[1] == "5")
					{
						BSSPD = csv.Context.Record[2];
						EYSPD = csv.Context.Record[3];
					}
					csv.Read();
					if (csv.Context.Record[0] == Entry && csv.Context.Record[1] == "6")
					{
						BSSPE = csv.Context.Record[2];
						EYSPE = csv.Context.Record[3];
					}
				}
			}
			Console.Write("+");
		}
		/// <summary>
		/// Pokemon_Species_Names
		/// </summary>
		static void csv_Pokemon_Species_Names()
		{
			CsvReader = File.OpenText(csvFiles[152]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				if (csv.Context.Record[0] == Entry && csv.Context.Record[1] == "9")
				{
					Species = csv.Context.Record[3];
					Species = Species.Replace(" PokÃ©mon", "");
					Species = Species.Replace(" Pokémon", "");
					//Species = Species;
				}
			}
			Console.Write("+");
		}
		/// <summary>
		/// Pokemon_Species_Flavor_Text
		/// </summary>
		static void csv_Pokemon_Species_Flavor_Text()
		{
			CsvReader = File.OpenText(csvFiles[151]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				if (csv.Context.Record[0] == Entry && csv.Context.Record[1] == "23" && csv.Context.Record[2] == "9")
				{
					string PokedexEntry = csv.Context.Record[3];
					//Better to leave text formatting char for in-game textbox
					//PokedexEntry = PokedexEntry.Replace("\n", "");
					//PokedexEntry = PokedexEntry.Replace("\r", "");
					//PokedexEntry = PokedexEntry.Replace(".", ". ");
					PokedexEntry = PokedexEntry.Replace("  ", " ");
					PokedexEntry = PokedexEntry.Replace("   ", " ");
					//PokedexEntry = PokedexEntry.Remove(PokedexEntry.Length - 1);
					PokedexEntry = PokedexEntry.TrimEnd();
					Description = PokedexEntry;
				}
			}
			Console.Write("+");
		}
		/// <summary>
		/// Pokemon_Moves
		/// </summary>
		static void csv_Pokemon_Moves()
		{
			List<string> TMList = new List<string>();
			List<string> Moves = new List<string>();
			List<int> Level = new List<int>();
			List<int> Gen = new List<int>();
			List<string> Egg = new List<string>();
			List<string> Tutor = new List<string>();
			List<string> lbe = new List<string>();
			List<string> Purification = new List<string>();
			List<string> FormChange = new List<string>();
			List<string> Shadow = new List<string>();
			CsvReader = File.OpenText(csvFiles[144]); //148?
			csv = new CsvReader(CsvReader);
			csv.Read();
			while (csv.Read())
			{
				if (csv.Context.Record[0] == Entry && int.Parse(csv.Context.Record[1]) <= Convert.ToInt32(Generation) && csv.Context.Record[3] == "1" && !string.IsNullOrEmpty(csv.Context.Record[4]))
				{
					if (!Moves.Contains(csv.Context.Record[2]) /*& !Level.Contains(int.Parse(csv.Context.Record[4]))*/)
					{
						Level.Add(int.Parse(csv.Context.Record[4]));
						Gen.Add(int.Parse(csv.Context.Record[1]));
						Moves.Add(csv.Context.Record[2]);
					}
				}
				else if (csv.Context.Record[0] == Entry && int.Parse(csv.Context.Record[1]) <= Convert.ToInt32(Generation) && csv.Context.Record[3] == "4")
				{
					if (!TMList.Contains(csv.Context.Record[2])) TMList.Add(csv.Context.Record[2]);
				}
				else if (csv.Context.Record[0] == Entry && int.Parse(csv.Context.Record[1]) <= Convert.ToInt32(Generation) && csv.Context.Record[3] == "3")
				{
					if (!Tutor.Contains(csv.Context.Record[2])) Tutor.Add(csv.Context.Record[2]);
				}
				else if (csv.Context.Record[0] == Entry && int.Parse(csv.Context.Record[1]) <= Convert.ToInt32(Generation) && csv.Context.Record[3] == "2")
				{
					if (!Egg.Contains(csv.Context.Record[2])) Egg.Add(csv.Context.Record[2]);
				}
				else if (csv.Context.Record[0] == Entry && int.Parse(csv.Context.Record[1]) <= Convert.ToInt32(Generation) && csv.Context.Record[3] == "6")
				{
					if (!lbe.Contains(csv.Context.Record[2])) lbe.Add(csv.Context.Record[2]);
				}
				else if (csv.Context.Record[0] == Entry && int.Parse(csv.Context.Record[1]) <= Convert.ToInt32(Generation) && (csv.Context.Record[3] == "7" || csv.Context.Record[3] == "9"))
				{
					if (!Purification.Contains(csv.Context.Record[2])) Purification.Add(csv.Context.Record[2]);
				}
				else if (csv.Context.Record[0] == Entry && int.Parse(csv.Context.Record[1]) <= Convert.ToInt32(Generation) && csv.Context.Record[3] == "10")
				{
					if (!FormChange.Contains(csv.Context.Record[2])) FormChange.Add(csv.Context.Record[2]);
				}
				else if (csv.Context.Record[0] == Entry && int.Parse(csv.Context.Record[1]) <= Convert.ToInt32(Generation) && csv.Context.Record[3] == "8")
				{
					if (!Shadow.Contains(csv.Context.Record[2])) Shadow.Add(csv.Context.Record[2]);
				}
			}
			string[] TMArray = TMList.ToArray();
			string[] EggArray = Egg.ToArray();
			string[] TutorArray = Tutor.ToArray();
			string[] LBEArray = lbe.ToArray();
			string[] PureArray = Purification.ToArray();
			string[] FormArray = FormChange.ToArray();
			string[] ShadowArray = Shadow.ToArray();
			Console.Write("+");

			csv_Move_Names(TMList.ToArray(), Egg.ToArray(), Tutor.ToArray(),
				lbe.ToArray(), Purification.ToArray(), FormChange.ToArray(), Shadow.ToArray(), Moves, Level, Gen);
		}
		/// <summary>
		/// Move_Names
		/// </summary>
		static void csv_Move_Names(
			string[] TMArray, string[] EggArray, string[] TutorArray, string[] LBEArray, 
			string[] PureArray, string[] FormArray, string[] ShadowArray, List<string> Move, List<int> Level, List<int> Gen)
		{
			//Dictionary<string, int> MoveLevelDictionary = new Dictionary<string, int>();
			//Dictionary<string, int[]> MoveLevelDictionary = new Dictionary<string, int[]>();
			string TM = "";
			string LevelString = "";
			string SEgg = "";
			string STutor = "";
			string SLBE = "";
			string SPure = "";
			string SForm = "";
			string SShadow = "";

			CsvReader = File.OpenText(csvFiles[93]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				for (int i = 0; i < TMArray.Length; i++)
				{
					if (csv.Context.Record[0] == TMArray[i])
					{
						string TMMove = csv.Context.Record[1];
						TMMove = TMMove.Replace(' ', '_');
						TMMove = TMMove.Replace('-', '_');
						TM = TM +
						$"new PokemonMoveset" +
						$"(" +
							$"\n\t\tmoveId: Moves.{TMMove.ToUpper()}," +
							$"\n\t\tmethod: LearnMethod.machine" +
						$"\n\t), ";
					}
				}
				for (int i = 0; i < EggArray.Length; i++)
				{
					if (csv.Context.Record[0] == EggArray[i])
					{
						string EggMove = csv.Context.Record[1];
						EggMove = EggMove.Replace(' ', '_');
						EggMove = EggMove.Replace('-', '_');
						SEgg = SEgg +
						$"new PokemonMoveset" +
						$"(" +
							$"\n\t\tmoveId: Moves.{EggMove.ToUpper()}," +
							$"\n\t\tmethod: LearnMethod.egg" +
						$"\n\t), ";
					}
				}
				for (int i = 0; i < TutorArray.Length; i++)
				{
					if (csv.Context.Record[0] == TutorArray[i])
					{
						string TutorMove = csv.Context.Record[1];
						TutorMove = TutorMove.Replace(' ', '_');
						TutorMove = TutorMove.Replace('-', '_');
						STutor = STutor +
						$"new PokemonMoveset" +
						$"(" +
							$"\n\t\tmoveId: Moves.{TutorMove.ToUpper()}," +
							$"\n\t\tmethod: LearnMethod.tutor" +
						$"\n\t), ";
					}
				}
				for (int i = 0; i < LBEArray.Length; i++)
				{
					if (csv.Context.Record[0] == LBEArray[i])
					{
						string LBEMove = csv.Context.Record[1];
						LBEMove = LBEMove.Replace(' ', '_');
						LBEMove = LBEMove.Replace('-', '_');
						SLBE = SLBE +
						$"new PokemonMoveset" +
						$"(" +
							$"\n\t\tmoveId: Moves.{LBEMove.ToUpper()}," +
							$"\n\t\tmethod: LearnMethod.light_ball_egg" +
						$"\n\t), ";
					}
				}
				for (int i = 0; i < FormArray.Length; i++)
				{
					if (csv.Context.Record[0] == FormArray[i])
					{
						string FormMove = csv.Context.Record[1];
						FormMove = FormMove.Replace(' ', '_');
						FormMove = FormMove.Replace('-', '_');
						SForm = SForm +
						$"new PokemonMoveset" +
						$"(" +
							$"\n\t\tmoveId: Moves.{FormMove.ToUpper()}," +
							$"\n\t\tmethod: LearnMethod.form_change" +
						$"\n\t), ";
					}
				}
				for (int i = 0; i < PureArray.Length; i++)
				{
					if (csv.Context.Record[0] == PureArray[i])
					{
						string PureMove = csv.Context.Record[1];
						PureMove = PureMove.Replace(' ', '_');
						PureMove = PureMove.Replace('-', '_');
						SPure = SPure +
						$"new PokemonMoveset" +
						$"(" +
							$"\n\t\tmoveId: Moves.{PureMove.ToUpper()}," +
							$"\n\t\tmethod: LearnMethod.purification" +
						$"\n\t), ";
					}
				}
				for (int i = 0; i < ShadowArray.Length; i++)
				{
					if (csv.Context.Record[0] == ShadowArray[i])
					{
						string ShadowMove = csv.Context.Record[1];
						ShadowMove = ShadowMove.Replace(' ', '_');
						ShadowMove = ShadowMove.Replace('-', '_');
						SShadow = SShadow +
						$"new PokemonMoveset" +
						$"(" +
							$"\n\t\tmoveId: Moves.{ShadowMove.ToUpper()}," +
							$"\n\t\tmethod: LearnMethod.shadow" +
						$"\n\t), ";
					}
				}
				for (int i = 0; i < Move.Count; i++)
				{
					if (csv.Context.Record[0] == Move[i])// && int.Parse(csv.Context.Record[2]) == Convert.ToInt32(Generation)
					{
						//Move[i] = csv.Context.Record[1];
						//MoveLevelDictionary.Add(Moves[i], new int[] { Level[i], Gen[i] });
						//string moveId = pair.Key.ToString().Replace(' ', '_');
						//moveId = moveId.Replace('-', '_');
						LevelString = LevelString +
							($"new PokemonMoveset" +
							$"(" +
								$"\n\t\tmoveId: Moves.{csv.Context.Record[1].Replace(' ', '_').Replace('-', '_').ToUpper()}," +
								$"\n\t\tmethod: LearnMethod.levelup," +
								$"\n\t\tlevel: {Level[i]}" +
								$"\n\t\t//,generation: {Gen[i]}" +
							$"\n\t), ");
					}
				}
			}
			/*for (int i = 0; i < Moves.Count; i++)
			{
				try
				{
					MoveLevelDictionary.Add(Moves[i], new int[] { Level[i], Gen[i] });
				}
				catch
				{
					//Do nothing and the int value will increase on it's own. Otherwise you're skipping over another value
					//i++;
				}
			}*/

			/*var items = from KeyValuePair in MoveLevelDictionary orderby KeyValuePair.Value[0] ascending select KeyValuePair;

			foreach (KeyValuePair<string, int[]> pair in items)
			{
				string moveId = pair.Key.ToString().Replace(' ', '_');
				moveId = moveId.Replace('-', '_');
				LevelString = LevelString +
					($"new PokemonMoveset" +
					$"(" +
						$"\n\t\tmoveId: Moves.{moveId.ToUpper()}," +
						$"\n\t\tmethod: LearnMethod.levelup," +
						$"\n\t\tlevel: {pair.Value[0]}," +
						$"\n\t\t//generation: {pair.Value[1]}" +
					$"\n\t), ");
			}*/
			Console.Write("+");
			//string FinalMoves = $"\t{LevelString}\n\n\t{TM}\n\n\t{SEgg}\n\n\t{STutor}\n\n\t{SLBE}\n\n\t{SShadow}\n\n\t{SPure}\n\n\t{SForm}";
			string FinalMoves = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
				string.IsNullOrWhiteSpace(LevelString) ? "" : "\t" + LevelString,
				string.IsNullOrWhiteSpace(TM) ? "" : "\n\n\t" + TM,
				string.IsNullOrWhiteSpace(SEgg) ? "" : "\n\n\t" + SEgg,
				string.IsNullOrWhiteSpace(STutor) ? "" : "\n\n\t" + STutor,
				string.IsNullOrWhiteSpace(SLBE) ? "" : "\n\n\t" + SLBE,
				string.IsNullOrWhiteSpace(SShadow) ? "" : "\n\n\t" + SShadow,
				string.IsNullOrWhiteSpace(SPure) ? "" : "\n\n\t" + SPure,
				string.IsNullOrWhiteSpace(SForm) ? "" : "\n\n\t" + SForm
			);
			//FinalMoves = FinalMoves.Replace("\n", System.Environment.NewLine);

			/*If last comma doesnt exist, you could end up removing an entire value off the end
			var index = FinalMoves.LastIndexOf(',');
			if (index >= 0)
			{
				FinalMoves = FinalMoves.Substring(0, index);
			}*/

			Moves = FinalMoves.Trim(new char[] { ',', ' ' });
		}
		/// <summary>
		/// Pokemon_Items
		/// </summary>
		static void csv_Pokemon_Items()
		{
			List<string> Items = new List<string>();
			List<int> Rarity = new List<int>();
			List<int> Version = new List<int>();
			CsvReader = File.OpenText(csvFiles[145]);
			csv = new CsvReader(CsvReader);
			csv.Read();
			while (csv.Read())
			{
				if (csv.Context.Record[0] == Entry && int.Parse(csv.Context.Record[1]) <= Convert.ToInt32(Generation))
				{
					//if(!Items.Contains(csv.Context.Record[2]) /*& !Version.Contains(int.Parse(csv.Context.Record[4]))*/) { 
					Rarity.Add(int.Parse(csv.Context.Record[4]));
					Version.Add(int.Parse(csv.Context.Record[1]));
					Items.Add(csv.Context.Record[2]);
					//}
				}
			}
			Console.Write("+");

			csv_Item_Names(Items, Rarity, Version);
		}
		/// <summary>
		/// Item_Names
		/// </summary>
		static void csv_Item_Names(List<string> Items, List<int> Rarity, List<int> Version)
		{
			string ItemString = "";
			CsvReader = File.OpenText(csvFiles[81]);
			csv = new CsvReader(CsvReader);
			while (csv.Read())
			{
				for (int i = 0; i < Items.Count; i++)
				{
					if (csv.Context.Record[0] == Items[i] && csv.Context.Record[1] == "9")// && int.Parse(csv.Context.Record[2]) == Convert.ToInt32(Generation)
					{
						//Items[i] = csv.Context.Record[1];
						//HeldItemDictionary.Add(Items[i], new int[] { Rarity[i], Version[i] });
						//string itemId = pair.Key.ToString().Replace(' ', '_');
						//itemId = itemId.Replace('-', '_');
						ItemString = ItemString +
							($"new PokemonHeldItems" +
							$"(" +
								"\n\t\titemId: Items." + csv.Context.Record[2]
									.Replace("PokÃ©mon", "Pokémon") //ToDo: regular `e`?
									.Replace("PokÃ©", "Poké")
									.Replace("â€™", "")
									.Replace(' ', '_')
									.Replace('-', '_')
									.Replace(".", "")
									.ToUpper() +
								$"\n\t\tpercent: {Rarity[i]}" +
								$"\n\t\t//,generation: {Version[i]}" +
							$"\n\t), ");
					}
				}
			}

			Console.Write("+");
			//string HeldItems = $"\t{ItemString}";
			string HeldItems = string.Format("{0}",
				string.IsNullOrWhiteSpace(ItemString) ? "" : "\t" + ItemString
			);
		}
		/// <summary>
		/// </summary>
		static void csv_()
		{
		}
		#endregion
	}

	/*
     * Okay so we have the Species Id in evolution
     * We take that value (Which is the entry number)
     * we go into Pokemon Species and take the evolved from value
     * Add the method to the Pokemon and at the end override the ToString method
    */
}
