// See https://aka.ms/new-console-template for more information
using System.IO;
using System.Linq;

public class Program
{

    static void Main(string[] args)
    {

        bool open = true;
        while (open)
        {
            string response;
            Console.WriteLine("Handy Degrees Of Progress dice simulator for statistics!");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("by mattihase");
            Console.WriteLine("");
            Console.WriteLine("");
            int usethisbase;
            int usethisDice;
            int usethisDiscord;
            try
            {
                Console.WriteLine("Which sided dice would you like to test?");
                usethisbase = int.Parse(Console.ReadLine());
                Console.WriteLine("How many Regular Dice to roll?");
                usethisDice = int.Parse(Console.ReadLine());
                Console.WriteLine("How many Discord Dice to roll?");
                usethisDiscord = int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.WriteLine("Please use valid numbers");
                Console.WriteLine("Test again? (y/n)");
                response = Console.ReadLine();
                if (response != "y" && response != "Y")
                    open = false;
                Console.Clear();

                continue;
            }
            Results test = new Results(usethisDice, usethisDiscord, usethisbase);
            Console.WriteLine("How many samples do you want the statistics to be generated from?");
            int samples = int.Parse(Console.ReadLine());
            Console.WriteLine("Generating Results");
            test.RunTest(samples);

            Console.WriteLine("Test again? (y/n)");
            response = Console.ReadLine();
            if (response != "y" && response != "Y")
                open = false;
            Console.Clear();
        }

        return;
    }

}
class Results 
{
    public string name = "0d10 + 0Dd10";
    public int DiceBase = 10;

    public int Dice;
    public int DiscordDice;
    public int SampleSize = 0; //how many rolls taken
    System.Random prng;


    int highestUniqueDiceRolled;
    int highestUniqueDiscordDiceRolled;
    int highestUniquePrunedDiceRolled;

    List<int> DiceExtraneous = new List<int>();
    List<int> DiceDifferent = new List<int>();

    List<int> DiscordDiceExtraneous = new List<int>();
    List<int> DiscordDiceDifferent = new List<int>();

    List<int> PrunedDiceExtraneous = new List<int>();
    List<int> PrunedDiceDifferent = new List<int>();

    List<int> DiscordCancels = new List<int>();
    void Roll() 
    {
        List<int> DiceRolls = new List<int>();
        List<int> DiscordDiceRolls = new List<int>();
        List<int> PrunedDiceRolls = new List<int>();
        for (int i = 0; i < Dice; i++)
        {
            DiceRolls.Add(prng.Next(DiceBase) + 1);
        }
        for (int i = 0; i < DiscordDice; i++)
        {
            DiscordDiceRolls.Add(prng.Next(DiceBase) + 1);
        }
        PrunedDiceRolls.AddRange(DiceRolls);
        List<int> PrunedDiscordDiceRolls = new List<int>();
        for (int i = 0; i < DiscordDiceRolls.Count; i++)
        {
            if (PrunedDiceRolls.Contains(DiscordDiceRolls[i]))
            {
                PrunedDiceRolls.Remove(DiscordDiceRolls[i]);
            }
            else
                PrunedDiscordDiceRolls.Add(DiscordDiceRolls[i]);
        }

        PrunedDiceRolls.AddRange(PrunedDiscordDiceRolls);

        List<int> UniqueDiceRolls = new List<int>();
        int UniqueDiceRollsTally = 0;
        int DuplicateDiceRollsTally = 0;
        for (int i = 0; i < DiceRolls.Count; i++)
        {
            if (UniqueDiceRolls.Contains(DiceRolls[i]))
            {
                DuplicateDiceRollsTally++;
            }
            else 
            {
                UniqueDiceRolls.Add(DiceRolls[i]);
                UniqueDiceRollsTally++;
            }
        }
        List<int> UniqueDiscordDiceRolls = new List<int>();
        int UniqueDiscordDiceRollsTally = 0;
        int DuplicateDiscordDiceRollsTally = 0;
        for (int i = 0; i < DiscordDiceRolls.Count; i++)
        {
            if (UniqueDiscordDiceRolls.Contains(DiscordDiceRolls[i]))
            {
                DuplicateDiscordDiceRollsTally++;
            }
            else
            {
                UniqueDiscordDiceRolls.Add(DiscordDiceRolls[i]);
                UniqueDiscordDiceRollsTally++;
            }
        }
        List<int> UniquePrunedDiceRolls = new List<int>();
        int UniquePrunedDiceRollsTally = 0;
        int DuplicatePrunedDiceRollsTally = 0;
        for (int i = 0; i < PrunedDiceRolls.Count; i++)
        {
            if (UniquePrunedDiceRolls.Contains(PrunedDiceRolls[i]))
            {
                DuplicatePrunedDiceRollsTally++;
            }
            else
            {
                UniquePrunedDiceRolls.Add(PrunedDiceRolls[i]);
                UniquePrunedDiceRollsTally++;
            }
        }

        DiceDifferent.Add(UniqueDiceRollsTally);
        DiceExtraneous.Add(DuplicateDiceRollsTally);
        DiscordDiceDifferent.Add(UniqueDiscordDiceRollsTally);
        DiscordDiceExtraneous.Add(DuplicateDiscordDiceRollsTally);
        PrunedDiceDifferent.Add(UniquePrunedDiceRollsTally);
        PrunedDiceExtraneous.Add(DuplicatePrunedDiceRollsTally);

        DiscordCancels.Add(DiceRolls.Count + DiscordDiceRolls.Count - PrunedDiceRolls.Count);

        if (UniqueDiceRollsTally > highestUniqueDiceRolled)
            highestUniqueDiceRolled = UniqueDiceRollsTally;
        if (UniqueDiscordDiceRollsTally > highestUniqueDiscordDiceRolled)
            highestUniqueDiscordDiceRolled = UniqueDiscordDiceRollsTally;
        if (UniquePrunedDiceRollsTally > highestUniquePrunedDiceRolled)
            highestUniquePrunedDiceRolled = UniquePrunedDiceRollsTally;

        SampleSize++;
    }



