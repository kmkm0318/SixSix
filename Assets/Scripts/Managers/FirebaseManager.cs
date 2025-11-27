using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

/// <summary>
/// 파이어베이스를 통해서 리더보드에 점수를 추가하고 가져오는 매니저
/// </summary>
public class FirebaseManager : Singleton<FirebaseManager>
{
    private FirebaseFirestore _firestore;
    private FirebaseAuth _auth;
    private FirebaseUser _user;
    //파이어베이스 초기화 완료 여부
    //로그인까지 완료하거나 실패로 끝난 경우에 true
    public bool _isInitialized { get; private set; } = false;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                _firestore = FirebaseFirestore.DefaultInstance;
                _auth = FirebaseAuth.DefaultInstance;

                Debug.Log("Firebase initialization completed.");

                SignInAnonymously();
            }
            else
            {
                Debug.LogError("Firebase initialization failed: " + task.Exception);
                _isInitialized = true;
            }
        });
    }

    //로그인은 익명으로 진행. 플레이어를 ID를 통해 구분하기 위해 필요.
    private void SignInAnonymously()
    {
        //로그인 되어있는 경우에는 진행하지 않음
        if (_auth.CurrentUser != null)
        {
            Debug.Log("User already signed in.");
            _user = _auth.CurrentUser;

            _isInitialized = true;

            return;
        }

        //익명으로 로그인
        _auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                _user = task.Result.User;
            }
            else
            {
                Debug.LogError("Anonymous sign-in failed: " + task.Exception);
            }

            _isInitialized = true;
        });
    }

    public async Task AddScoreToLeaderboard(double score)
    {
        if (_user == null) return;

        Dictionary<string, object> data = new()
        {
            { "playerID", _user.UserId },
            { "playerName", "Anonymous_" + _user.UserId },
            { "score", score },
            { "timestamp", FieldValue.ServerTimestamp }
        };

        try
        {
            await _firestore.Collection("leaderboard").AddAsync(data);
            Debug.Log("add score to leaderboard completed.");
        }
        catch (Exception e)
        {
            Debug.LogError("Add Score To Leaderboard failed: " + e.Message);
        }
    }

    public async Task<List<LeaderboardEntry>> GetTopScores(int days = 7, int topN = 10)
    {
        if (_firestore == null) return null;

        List<LeaderboardEntry> leaderboard = new();

        try
        {
            DateTime startDate = DateTime.UtcNow.AddDays(-days);

            Query q = _firestore
            .Collection("leaderboard")
            .WhereGreaterThanOrEqualTo("timestamp", startDate)
            .OrderByDescending("score")
            .Limit(topN);

            QuerySnapshot ss = await q.GetSnapshotAsync();

            foreach (var doc in ss.Documents)
            {
                var data = doc.ToDictionary();
                leaderboard.Add(new()
                {
                    playerName = data["playerName"].ToString(),
                    score = Convert.ToDouble(data["score"])
                });
            }

            return leaderboard;
        }
        catch (Exception e)
        {
            Debug.LogError("Get Top Scores failed: " + e.Message);
            return null;
        }
    }

    public async Task<double> GetMyBestScore()
    {
        if (_firestore == null || _user == null) return 0;

        try
        {
            Query q = _firestore
            .Collection("leaderboard")
            .WhereEqualTo("playerID", _user.UserId)
            .OrderByDescending("score")
            .Limit(1);

            QuerySnapshot ss = await q.GetSnapshotAsync();

            if (ss.Documents.Count() > 0)
            {
                var data = ss.Documents.First().ToDictionary();
                return Convert.ToDouble(data["score"]);
            }

            return 0;
        }
        catch (Exception e)
        {
            Debug.LogError("Get My Best Score failed: " + e.Message);
            return 0;
        }
    }
}
