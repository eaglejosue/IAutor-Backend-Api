﻿namespace Pay4Tru.Api.Data.Model;

public sealed class UserLog
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public long UserId { get; set; }
    public string Log { get; set; }

    [JsonIgnore] public User? User { get; set; }

    public UserLog() { }
    public UserLog(long userId, string log)
    {
        CreatedAt = DateTimeBr.Now;
        UserId = userId;
        Log = log;
    }
}