    public Results(int dice, int discordDice) 
    {
        Dice = Math.Max(0, dice);
        DiceBase = 10;
        DiscordDice = Math.Max(0, discordDice);
        name = "";
        if (Dice > 0) 
        {
            name += Dice + "d" + DiceBase;
            if (DiscordDice > 0)
                name += " + ";
        }
        if (DiscordDice > 0)
            name += DiscordDice + "Dd" + DiceBase;

        prng = new Random(System.DateTime.UtcNow.Millisecond);
        highestUniqueDiceRolled = 0;
        highestUniqueDiscordDiceRolled = 0;
        highestUniquePrunedDiceRolled = 0;

    }
    public Results(int dice, int discordDice, int dBase)
    {
        Dice = Math.Max(0, dice);
        DiceBase = dBase;
        DiscordDice = Math.Max(0, discordDice);
        name = "";
        if (Dice > 0)
        {
            name += Dice + "d" + DiceBase;
            if (DiscordDice > 0)
                name += " + ";
        }
        if (DiscordDice > 0)
            name += DiscordDice + "Dd" + DiceBase;

        prng = new Random(System.DateTime.UtcNow.Millisecond);
        highestUniqueDiceRolled = 0;
        highestUniqueDiscordDiceRolled = 0;
        highestUniquePrunedDiceRolled = 0;

    }

    public void RunTest(int sampleize) 
    {
        if (sampleize <= 0)
        {
            Console.WriteLine("I wouldn't exactly call that a sample size");
            return;
        }
        if (Dice + DiscordDice <= 0)
        {
            Console.WriteLine("No dice, baby!");
            return;
        }
        if (DiceBase <= 0)
        {
            Console.WriteLine("Zero sided dice are outside of the purview of this program");
            return;
        }
        for (int i = 0; i < sampleize; i++)
        {
            Roll();
        }
        Evaluate();
    }

    public void Evaluate() 
    {
        int[] diceOccuranceCounts = new int[highestUniqueDiceRolled + 1];
        if (Dice > 0)
        {
            for (int i = 0; i < DiceDifferent.Count; i++)
            {
                diceOccuranceCounts[DiceDifferent[i] - 1]++;
            }
        }
        int[] discordDiceOccuranceCounts = new int[highestUniqueDiscordDiceRolled];
        if (DiscordDice > 0)
        {
            for (int i = 0; i < DiscordDiceDifferent.Count; i++)
            {
                discordDiceOccuranceCounts[DiscordDiceDifferent[i] - 1]++;
            }
        }
        int[] prunedDiceOccuranceCounts = new int[highestUniquePrunedDiceRolled];
        for (int i = 0; i < PrunedDiceDifferent.Count; i++)
        {
            prunedDiceOccuranceCounts[PrunedDiceDifferent[i] - 1]++;
        }

        float discordcancelamount = 0;
        for (int i = 0; i < DiscordCancels.Count; i++)
        {
            discordcancelamount += (float)DiscordCancels[i];
        }
        float discordCancelAverage = discordcancelamount / DiscordCancels.Count;

        Console.Clear();
        Console.WriteLine(name + "stats, For a sample size of " + SampleSize);
        if (Dice > 0)
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Regular Dice:");
            for (int i = 0; i < highestUniqueDiceRolled; i++)
            {
                float proportionPercent = ((float)diceOccuranceCounts[i] / (float)DiceDifferent.Count) * 100;
                int BarLines = Math.Max((int)(proportionPercent / 5), 1);
                for (int b = 0; b < BarLines; b++)
                {
                    Console.Write("|");
                }
                Console.Write((i + 1) + " = " + proportionPercent + "% ");
                Console.Write('\n');
            }
        }

        if (DiscordDice > 0)
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Discord Dice:");
            for (int i = 0; i < highestUniqueDiscordDiceRolled; i++)
            {
                float proportionPercent = ((float)discordDiceOccuranceCounts[i] / (float)DiscordDiceDifferent.Count) * 100;
                int BarLines = Math.Max((int)(proportionPercent / 5), 1);
                for (int b = 0; b < BarLines; b++)
                {
                    Console.Write("|");
                }
                Console.Write((i + 1) + " = " + proportionPercent + "% ");
                Console.Write('\n');
            }
            Console.WriteLine("");
            Console.WriteLine("Average dice pruned as a result of a discord collision: " + discordCancelAverage + " per roll");
        }

        Console.WriteLine("");
        Console.WriteLine("");
        Console.WriteLine("Final (pruned) roll results:");
        for (int i = 0; i < highestUniquePrunedDiceRolled; i++)
        {
            float proportionPercent = ((float)prunedDiceOccuranceCounts[i] / (float)PrunedDiceDifferent.Count) * 100;
            int BarLines = Math.Max((int)(proportionPercent / 5), 1);
            for (int b = 0; b < BarLines; b++)
            {
                Console.Write("|");
            }
            Console.Write((i + 1) + " = " + proportionPercent + "% ");
            Console.Write('\n');
        }
    }
}