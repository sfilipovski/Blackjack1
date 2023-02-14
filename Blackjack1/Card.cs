using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack1
{
    public enum Shape
    {
        Diamond = 1,
        Heart,
        Spade,
        Club
    }
    public class Card
    {
        public int Value { get; set; }
        public String Image { get; set; }
        public Shape shape { get; set; }

        public Card(int Value, Shape shape, String image)
        {
            this.Value = Value;
            this.Image = image;
            this.shape = shape;
        }
    }
}
