using System;
using Android.App;
using Android.Util;
using Firebase.Iid;


namespace PeerCover.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    [Obsolete]

    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";

        [Obsolete]
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);
            Console.WriteLine(refreshedToken);
            HelperAppSettings.AppToken = refreshedToken;
            SendRegistrationToServerAsync(refreshedToken);
        }
        void SendRegistrationToServerAsync(string refreshedToken)
        {

        }

    }
}