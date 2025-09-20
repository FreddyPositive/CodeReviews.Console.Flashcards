using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCardLearning.Model.entities
{
    internal class FlashCards
    {
        public int Id {get; set;}
        public string? Question { get; set; }
        public string? Answer { get; set; }
        public int StackId { get; set; }
    }
}
