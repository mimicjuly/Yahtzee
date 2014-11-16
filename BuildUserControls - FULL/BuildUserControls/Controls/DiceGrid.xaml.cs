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
	/// Interaction logic for DiceGrid.xaml
	/// </summary>
	public partial class DiceGrid : UserControl
	{
		int rolls=2;
		static int Counter=5;
		static Random random = new Random();
		DoubleAnimation da = new DoubleAnimation();                                                     
        List<Dice> alldice = new List<Dice>();
        List<Button> selectedbtnd = new List<Button>();
        List<Button> notSelectedbtnd = new List<Button>();
		List<int> deck = new List<int>();
        List<int> rows = new List<int>(){ 0, 1, 2, 3, 4 };
        List<int> columns = new List<int>{ 0, 1, 2, 3, 4,5,6 };
		int[] position = { 1, 2, 3, 4, 5 };
		public DiceGrid()
		{
			this.InitializeComponent();
			this.AddHandler(Dice.StopRollEvent, new RoutedPropertyChangedEventHandler<int>((X, Y) => { rollended(Y.OldValue); }));
  		  	alldice.Add(Dice6);
            alldice.Add(Dice7);
            alldice.Add(Dice8);
            alldice.Add(Dice9);
            alldice.Add(Dice10);
            selectedbtnd.Add(Dice6Btn);
            selectedbtnd.Add(Dice7Btn);
            selectedbtnd.Add(Dice8Btn);
            selectedbtnd.Add(Dice9Btn);
            selectedbtnd.Add(Dice10Btn);
            da.AutoReverse = true;
            da.Duration = new Duration(TimeSpan.FromSeconds(3));
            da.RepeatBehavior = new RepeatBehavior(TimeSpan.FromSeconds(9));
			
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
                //game.player.suggestion(alldice[0].State+1, alldice[1].State+1, alldice[2].State+1, alldice[3].State+1, alldice[4].State+1);
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
        public void Start_Click()
        {                          
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("gh");
        }
	}
}