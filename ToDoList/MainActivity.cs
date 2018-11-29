using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ToDoList.Helper;
using Xamarin.Facebook;
using Xamarin.Facebook.Login.Widget;

namespace ToDoList
{
    [Activity(Label = "ToDoList", Theme = "@style/Theme.AppCompat.Light", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IFacebookCallback
    {
        private ICallbackManager mFBCallManager;
        private MyProfileTracker mprofileTracker;
        private TextView TxtFirstName;
        private TextView TxtLastName;
        private TextView TxtName;
        private ProfilePictureView mprofile;
        LoginButton BtnFBLogin;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            FacebookSdk.SdkInitialize(this.ApplicationContext);
            mprofileTracker = new MyProfileTracker();
            mprofileTracker.mOnProfileChanged += mProfileTracker_mOnProfileChanged;
            mprofileTracker.StartTracking();
            // Set our view from the "main" layout resource  
            SetContentView(Resource.Layout.login);
            BtnFBLogin = FindViewById<LoginButton>(Resource.Id.fblogin);
            TxtFirstName = FindViewById<TextView>(Resource.Id.TxtFirstname);
            TxtLastName = FindViewById<TextView>(Resource.Id.TxtLastName);
            TxtName = FindViewById<TextView>(Resource.Id.TxtName);
            mprofile = FindViewById<ProfilePictureView>(Resource.Id.ImgPro);
            BtnFBLogin.SetReadPermissions(new List<string> {
        "user_friends",
        "public_profile"
    });
            mFBCallManager = CallbackManagerFactory.Create();
            BtnFBLogin.RegisterCallback(mFBCallManager, this);
        }

        internal void LoadTaskList()
        {
            throw new NotImplementedException();
        }

        public void OnCancel() { }
        public void OnError(FacebookException p0) { }
        public void OnSuccess(Java.Lang.Object p0) { }
        void mProfileTracker_mOnProfileChanged(object sender, OnProfileChangedEventArgs e)
        {
            if (e.mProfile != null)
            {
                try
                {
                    TxtFirstName.Text = e.mProfile.FirstName;
                    TxtLastName.Text = e.mProfile.LastName;
                    TxtName.Text = e.mProfile.Name;
                    mprofile.ProfileId = e.mProfile.Id;
                }
                catch (Java.Lang.Exception ex) { }
            }
            else
            {
                TxtFirstName.Text = "First Name";
                TxtLastName.Text = "Last Name";
                TxtName.Text = "Name";
                mprofile.ProfileId = null;
            }
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            mFBCallManager.OnActivityResult(requestCode, (int)resultCode, data);
        }
        public class MyProfileTracker : ProfileTracker
        {
            public event EventHandler<OnProfileChangedEventArgs> mOnProfileChanged;
            protected override void OnCurrentProfileChanged(Profile oldProfile, Profile newProfile)
            {
                if (mOnProfileChanged != null)
                {
                    mOnProfileChanged.Invoke(this, new OnProfileChangedEventArgs(newProfile));
                }
            }
        }
        public class OnProfileChangedEventArgs : EventArgs
        {
            public Profile mProfile;
            public OnProfileChangedEventArgs(Profile profile)
            {
                mProfile = profile;
            }
        }


    }
}

