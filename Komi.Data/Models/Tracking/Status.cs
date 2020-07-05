using System;
using System.Collections.Generic;
using System.Linq;

namespace Komi.Data.Models.Tracking
{
    public enum Status
    {
        Unknown,
        InProgress,
        Finished,
        Dropped
    }

    public static class StatusExtensions
    {
        public static Status GetStatus(this IEnumerable<Status> statuses)
        {
            var enumerable = statuses as Status[] ?? statuses.ToArray();

            foreach (Status status in Enum.GetValues(typeof(Status)))
            {
                if (enumerable.All(s => s == status))
                    return status;
            }

            if (enumerable.Any(s =>
                s == Status.InProgress
             || s == Status.Unknown
             || s == Status.Dropped))
                return Status.InProgress;

            if (enumerable.All(s => s != Status.InProgress))
                return Status.Finished;

            return Status.Unknown;
        }

        public static string GetImage(this Status status)
        {
            var baseUrl = "https://img.shields.io/badge/Status-";
            string statusUrl = status switch
            {
                Status.Unknown    => "Unknown-lightgrey",
                Status.InProgress => "In_Progress-yellow",
                Status.Finished   => "Finished-green",
                Status.Dropped    => "Dropped-red",
                _                 => "Unknown-lightgrey"
            };

            return baseUrl + statusUrl;
        }
    }
}