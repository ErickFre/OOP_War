using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd
{
    public class Deck
    {
        private List<Card> _deck = new List<Card>();

        public Deck()
        {
            MakeDeck();
        }

        private void MakeDeck()
        {
            // there are 4 suits
            for (int pickASuit = 0; pickASuit <= 3; pickASuit++)
            {
                // there are 13 cards per suit
                for (int pickAValue = 0; pickAValue <= 12; pickAValue++)
                {
                    // create a card for the current suit and value
                    Card newCard = new Card((Suit)pickASuit, (FaceValue)pickAValue);

                    // add the card to the deck
                    _deck.Add(newCard);
                }
            }
        }

        public void Shuffle()
        {
            Random rGen = new Random();
            List<Card> newDeck = new List<Card>();
            while (_deck.Count > 0)
            {
                int removeIndex = rGen.Next(0, (_deck.Count));
                Card removeObject = _deck[removeIndex];
                _deck.RemoveAt(removeIndex);
                //  Add the removed card to the new deck.
                newDeck.Add(removeObject);
            }

            //  replace the old deck with the new deck
            _deck = newDeck;
        }

        //  Public Sub Shuffle()
        //     start a random number generator
        //     create a temporary list of cards
        // loop 
        //     until there are no cards left in the original deck
        //     generate a random number from 0 to the number of cards left in the deck
        //     save the card at that position in the deck
        //     remove that card from the original deck
        //     add it to the new deck
        // End loop
        //  replace the old deck with the new deck
        // End Sub

        public Hand DealHand(int number)
        {
            if (_deck.Count == 0)
            {
                throw new ConstraintException("There are no cards left in the deck. Redeal.");
            }

            Hand hand = new Hand();
            if (_deck.Count >= number)
            {
                for (int handIndex = 0; handIndex < number; handIndex++)
                {
                    hand.AddCard(_deck[0]);
                    _deck.RemoveAt(0);
                }
            }
            else
            {
                for (int handIndex = 0; handIndex < _deck.Count; handIndex++)
                {
                    hand.AddCard(_deck[0]);
                    _deck.RemoveAt(0);
                }
            }
            return hand;

        }

        public Card DrawOneCard()
        {
            Card topCard;
            if ((_deck.Count > 0))
            {
                topCard = _deck[0];
                _deck.RemoveAt(0);
            }
            else
            {
                throw new ArgumentException("There are no cards in the deck - deal again");
            }

            return topCard;
        }
    }
}
