using Application.DTOs.Firebase;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class FirebaseSerivce : IFirebaseSerivce
    {
        private readonly string path;
        private readonly FirebaseApp app;
        private readonly FirebaseMessaging firebaseMessaging;
        //private readonly IFirebaseTokenRepositoryAsync _firebaseTokenRepository;

        public FirebaseSerivce(/*IFirebaseTokenRepositoryAsync firebaseTokenRepository*/)
        {
            path = "firebase-admin.json";
            try
            {
                app = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(path)
                }, "myApp");
            }
            catch (Exception ex)
            {
                app = FirebaseApp.GetInstance("myApp");
            }
            firebaseMessaging = FirebaseMessaging.GetMessaging(app);
            //this._firebaseTokenRepository = firebaseTokenRepository;
        }
        public async Task<IReadOnlyList<SendResponse>> SendMessage(IReadOnlyList<string> registration_ids, MessageNotiData messageValue)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };

            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                {
                    {"type",1.ToString() },
                    { "message", JsonConvert.SerializeObject(messageValue,settings)}
                },
                Tokens = registration_ids,
                Android = new AndroidConfig{ 
                    Priority = Priority.High
                },
            };
            return (await firebaseMessaging.SendMulticastAsync(message)).Responses;
        }

        public async Task<IReadOnlyList<SendResponse>> SendReceiveRequestNotification(IReadOnlyList<string> registration_ids, ReceiveRequestNotificationData receiveRequestData)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var settings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                {
                    {"type",2.ToString() },
                    { "message", JsonConvert.SerializeObject(receiveRequestData,settings)}

                },
                Tokens = registration_ids,
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                },
            };
            return (await firebaseMessaging.SendMulticastAsync(message)).Responses;
        }


    }
}
