using System;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using System.Diagnostics;
using Take.Blip.Client;
using Lime.Messaging.Contents;
using ChatBotTakeAPI.Service;
using ChatBotTakeAPI.Model;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ChatBotTakeAPI.Controller
{

    public class MessageReceiver : IMessageReceiver
    {
        //Url para acesso as informações da Take no Github
        private readonly string USER_GIT_URL = "https://api.github.com/users/takenet";
        /*Url para acesso os repositórios públicos, e já ordenados por data, da Take no Github. 
        A ordenação e tipo de repositório são passados via parâmetros na url.*/
        private readonly string USER_REPOS_URL = "https://api.github.com/users/takenet/repos?type=public&sort=created&direction=asc";

        private readonly ISender _sender;
        GitHubUserModel gitUser;

        public MessageReceiver(ISender sender)
        {
            _sender = sender;
        }

        //Executado quando o usuário envia uma mensagem no chat.
        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            gitUser = await getGitUserInformationAsync();

            Document document;
            document = getDocumentCollectionMenuMultimidia();

            
            //Retorna uma mensagem no chat
            await _sender.SendMessageAsync(document, message.From, cancellationToken);

        }

        //Busca os dados no git via requisição HTTP Get e retorna um objeto GitHubUserModel com as informações.
        private async Task<GitHubUserModel> getGitUserInformationAsync()
        {
            String userResponse = await HTTPRequestService.Instance.GetAsync(USER_GIT_URL);
            String reposResponse = await HTTPRequestService.Instance.GetAsync(USER_REPOS_URL);

            JObject userObject = JObject.Parse(userResponse);
            GitHubUserModel gitUser = JsonConvert.DeserializeObject<GitHubUserModel>(userObject.ToString());

            JArray reposArray = JArray.Parse(reposResponse);
            List<ProjetoModel> userRepos = JsonConvert.DeserializeObject<List<ProjetoModel>>(reposArray.ToString());

            gitUser.Projetos = userRepos.GetRange(0, 5);

            return gitUser;
        }


        //Cria um carrossel de cards com as informações dos 5 primeiros repositórios da Take no GitHub.
        public DocumentCollection getDocumentCollectionMenuMultimidia()
        {

            List<DocumentSelect> documents = new List<DocumentSelect>();

            
            foreach(ProjetoModel projeto in gitUser.Projetos)
            {
                documents.Add(new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            Title = projeto.Full_Name,
                            Text = projeto.Description,
                            Type = "image/jpeg",
                            Uri = new Uri(gitUser.Avatar_URL),
                        }
                    }
                });

            }

            var document = new DocumentCollection
            {
                ItemType = "application/vnd.lime.document-select+json",
                Items = documents.ToArray(),
            };

            return document;
        }
    }
}
