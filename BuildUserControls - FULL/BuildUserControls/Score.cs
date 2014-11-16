using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuildUserControls
{
    public enum Options
    {
        ONE = 0, TWO, THREE, FOUR, FIVE, SIX, SUM, BONUS, THREE_OF_A_KIND,
        FOUR_OF_A_KIND, FULL_HOUSE, SMALL_STRAIGHT, LARGE_STRAIGHT,YAHTZEE, CHANCE, TOTAL
    };
    public class Score
    {
        int?[] scores = new int?[Enum.GetValues(typeof(Options)).Length];
        int[] sScore = new int[Enum.GetValues(typeof(Options)).Length];

        public int[] SScore
        {
            get { return sScore; }
            set { sScore = value; }
        }
        
        public void reset()
        {
            Array.Clear(scores, 0, scores.Length);
            Array.Clear(sScore, 0, sScore.Length);
        }
        /// <summary>
        /// checks if the lists of numbers are true
        /// </summary>
        /// <param name="numbers"> int</param>
        /// <returns> true or false</returns>
        public bool equal(params int[] numbers)
        {
            for(int i=0; i< numbers.Length-1; i++)
            {
                if(!int.Equals(numbers[i], numbers[i+1]))
                    return false;
            }
            return true;
        }

        private bool contains(int[] array,string orAnd="and", params int[]prm)
        {
            foreach (int item in prm)
            {
               if( array.Contains(item))
               {
                   if (orAnd == "or") 
                   {
                       return true;
                   }
               }
               else
               {
                   if (orAnd == "and")
                       return false;
               }
            }
           return orAnd.Equals("and") ?  true :  false;
        }
        public void suggest(params int[] dice)
        {
            reset();
            bool three = false;
            Array.Sort(dice);
            sScore[(int)Options.CHANCE] = dice.Sum();
            foreach (int die in dice)
            {
                switch (die)
                {
                    case 1: if(equal(sScore[(int)Options.ONE]+=1,3)) three=true; break;
                    case 2: if(equal(sScore[(int)Options.TWO]+=2,6)) three=true; break;
                    case 3: if(equal(sScore[(int)Options.THREE]+=3,9)) three=true; break;
                    case 4: if(equal(sScore[(int)Options.FOUR]+=4,12)) three=true; break;
                    case 5: if(equal(sScore[(int)Options.FIVE]+=5,15))three=true; break;
                    case 6: if(equal(sScore[(int)Options.SIX]+=6, 18)) three = true; break;
                    default: break;
                }
            }
            
            if(three)//means that there might be 4 of a kind or full house...
            {
                
                sScore[(int)Options.THREE_OF_A_KIND] = dice.Sum();
                if (equal(dice[0], dice[1], dice[2], dice[3])) 
                {
                    sScore[(int)Options.FOUR_OF_A_KIND] = dice.Sum();
                    if(int.Equals(dice[3],dice[4]))
                    {
                        //event yahtzee
                        sScore[(int)Options.YAHTZEE] = 50;
                        sScore[(int)Options.FULL_HOUSE] = 25;
                    }
                }
                else
                {
                    if (equal(dice[1], dice[2], dice[3], dice[4]))
                    {
                        sScore[(int)Options.FOUR_OF_A_KIND] = dice.Sum();
                    }
                }
                //full house
                if(equal(dice[0], dice[1], dice[2])&& equal(dice[3], dice[4])
                    ||equal(dice[2], dice[3], dice[4])&&equal(dice[0], dice[1]))
                {
                    sScore[(int)Options.FULL_HOUSE] = 25;
                }
                
            }
            else//possibility for straights
            {
                //contains "3, 4"
                //{if contains "2, 5" smallstraight!
                //   {if contains "1 or 6" largestraight}
                // else "1 , 2" small
                if (contains(dice,"and",3,4))
                {
                    if(contains(dice, "and", 2,5))
                    {
                        sScore[(int)Options.SMALL_STRAIGHT] = 30;
                        if (contains(dice,"or",1,6))
                        {
                            sScore[(int)Options.LARGE_STRAIGHT] = 40;
                        }
                    }
                    else
                    {
                        if(contains(dice,"and",1, 2))
                        {
                            sScore[(int)Options.SMALL_STRAIGHT] = 30;
                        }
                        else if(contains(dice,"and",5,6))
                        {
                            sScore[(int)Options.SMALL_STRAIGHT] = 30;
                        }

                    }
                }
            }


        }

        
    }
}
