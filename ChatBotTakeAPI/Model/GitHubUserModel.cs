using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotTakeAPI.Model
{
    class GitHubUserModel
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Avatar_URL { get; set; }
        public string URL { get; set; }
        public List<ProjetoModel> Projetos { get; set; }
    }
}
