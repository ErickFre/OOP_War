using BackEnd;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project_War
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Hand player1hand;
        Hand player2hand;
        Hand player1Discard = new Hand();
        Hand player2Discard = new Hand();
        Hand player1Bet = new Hand();
        Hand player2Bet = new Hand();
        Card player1Battle;
        Card player2Battle;
        Deck aDeck;

        public MainWindow()
        {
            InitializeComponent();
        }

        //Clears and prepares the app for a new game
        private void SetUp()
        {
            try
            {

                tb_log.Text = "";
                btn_Battle.IsEnabled = true;
                btn_RestartGame.IsEnabled = true;

                aDeck = new Deck();
                aDeck.Shuffle();
                player1hand = aDeck.DealHand(26);
                player2hand = aDeck.DealHand(26);
                
                ShowHand(player1hand, cnv_P1Deck);
                ShowHand(player2hand, cnv_P2Deck);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        //Displays a hand of cards (in this game hands are face down)
        private void ShowHand(Hand hand, Canvas nameOfCanvas)
        {
            Image img = new Image()
            {
                Source = new BitmapImage(new Uri($@"images\cardback.gif", UriKind.Relative)),
                Height = 100,
                Width = 75,
                Tag = hand
            };
            nameOfCanvas.Children.Add(img);
        }

        //Shows the face of a given card
        private void ShowHand(Card card, Canvas nameOfCanvas)
        {
            Image img = new Image()
            {
                Source = new BitmapImage(new Uri($@"images\{card.FaceValue}{card.Suit}.jpg", UriKind.Relative)),
                Height = 100,
                Width = 75
            };
            //Canvas.SetTop(img, 0);
            //Canvas.SetLeft(img, 0);
            nameOfCanvas.Children.Add(img);
        }

        //Decides if there is a winner by seeing if both players have enough cards to play,
        //if there is not a winner yet - a battle begins.
        private void btn_Battle_Click(object sender, RoutedEventArgs e)
        {
            if ((player1hand.Count <= 0  && player1Discard.Count < 0) || (player2hand.Count <= 0 && player2Discard.Count <= 0))
            {
                MessageBox.Show("A Player is out of cards. Restock your troops!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if(player2hand.Count == 0 && player2Discard.Count == 0)
            {
                MessageBox.Show("Player 1 has won the war! Press the restart game button to play again.", "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (player1hand.Count == 0 && player1Discard.Count == 0)
            {
                MessageBox.Show("Player 2 has won the war! Press the restart game button to play again.", "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                //Battle
                player1Battle = player1hand[0];
                player2Battle = player2hand[0];
                FightCalculation(player1Battle, player2Battle);
            }
            
        }

        //takes the battle cards and measures their worth based on face value.
        //The highest card wins the round (Ace is high)
        private void FightCalculation(Card player1Battle, Card player2Battle)
        {
            player2hand.RemoveCard(0);
            player1hand.RemoveCard(0);

            ShowHand(player1Battle, cnv_P1BattleCard);
            ShowHand(player2Battle, cnv_P2BattleCard);

            if ((int)player1Battle.FaceValue > (int)player2Battle.FaceValue)
            {
                btn_RestockP1.IsEnabled = true;
                player1Discard.AddCard(player1Battle);
                player1Discard.AddCard(player2Battle);
                for (int i = 0; i < player1Bet.Count; i++)
                {
                    player1Discard.AddCard(player1Bet[0]);
                    player1Discard.AddCard(player2Bet[0]);

                }
                player1Bet.Clear();
                player2Bet.Clear();
                cnv_P1Bet.Children.Clear();
                cnv_P2Bet.Children.Clear();
                ShowHand(player1Discard, cnv_P1DiscardPile);
                updateCardAmounts();
                tb_log.Text += "Player one wins the round!\r\n";
            }
            else if((int)player1Battle.FaceValue < (int)player2Battle.FaceValue)
            {
                btn_RestockP2.IsEnabled = true;
                player2Discard.AddCard(player1Battle);
                player2Discard.AddCard(player2Battle);
                for (int i = 0; i < player2Bet.Count; i++)
                {
                    player1Discard.AddCard(player1Bet[0]);
                    player1Discard.AddCard(player2Bet[0]);
                }
                player1Bet.Clear();
                player2Bet.Clear();
                cnv_P1Bet.Children.Clear();
                cnv_P2Bet.Children.Clear();
                ShowHand(player2Discard, cnv_P2DiscardPile);
                updateCardAmounts();
                tb_log.Text += "Player two wins the round!\r\n";
            }
            else
            {

                MessageBox.Show("You've played the same card!", "This means WAR!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);

                War(player1Bet, player2Bet);

            }
        }

        //Players have played the same car. Bets are placed and a second battle
        //is fought. Winner takes everything.
        private void War(Hand player1Bet, Hand player2Bet)
        {
            for (int i = 0; i < 3; i++)
            {
                Image img = new Image()
                {
                    Source = new BitmapImage(new Uri($@"images\{player1hand[i].FaceValue}{player1hand[i].Suit}.jpg", UriKind.Relative)),
                    Height = 100,
                    Width = 75
                };
                Canvas.SetLeft(img, 30 * i);
                cnv_P1Bet.Children.Add(img);

                Image img2 = new Image()
                {
                    Source = new BitmapImage(new Uri($@"images\{player2hand[i].FaceValue}{player2hand[i].Suit}.jpg", UriKind.Relative)),
                    Height = 100,
                    Width = 75
                };
                Canvas.SetLeft(img2, 30 * i);
                cnv_P2Bet.Children.Add(img2);
            }


            MessageBox.Show("Press OK to go to war!", "Ready?",
                MessageBoxButton.OK, MessageBoxImage.Question);

            player1Battle = player1hand[0];
            player2Battle = player2hand[0];

            FightCalculation(player1Battle, player2Battle);

            if ((int)player1Battle.FaceValue > (int)player2Battle.FaceValue)
            {
                MessageBox.Show("Player 1 has won the war! They win the pot.",
                                 "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            { 
                MessageBox.Show("Player 2 has won the war! They win the pot.",
                                 "Congratulations!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }


        }

        //Updates textblocks with up to date numbers on what each hand currently has
        public void updateCardAmounts()
        {
            tb_P1Remaining.Text = $"{player1hand.Count} :Remaining";
            tb_P2Remaining.Text = $"Remaining: {player2hand.Count}";
            tb_P1Discard.Text = $"P1 War Reserves: {player1Discard.Count}";
            tb_P2Discard.Text = $"P2 War Reserves: {player2Discard.Count}";

            if(player1Discard.Count == 0)
            {
                cnv_P1DiscardPile.Children.Clear();
            }
            if (player2Discard.Count == 0)
            {
                cnv_P2DiscardPile.Children.Clear();
            }
        }

        
        //Begins the game
        private void btn_StartGame_Click(object sender, RoutedEventArgs e)
        {
            SetUp();
        }

        //Reloads the window
        private void btn_RestartGame_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You will lose your progress. Are you sure?", "App Restart", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();

            
        }

        //Shuffles discard pile and places it into main hand (p1)
        private void btn_RestockP1_Click(object sender, RoutedEventArgs e)
        {
            player1Discard.ShuffleHand();
            player1hand.RestockHand(player1hand, player1Discard);
            player1Discard.Clear();
            updateCardAmounts();
        }

        //Shuffles discard pile and places it into main hand (p2)
        private void btn_RestockP2_Click(object sender, RoutedEventArgs e)
        {
            player2Discard.ShuffleHand();
            player2hand.RestockHand(player2hand, player2Discard);
            player2Discard.Clear();
            updateCardAmounts();
        }
    }
}
