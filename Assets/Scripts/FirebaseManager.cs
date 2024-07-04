using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;

public class FirebaseManager : MonoBehaviour
{
    private DatabaseReference dbRef;
    public static FirebaseManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
        });
    }
    public void InitializeFirebase()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void SaveScore(int score)
    {
        dbRef.Child("score").SetValueAsync(score);
    }
    public void LoadScore(System.Action<int> onLoaded)
    {
        if (dbRef == null) return;
        dbRef.Child("score").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error loading score from Firebase");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot dataSnapshot = task.Result;
                if (dataSnapshot.Exists)
                {
                    int loadedScore = int.Parse(dataSnapshot.Value.ToString());
                    onLoaded.Invoke(loadedScore);
                }
            }
            else
            {
                Debug.Log("Score does not exist in the database.");
                onLoaded?.Invoke(0);
            }
        });
    }
}

