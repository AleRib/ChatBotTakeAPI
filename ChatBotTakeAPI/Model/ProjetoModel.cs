using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotTakeAPI.Model
{
    class ProjetoModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Full_Name { get; set; }
        public string Description { get; set; }
        public DateTime created_at { get; set; }
    }
}
