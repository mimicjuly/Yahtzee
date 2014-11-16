using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Controls.Primitives;

namespace BuildUserControls
{
    /// <summary>
    /// Interaction logic for YAHTZEE.xaml
    /// </summary>
    public partial class YAHTZEE : Window
    {
        MediaPlayer md = new MediaPlayer();
        static int Counter = 5;
        static Random random = new Random();
        DoubleAnimation da = new DoubleAnimation();                                                     
        List<Dice> alldice = new List<Dice>();
        List<Button> selectedbtnd = new List<Button>();
        List<Button> notSelectedbtnd = new List<Button>();
        Game game = new Game(13);
        List<int> deck = new List<int>();
        List<int> rows = new List<int>(){ 0, 1, 2, 3, 4 };
        List<int> columns = new List<int>{ 0, 1, 2, 3, 4,5,6 };
        int rolls = 2;
        int[] position = { 1, 2, 3, 4, 5 };
        public YAHTZEE()
        {
            InitializeComponent();
            // Attached event
            this.AddHandler(Dice.StopRollEvent, new RoutedPropertyChangedEventHandler<int>((X, Y) => { rollended(Y.OldValue); }));
            this.DataContext = this;
            //game.GameEnded+=game_GameEnded;
            alldice.Add(Dice1);
            alldice.Add(Dice2);
            alldice.Add(Dice3);
            alldice.Add(Dice4);
            alldice.Add(Dice5);
            selectedbtnd.Add(Dice1Btn);
            selectedbtnd.Add(Dice2Btn);
            selectedbtnd.Add(Dice3Btn);
            selectedbtnd.Add(Dice4Btn);
            selectedbtnd.Add(Dice5Btn);
            da.AutoReverse = true;
            da.Duration = new Duration(TimeSpan.FromSeconds(3));
            da.RepeatBehavior = new RepeatBehavior(TimeSpan.FromSeconds(9));
            ScoreSheet.SelectedOption += sheet_SelectedOption;
            game.ShowDialog();
            this.WindowState = game.WindowState;
            countdn.TimerEnded+=Timer_Ended;
            Grid.SetColumn(game.sheetGrid, 0);
            Grid.SetRow(game.sheetGrid, 0);
            sheetGrid.Children.Add(game.sheetGrid);
      
        }       
        private void sheet_SelectedOption(object sender, EventArgs e)
        {
            //call resetDice
			countdn.start();
			startbtn.IsEnabled=false;
			resetDice();
			
        }	       
        private void Timer_Ended(object sender, EventArgs e)
        {
            //call resetDice
			startbtn.IsEnabled=true;			
			game.NextPlayer();
            
        }
        private void resetDice()
        {
            while (notSelectedbtnd.Count>0)
            {
                resetDiebtn(notSelectedbtnd[0]);
            }
            rolls = 2;
        }
        private void rollended(int num)
        {
            moveDie(num);
            if (--Counter == 0)
            {             
                rolls--;
                game.player.suggestion(alldice[0].State+1, alldice[1].State+1, alldice[2].State+1, alldice[3].State+1, alldice[4].State+1);
            }
        }
        private void moveDie(int num)
        {
            Button btn=null;
            Random rndr = new Random();
            Random rndc = new Random();
            Random rotate = new Random();
            RotateTransform rt = new RotateTransform(0, 0, 0);
            rows = rows.OrderBy(x => rndr.Next()).ToList();
            columns = columns.OrderBy(x => rndc.Next()).ToList();
            foreach (Button item in selectedbtnd)
            {
                if (item.Name == "Dice" + num + "Btn")
                {                   
                    deck.Add(Grid.GetColumn(item));                    
                    item.RenderTransform = rt;
                    item.RenderTransformOrigin = new Point(0.5, 0.5);           
                    Grid.SetColumn(item, columns[0]);
                    Grid.SetRow(item, rows[0]);
                    rt.Angle = rotate.Next(7) * 10;
                    rt.BeginAnimation(RotateTransform.AngleProperty, da);
                    //removing the positions (making them unavailable)
                    columns.RemoveAt(0);
                    rows.RemoveAt(0);
                    notSelectedbtnd.Add(item);
                    btn = item;
                }               
            }
            selectedbtnd.Remove(btn);
            md1.Position = TimeSpan.FromSeconds(2);
            md1.Play();
        }
        private void moveDice()
        {
            Random rndr = new Random();
            Random rndc = new Random();
            Random rotate = new Random();
            rows = rows.OrderBy(x => rndr.Next()).ToList();
            columns = columns.OrderBy(x => rndc.Next()).ToList();
            for (int i = 0; i < selectedbtnd.Count; i++)
            {
               deck.Add(Grid.GetColumn(selectedbtnd[i]));
                Grid.SetColumn(selectedbtnd[i], columns[0]);
                Grid.SetRow(selectedbtnd[i],rows[0]);
                selectedbtnd[i].RenderTransform = new RotateTransform(rotate.Next(7)*10,0,0);
                //removing the positions (making them unavailable)
                columns.RemoveAt(0);
                rows.RemoveAt(0);
            }
            notSelectedbtnd.AddRange(selectedbtnd);
            selectedbtnd.Clear();
        }
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Dice d = e.Source as Dice;
            d.StartRoll();
            Counter++;
        }
        private void resetDiebtn(Button btn)
        {
            RotateTransform rt = new RotateTransform(0, 0, 0);
            btn.RenderTransform = rt;
            selectedbtnd.Add(btn);
            notSelectedbtnd.Remove(btn);
            rows.Add(Grid.GetRow(btn));
            columns.Add(Grid.GetColumn(btn));
            deck.Sort();
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
            Grid.SetRow(btn, 6);
            Grid.SetColumn(btn, deck[0]);
            deck.RemoveAt(0);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (rolls == 1) 
            { 
                Button a = e.Source as Button;
                if(Grid.GetRow(a)==6)//means I want to unselect so I bring it back to the last position that was emptied...
                {
                    md3.Position = TimeSpan.Zero ;
                    md3.Play();
                    deck.Add(Grid.GetColumn(a));
                    Grid.SetRow(a, rows[rows.Count-1]);
                    Grid.SetColumn(a, columns[columns.Count-1]);
                    rows.RemoveAt(rows.Count-1);
                    columns.RemoveAt(columns.Count-1);
                    selectedbtnd.Remove(a);
                    notSelectedbtnd.Add(a);
                    a.RenderTransform= new RotateTransform( random.Next(7)*10);
                }
                else
                {
                    md2.Position =TimeSpan.Zero; 
                    md2.Play();
                    resetDiebtn(a);
                }
            }          
        }
        private void setMessage(string message)
        {
            MessageBox.Show(message);
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {        
			//kkkk.Start_Click(sender,e);
			//dg.Start_Click();
            if (rolls > 0)
            {
                Counter = selectedbtnd.Count;
                if (Counter == 0)
                {
                    setMessage("dude you haven't selected any dice");
                    return;
                }
                foreach (Button item in selectedbtnd)
                    foreach (Dice d in alldice)
                    {
                        if (item.Name == d.Name + "Btn")
                        {
                            d.StartRoll();                           
                        }
                    }                
            }
            else
            {
                setMessage("you must select an option");
            }                
        }       
    }
}