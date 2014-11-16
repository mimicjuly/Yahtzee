using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace BuildUserControls
{
    public class Player
    {
        private string name;
        private int currentScore, totalScore;
        public ScoreSheet sheet = new ScoreSheet();
        public string Name 
        { 
            get { return name; } 
            set 
            { 
                name = value;
                if(name!="")
                    sheet.setName(name);
            }
        }
        public int Score
        {
            set { currentScore = value; }
            get
            {
                if (currentScore != sheet.Total)
                {
                    totalScore += sheet.Total - currentScore;
                    currentScore = sheet.Total;                    
                }
               return currentScore;
            }
        }
        public int TotalScore
        {
            get
            {
                return totalScore;
            }
        }
        public void reset()
        {
            currentScore = 0;
            sheet.set();
        }
        public void suggestion(params int [] prm)
        {
            sheet.setSuggestion(prm);
        }
        public void setIsTurn(bool isTurn)
        {
            if (isTurn)
                sheet.Visibility = Visibility.Visible;
            else
                sheet.Visibility = Visibility.Hidden;
        }
    }
}
