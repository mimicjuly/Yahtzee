using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace BuildUserControls
{
    /// <summary>
    /// Interaction logic for ScoreSheet.xaml
    /// </summary>   
    public partial class ScoreSheet : UserControl
    {       
        public class DataObject : INotifyPropertyChanged
        {
            private int score;
            private string color;
            private string scoreString;
            private bool isSelectable;            
            private string textColor;
            public event PropertyChangedEventHandler PropertyChanged;
            public int Scores
            {
                get { return score; }
                set
                {                   
                    score=value;
                    ScoreString = score.ToString();
                }
                    
                
            }
            public string ScoreString
            {
                get { return scoreString; }
                set 
                {
                    if (value == "0"&&IsSelectable)
                        scoreString = "x";
                    else if (value == "0"||value=="-1")
                        scoreString = "";
                    else
                        scoreString = value;
                    OnPropertyChanged("ScoreString");
                }
            }
            public string Colors
            {
                get { return color; }
                set
                {
                    color = value;
                    OnPropertyChanged("Colors");
                }
            }
            public string TextColor
            {
                get { return textColor; }
                set
                {
                    textColor = value;
                    OnPropertyChanged("TextColor");
                }
            }
            public bool IsSelectable
            {
                get { return isSelectable; }
                set { isSelectable = value; }
            }
            public string Title { get; set; }         
            protected void OnPropertyChanged(string name)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(name));
                }
            }
        }
        
        public static event EventHandler SelectedOption;
        ObservableCollection<DataObject> collect = new ObservableCollection<DataObject>();
        public List<string> list;
        public int Total { get { return collect[(int)Options.TOTAL].Scores; } }
        public Score score;
        public Options op;
        public ScoreSheet()
        {
            InitializeComponent();
            this.dataGrid1.ItemsSource = collect;
            score= new Score();
            set();
        }
        protected void OnSelectedOption()
        {
            EventHandler handler = SelectedOption;
            if (handler != null)
            {
                handler(this, null);
            }
        }        
        public void setName(string name)
        {
            nameLb.Content = name;
        }
        public void set() 
        {
            score.reset();
            collect.Clear();
            string colors, linkColor = "purple";
            bool isSelectable=true;
            list = Enum.GetValues(typeof(Options)).Cast<Options>().Select(v => v.ToString().Replace("_", " ")).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 6 || i == 7 || i == 15)
                { linkColor = "#FFA00202"; colors = "#FFA00202"; isSelectable = false; }
                else
                { linkColor = "purple"; colors = "black"; isSelectable = true; }
                collect.Add(new DataObject() { Title = list[i],TextColor=linkColor, Scores = 0, IsSelectable = isSelectable, Colors = colors});
            }

        }
        public void setSuggestion(params int[] dice)
        {           
            score.suggest(dice);
            for (int i = 0; i < list.Count; i++)
            {
                if (collect[i].IsSelectable)  
                {                    
                    collect[i].Scores = score.SScore[i];
                    collect[i].ScoreString = score.SScore[i].ToString();
                }
            }          
        }
        private void selected_Choice(object sender, RoutedEventArgs e)
        {
            int index = dataGrid1.SelectedIndex;           
            if (!collect[index].IsSelectable)
                return;
            Button btn = (Button)e.OriginalSource;          
            collect[index].IsSelectable = false;
            btn.IsHitTestVisible = false;
            dataGrid1.SelectedIndex = -1;
            collect[(int)Options.TOTAL].Scores += collect[index].Scores;
            if(index<6)
            {
               collect[(int)Options.SUM].Scores += collect[index].Scores;
                if (collect[(int)Options.SUM].Scores >= 63)
                {
                    collect[(int)Options.BONUS].Scores = 35;
                    collect[(int)Options.TOTAL].Scores += 35;
                }
            }
            foreach (DataObject obj in collect)
            {
                if (obj.IsSelectable)
                    obj.Scores = -1;
            }
            OnSelectedOption();//raise event that it was selected
         }
    }
}
