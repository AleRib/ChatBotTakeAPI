using System;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using System.Diagnostics;
using Take.Blip.Client;
using Lime.Messaging.Contents;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace MediaReceiver
{

    public class MessageReceiver : IMessageReceiver
    {
        private readonly ISender _sender;
        private HttpClient client = new HttpClient();
        Document[] documents;
        JsonDocument jsonDocuments;
        JsonDocument jsonDocuments2;
        JsonDocument jsonDocuments3;

        public MessageReceiver(ISender sender)
        {
            _sender = sender;
        }

        public async Task ReceiveAsync(Message message, CancellationToken cancellationToken)
        {
            Document document;
            document = getDocumentCollectionMenuMultimidia();

            String s = await GetAsync();

            Trace.TraceInformation($"From: {message.From} \tContent: {message.Content}");
            await _sender.SendMessageAsync(document, message.From, cancellationToken);

        }


        public async Task<string> GetAsync()
        {
            var responseString = "";
            var s = "";
            HttpResponseMessage response;
            try
            {

                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
                response = await client.GetAsync("https://api.github.com/users/takenet/repos?type=public&sort=created&direction=asc");
                //responseString = await client.GetStringAsync("https://api.github.com/users/takenet/repos?type=public&sort=created&direction=asc");
                // response = await client.GetAsync("https://api.github.com/");
                if (response.IsSuccessStatusCode)
                {
                    responseString = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    responseString = response.ToString();
                }
                JArray jsonArray = JArray.Parse(responseString);
                JObject j = null;

                for (int i = 0; i < 5; i++)
                {
                    j = JObject.Parse(jsonArray[i].ToString());
                    s += j["name"].Value<string>();
                }
                //responseString = await client.GetStringAsync("https://api.github.com/users/takenet/repos?client_id=xxxx&client_secret=yyyy");
                //responseString = await client.GetStringAsync("https://api.github.com/users/takenet/repos?type=public&sort=created&direction=asc");
            }
            catch (Exception e)
            {
                return e.Message;
            }



            //return new string[] { "value1", "value2" };
            return s;
        }


        public DocumentCollection getDocumentCollectionMenuMultimidia()
        {
            jsonDocuments = new JsonDocument();
            jsonDocuments2 = new JsonDocument();
            jsonDocuments3 = new JsonDocument();

            jsonDocuments.Add("Key1", "value1");
            jsonDocuments.Add("Key2", "2");

            jsonDocuments2.Add("Key3", "value3");
            jsonDocuments2.Add("Key4", "4");

            jsonDocuments3.Add("Key5", "value5");
            jsonDocuments3.Add("Key6", "6");

            DocumentSelect[] documents = new DocumentSelect[]
            {
                new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            Title = "Title",
                            Text = "This is a first item",
                            Type = "image/jpeg",
                            Uri = new Uri("http://www.isharearena.com/wp-content/uploads/2012/12/wallpaper-281049.jpg"),
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer
                            {
                                Value = new WebLink
                                {
                                    Title = "Link",
                                    Uri = new Uri("http://www.adoteumgatinho.org.br/")
                                }
                            }
                        },
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer
                            {
                                Value = new PlainText
                                {
                                    Text = "Text 1"
                                }
                            },
                            Value = new DocumentContainer
                            {
                                Value = jsonDocuments
                            }
                        }
                    }
                },
                new DocumentSelect
                {
                    Header = new DocumentContainer
                    {
                        Value = new MediaLink
                        {
                            Title = "Title 2",
                            Text = "This is another item",
                            Type = "image/jpeg",
                            Uri = new Uri("http://www.freedigitalphotos.net/images/img/homepage/87357.jpg")
                        }
                    },
                    Options = new DocumentSelectOption[]
                    {
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer
                            {
                                Value = new WebLink
                                {
                                    Title = "Second link",
                                    Text = "Weblink",
                                    Uri = new Uri("https://pt.dreamstime.com/foto-de-stock-brinquedo-pl%C3%A1stico-amarelo-do-pato-image44982058")
                                }
                            }
                        },
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer
                            {
                                Value = new PlainText {
                                    Text = "Second text"
                                }
                            },
                            Value = new DocumentContainer
                            {
                                Value = jsonDocuments2
                            }
                        },
                        new DocumentSelectOption
                        {
                            Label = new DocumentContainer
                            {
                                Value = new PlainText {
                                    Text = "More one text"
                                }
                            },
                            Value = new DocumentContainer
                            {
                                Value = jsonDocuments3
                            }
                        }
                    }
                }

            };

            var document = new DocumentCollection
            {
                ItemType = "application/vnd.lime.document-select+json",
                Items = documents,
            };

            return document;
        }
    }
}
