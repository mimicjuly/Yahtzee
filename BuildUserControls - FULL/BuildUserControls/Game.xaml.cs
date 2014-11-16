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
using System.Windows.Shapes;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel; 
using System.Windows.Navigation; 
using System.Windows.Shapes;
namespace BuildUserControls
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {        
        public class NameNScore
        {
            public string Name { get; set; }
            public int Score { get; set; }
            public int TotalScore { get; set; }
        }
        
        private Label lb;
        private TextBox txt;
        private int rounds;
        private Player plyr;
        private List<TextBox> tx = new List<TextBox>();
        public ObservableCollection<Player> players = new ObservableCollection<Player>()
        { new Player { Name = "" } };

        public Grid sheetGrid = new Grid();

        public Game(int rounds)
        {
            InitializeComponent();
            DataContext = players;
            tx.Add(text0);
            txt = text0;
            Rounds = rounds;
            plyr = players.First();
			Keyboard.Focus(txt);
        }

        private void plus_Click(object sender, RoutedEventArgs e)
        {
            if (txt.Text == "")
            {
                MessageBox.Show("you must enter a name first");
                return;
            }
            if (players.Count == grid1.RowDefinitions.Count - 6)
            {
                grid1.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(35) });
                this.Height += 30;
            }
            txt = new TextBox();
            players.Add(new Player());
            lb = new Label();
            Grid.SetRow(lb, players.Count + 1);
            grid1.Children.Add(lb);
            Grid.SetRow(txt, players.Count + 1);
            grid1.Children.Add(txt);
            tx.Add(txt);
            Grid.SetRow(plus, players.Count + 1);
            Grid.SetRow(minus, players.Count + 1);
            Grid.SetRow(start, players.Count + 2);
            Keyboard.Focus(txt);
            minus.IsEnabled = true;
        }
        private void start_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (tx[i].Text == "")
                {
                    Keyboard.Focus(tx[i]);
                    return;
                }
                players[i].Name = tx[i].Text;
                players[i].setIsTurn(false);
            }
            player.sheet.Visibility = Visibility.Visible;
            foreach (Player item in players)
            {
                sheetGrid.Children.Add(item.sheet);
            }
            this.Close();
        }
        private void minus_Click(object sender, RoutedEventArgs e)
        {
            if (players.Count == 2)
                minus.IsEnabled = false;
            if (players.Count == grid1.RowDefinitions.Count - 6)
            {
                grid1.RowDefinitions.RemoveAt(grid1.RowDefinitions.Count - 1);
                this.Height -= 30;
            }
            grid1.Children.RemoveAt(grid1.Children.Count - 1);
            grid1.Children.RemoveAt(grid1.Children.Count - 1);
            tx.RemoveAt(tx.Count - 1);
            txt = tx.Last();
            players.RemoveAt(players.Count - 1);
            Grid.SetRow(plus, players.Count + 1);
            Grid.SetRow(minus, players.Count + 1);
            Grid.SetRow(start, players.Count + 2);
        }
		
        
        private void maxi_Click(object sender, RoutedEventArgs e)
        {
			
			
         	if(this.WindowState == WindowState.Normal)
			{
				this.WindowState= WindowState.Maximized;
				normal.Visibility=Visibility.Visible;
				maximize.Visibility=Visibility.Hidden;
			}
			else
			{
				this.WindowState= WindowState.Normal;
				normal.Visibility=Visibility.Hidden;
				maximize.Visibility=Visibility.Visible;
			}
		}
		private void exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        public int Rounds 
        {
            get { return rounds; }
            set { rounds = value;
            if (rounds==0)
                OnGameEnded();
            }
        }       
        public Player player 
        {
            get { return plyr; }          
        }      
        protected void OnGameEnded()
        {
            List<NameNScore> list = new List<NameNScore>();
            GameEnded ge;
            foreach (Player item in players)
	        {
		        list.Add( new NameNScore{ Name = item.Name,
                Score= item.Score, TotalScore=item.TotalScore});
	        }
            list.Sort(delegate(NameNScore Object1, NameNScore Object2)
            {
                return Object2.TotalScore.CompareTo(Object1.TotalScore);
            });
            ge = new GameEnded(list);
            ge.ShowDialog();
            if(ge.newGame)
            {
                System.Windows.Forms.Application.Restart();
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                resetSheets();
                Rounds = 13;
            }
        }        
        public Player NextPlayer()
        {
            int index = players.IndexOf(plyr);
            if(++index==players.Count)
            {
                index = 0;
                Rounds--;
            }
            plyr.sheet.Visibility = Visibility.Hidden;
            plyr = players[index];
            plyr.sheet.Visibility = Visibility.Visible;
            return player;
        }
        public void resetSheets()
        {
            foreach (Player item in players)
            {
                item.reset();
            }
        }                    
    }
}
