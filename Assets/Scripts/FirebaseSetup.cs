using System;
using Firebase;
using Firebase.Analytics;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Messaging;
using UnityEngine;

public class FirebaseSetup : MonoBehaviour
{
    FirebaseApp app;
    public static FirebaseSetup instance;
    DatabaseReference reference;
    public string CurrentLeaderboardStatName;
    private void Awake()
    {
        instance = this;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Firebase is ready to use
                Invoke(nameof(InitializeFirebase), 0.5f);
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
        //Invoke(nameof(InitializeFirebase), 1f);
    }
    void WriteData(string key, string value)
    {
        // Set the value in the "currentleaderboard" node
        Debug.Log("Here");
        reference.Child(key).SetValueAsync(value);
        Debug.Log("Data written to Firebase Realtime Database.");
    }
    void FetchData(string key)
    {
        reference.Child(key).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // Handle the error
                Debug.LogError("Failed to fetch data from Firebase: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                // Retrieve the data
                DataSnapshot snapshot = task.Result;
                string retrievedValue = snapshot.Value.ToString();
                CurrentLeaderboardStatName = retrievedValue;
                loginWithPlayFab.instance.statName = retrievedValue;
                Debug.Log("Retrieved value from Firebase: " + retrievedValue);
            }
        });
    }

    private void InitializeFirebase()
    {
        app = FirebaseApp.DefaultInstance;
        FirebaseMessaging.TokenReceived += OnTokenReceived;
        FirebaseMessaging.MessageReceived += OnMessageReceived;
        LogAppOpenEvent();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //WriteData("Currentleaderboard", "ColorPuzzleWeek2");
        FetchData("Currentleaderboard");
    }

    private void OnTokenReceived(object sender, TokenReceivedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    {
        throw new NotImplementedException();
    }
    void LogAppOpenEvent()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);
    }
    public void LogWatchedAdEvent(string adtype = "1")
    {
        FirebaseAnalytics.LogEvent("watched_ad_event" + adtype);
    }
    public void LogStartingAdEvent(string adtype = "1")
    {
        FirebaseAnalytics.LogEvent("starting_ad_event" + adtype);
    }
    public void LogStartingAdClickEvent(string adtype = "1")
    {
        FirebaseAnalytics.LogEvent("starting_click_ad_event" + adtype);
    }

}
