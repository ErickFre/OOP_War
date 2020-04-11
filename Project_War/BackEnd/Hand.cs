using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd
{
    public class Hand
    {
        private List<Card> cards = new List<Card>();

        public int Count
        {
            get
            {
                return cards.Count();
            }
        }

        //Places discard pile back into hand
        public void RestockHand(Hand h1, Hand h2)
        {
            int amount = h2.cards.Count;
            for (int i = 0; i < amount; i++)
            {
                h1.AddCard(h2.cards[0]);
                h2.RemoveCard(0);
                
            }
        }



        public Card this[int index]
        {
            get
            {
                return cards[index];
            }
        }

        public void ShuffleHand()
        {
            Random rGen = new Random();
            List<Card> newDeck = new List<Card>();
            while (cards.Count > 0)
            {
                int removeIndex = rGen.Next(0, (cards.Count));
                Card removeObject = cards[removeIndex];
                cards.RemoveAt(removeIndex);
                //  Add the removed card to the new deck.
                newDeck.Add(removeObject);
            }

            //  replace the old deck with the new deck
            cards = newDeck;
        }


        public void AddCard(Card newCard)
        {
            // the List<T>.Contains method cannot be used since it only checks if the same reference object exists
            if (ContainsCard(newCard))
            {
                throw new ConstraintException(newCard.FaceValue.ToString() + " of " +
                    newCard.Suit.ToString() + " already exists in the Hand");
            }

            cards.Add(newCard);
        }


        private bool ContainsCard(Card cardToCheck)
        {
            foreach (Card card in cards)
            {
                if (card.FaceValue == cardToCheck.FaceValue && card.Suit == cardToCheck.Suit)
                {
                    return true;
                }
            }

            return false;
        }

        public void RemoveCard(Card theCard)
        {
            if (cards.Contains(theCard))
            {
                cards.Remove(theCard);
            }
            else
            {
                throw new ConstraintException(theCard.FaceValue.ToString() + " of " +
                    theCard.Suit.ToString() + " does not exist in the Hand");
            }
        }

        public void RemoveCard(int index)
        {
            if (index <= cards.Count - 1)
            {
                cards.RemoveAt(index);
            }
            else
            {
                throw new DataException("Index value exceeds the number of cards in the hand.");
            }
        }

        public void RemoveCard(Suit theSuit, FaceValue theFaceValue)
        {
            Card removeCard = cards.Where(card => card.Suit == theSuit && card.FaceValue == theFaceValue).FirstOrDefault();

            if (removeCard != null)
            {
                cards.Remove(removeCard);
            }
            else
            {
                throw new DataException(theFaceValue.ToString() + " of " + theSuit.ToString() + " does not exist in the Hand.");
            }
        }

        //empties hand
        public void Clear()
        {
            for(int i = 0; i < this.Count; i++)
            {
                this.RemoveCard(0);
            }
        }
    }
}
