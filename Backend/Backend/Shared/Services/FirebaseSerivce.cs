using Application.DTOs.Firebase;
using Application.Enums;
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

        public FirebaseSerivce()
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
        }
        public async Task<IReadOnlyList<SendResponse>> SendMessage(IReadOnlyList<string> registration_ids, string messageValue, NotificationType type)
        {
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                {
                    {"type", ((int)type).ToString() },
                    { "message", messageValue}
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
