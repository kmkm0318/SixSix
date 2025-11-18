using System.IO;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using System;

public class DatabaseManager : Singleton<DatabaseManager>
{
    private SqliteConnection connection;
    private string dbPath;

    protected override void Awake()
    {
        base.Awake();

        dbPath = Path.Combine(Application.persistentDataPath, "GameDB.sqlite");
        Initialize();
    }

    private void OnApplicationQuit()
    {
        if (connection != null && connection.State == System.Data.ConnectionState.Open)
        {
            connection.Close();
            Debug.Log("Database connection closed");
        }
    }

    private void Initialize()
    {
        if (connection != null) return;

        Debug.Log($"DB Path : {dbPath}");
        connection = new SqliteConnection($"Data Source={dbPath};Version=3;");
        connection.Open();

        CreateTables();
    }

    private void CreateTables()
    {
        string createGameRunsSql = @"
        CREATE TABLE IF NOT EXISTS GameRuns(
            RunID INTEGER PRIMARY KEY AUTOINCREMENT,
            ClearedRound INTEGER NOT NULL,
            RunTimestamp TEXT NOT NULL
        );";

        string createRunDiceSql = @"
        CREATE TABLE IF NOT EXISTS RunDice(
            RunID INTEGER,
            AbilityDiceID INTEGER,
            PRIMARY KEY (RunID, AbilityDiceID),
            FOREIGN KEY (RunID) REFERENCES GameRuns(RunID)
        );";

        using (var command = connection.CreateCommand())
        {
            command.CommandText = createGameRunsSql;
            command.ExecuteNonQuery();
            command.CommandText = createRunDiceSql;
            command.ExecuteNonQuery();
        }

        Debug.Log("Tables created or already exist");
    }

    public void SaveGameRun(List<int> diceIDs, int clearedRound)
    {
        if (diceIDs == null || diceIDs.Count == 0) return;

        var uniqueDiceIDs = new HashSet<int>(diceIDs);

        if (connection == null || connection.State != System.Data.ConnectionState.Open)
        {
            Debug.LogError("Database not initialized");
            return;
        }

        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                string insertRunSql = "INSERT INTO GameRuns (ClearedRound, RunTimestamp) VALUES (@round, datetime('now', 'localtime'));";
                long runId;

                using (var command = new SqliteCommand(insertRunSql, connection, transaction))
                {
                    command.Parameters.AddWithValue("@round", clearedRound);
                    command.ExecuteNonQuery();

                    command.CommandText = "SELECT last_insert_rowid();";

                    runId = (long)command.ExecuteScalar();
                }

                string insertDiceSql = "INSERT INTO RunDice (RunID, AbilityDiceID) VALUES (@runId, @diceId);";
                foreach (var diceId in uniqueDiceIDs)
                {
                    using (var command = new SqliteCommand(insertDiceSql, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@runId", runId);
                        command.Parameters.AddWithValue("@diceId", diceId);
                        command.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                Debug.Log($"Game Run (ID: {runId}) and Dice combination saved");
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Debug.LogError($"Failed to save game run: {e.Message}");
            }
        }
    }

    public int GetCombinationClearedRound(List<int> diceIds)
    {
        if (diceIds == null || diceIds.Count == 0) return 0;

        diceIds.Sort();
        string combination = string.Join(",", diceIds);

        string query = @"
            SELECT AVG(A.ClearedRound)
            FROM GameRuns AS A
            JOIN
                (
                SELECT RunID, GROUP_CONCAT(AbilityDiceID ORDER BY AbilityDiceID ASC, ',') AS DiceCombination
                FROM RunDice
                GROUP BY RunID
                ) AS B
            ON A.RunID = B.RunID
            WHERE B.DiceCombination = @combination
            ";

        using (var command = connection.CreateCommand())
        {
            command.CommandText = query;
            command.Parameters.AddWithValue("@combination", combination);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read() && !reader.IsDBNull(0))
                {
                    return (int)reader.GetDouble(0);
                }
            }
        }

        return 0;
    }
}