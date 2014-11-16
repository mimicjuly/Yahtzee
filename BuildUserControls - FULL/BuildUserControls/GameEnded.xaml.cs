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

namespace BuildUserControls
{
    /// <summary>
    /// Interaction logic for GameEnded.xaml
    /// </summary>
    public partial class GameEnded : Window
    {
        //List<Game.NameNScore> collection;
        public bool newGame { get; set; }
        public GameEnded(List<Game.NameNScore> collection)
        {
            InitializeComponent();
            dataGrid1.ItemsSource = collection;
        }

        private void newGameBtn_Click(object sender, RoutedEventArgs e)
        {
            newGame = true;
            this.Close();
        }

        private void anotherRoundBtn_Click(object sender, RoutedEventArgs e)
        {
            newGame = false;
            this.Close();
        }

        private void gameEndBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
