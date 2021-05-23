using Application.Interfaces.Service;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

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
        public async void SendMessage(IReadOnlyList<string> registration_ids, string messageValue)
        {
            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                {
                    {"type",1.ToString() },
                    { "message", messageValue}
                },
                Tokens = registration_ids,
                Notification = new Notification
                {
                    Title = "Title",
                    Body = "Body"
                }
            };
            await firebaseMessaging.SendMulticastAsync(message);
        }
    }
}
