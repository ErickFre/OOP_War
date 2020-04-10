using BackEnd;
using System;
using System.Collections.Generic;
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
        public Hand player1hand;
        public Hand player2hand;
        public Hand player1Discard = new Hand();
        public Hand player2Discard = new Hand();
        public Hand player1Bet = new Hand();
        public Hand player2Bet = new Hand();
        public Card player1Battle;
        public Card player2Battle;
        Deck aDeck;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetUp()
        {
            try
            {
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

        private void ShowHand(Hand hand, Canvas nameOfCanvas)
        {
            Image img = new Image()
            {
                Source = new BitmapImage(new Uri($@"images\cardback.gif", UriKind.Relative)),
                Height = 100,
                Width = 75,
                Tag = hand
            };
            //img.MouseDown += Img_MouseDown;
            //Canvas.SetLeft(img, 75);
            nameOfCanvas.Children.Add(img);
        }
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

        private void btn_Battle_Click(object sender, RoutedEventArgs e)
        {
            if (player1hand.Count <= 0 || player2hand.Count <= 0)
            {
                MessageBox.Show("Error", "A Player is out of cards", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                player1Battle = player1hand[0];
                player2Battle = player2hand[0];
                FightCalculation(player1Battle, player2Battle);
            }
            
        }

        private void FightCalculation(Card player1Battle, Card player2Battle)
        {
            player2hand.RemoveCard(0);
            player1hand.RemoveCard(0);

            ShowHand(player1Battle, cnv_P1BattleCard);
            ShowHand(player2Battle, cnv_P2BattleCard);

            if ((int)player1Battle.FaceValue > (int)player2Battle.FaceValue)
            {
                player1Discard.AddCard(player1Battle);
                player1Discard.AddCard(player2Battle);
                ShowHand(player1Discard, cnv_P1DiscardPile);
                updateCardAmounts();
            }
            else if((int)player1Battle.FaceValue < (int)player2Battle.FaceValue)
            {
                player2Discard.AddCard(player1Battle);
                player2Discard.AddCard(player2Battle);
                ShowHand(player2Discard, cnv_P2DiscardPile);
                updateCardAmounts();
            }
            else
            {

                MessageBox.Show("You've played the same card!", "This means WAR!",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);

                for (int i = 0; i < 3; i++)
                {
                    player1Bet.AddCard(player1hand[i]);

                    Image img = new Image()
                    {
                        Source = new BitmapImage(new Uri($@"images\{player1hand[i].FaceValue}{player1hand[i].Suit}.jpg", UriKind.Relative)),
                        Height = 100,
                        Width = 75
                    };
                    Canvas.SetLeft(img, 30 * i);
                    cnv_P1Bet.Children.Add(img);
                    player1hand.RemoveCard(player1hand[i]);
                    updateCardAmounts();
                }

                for (int i = 0; i < 3; i++)
                {
                    player2Bet.AddCard(player2hand[i]);

                    Image img = new Image()
                    {
                        Source = new BitmapImage(new Uri($@"images\{player2hand[i].FaceValue}{player2hand[i].Suit}.jpg", UriKind.Relative)),
                        Height = 100,
                        Width = 75
                    };
                    Canvas.SetLeft(img, 30 * i);
                    cnv_P2Bet.Children.Add(img);
                    player1hand.RemoveCard(player1hand[i]);
                    updateCardAmounts();
                }

                MessageBox.Show("Press OK to go to war!", "Ready?",
                    MessageBoxButton.OK, MessageBoxImage.Question);

                player1Battle = player1hand[4];
                player2Battle = player2hand[4];
                player2hand.RemoveCard(4);
                player1hand.RemoveCard(4);

                ShowHand(player1Battle, cnv_P1BattleCard);
                ShowHand(player2Battle, cnv_P2BattleCard);

                if ((int)player1Battle.FaceValue > (int)player2Battle.FaceValue)
                {
                    player1Discard.AddCard(player1Battle);
                    player1Discard.AddCard(player2Battle);
                    for (int i = 0; i < 3; i++)
                    {
                        player1Discard.AddCard(player1Bet[i]);
                        player1Discard.AddCard(player2Bet[i]);
                    }
                    player1Bet.Clear(player1Bet);
                    ShowHand(player1Discard, cnv_P1DiscardPile);
                    updateCardAmounts();
                }
                else if ((int)player1Battle.FaceValue < (int)player2Battle.FaceValue)
                {
                    player2Discard.AddCard(player1Battle);
                    player2Discard.AddCard(player2Battle);
                    for (int i = 0; i < 3; i++)
                    {
                        player2Discard.AddCard(player1Bet[i]);
                        player2Discard.AddCard(player2Bet[i]);
                    }
                    player2Bet.Clear(player2Bet);
                    ShowHand(player2Discard, cnv_P2DiscardPile);
                    updateCardAmounts();
                }


            }
        }

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

        

        private void btn_StartGame_Click(object sender, RoutedEventArgs e)
        {
            SetUp();
        }

        private void btn_RestartGame_Click(object sender, RoutedEventArgs e)
        {
            SetUp();
        }

        private void btn_RestockP1_Click(object sender, RoutedEventArgs e)
        {
            player1Discard.ShuffleHand();
            player1hand.RestockHand(player1hand, player1Discard);
            player1Discard.Clear(player1Discard);
            updateCardAmounts();
        }

        private void btn_RestockP2_Click(object sender, RoutedEventArgs e)
        {
            player2Discard.ShuffleHand();
            player2hand.RestockHand(player2hand, player2Discard);
            player2Discard.Clear(player2Discard);
            updateCardAmounts();
        }
    }
}
