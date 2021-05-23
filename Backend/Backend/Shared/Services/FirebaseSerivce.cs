using Application.DTOs.Firebase;
using Application.Interfaces.Repositories;
using Application.Interfaces.Service;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
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
        public async Task<int> SendMessage(IReadOnlyList<string> registration_ids, MessageNotiData messageValue)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var message = new MulticastMessage()
            {
                Data = new Dictionary<string, string>()
                {
                    {"type",1.ToString() },
                    { "message", JsonSerializer.Serialize(messageValue,options)}
                },
                Tokens = registration_ids,
            };
            var responses = (await firebaseMessaging.SendMulticastAsync(message)).Responses;
            for(int i = 0; i < responses.Count; i++)
            {
                var response = responses[i];
                if(!response.IsSuccess)
                {
                    //var result = await _firebaseTokenRepository.GetByConditionAsync(f => f.Token == registration_ids[i]);
                    //await _firebaseTokenRepository.DeleteAsync(result[0]);
                }
            }
            return 0;
        }
    }
}
