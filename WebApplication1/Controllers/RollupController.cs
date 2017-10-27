using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using WebApplication1.Models;
using WebApplication1.Utils;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class RollupController : Controller
    {
        private const string fbProductionAppSecret = "qSR94yDFpnkuQfgzVq871v877Go4sONlnJ49x7gk";
        private const string fbProductionURL = "https://vutiliti-platform.firebaseio.com";

        private static FirebaseClient firebase = null;

        private const int FAKE_MULTIPLIER = 253;

        private FirebaseClient GetFirebase()
        {
            if (firebase == null)
            {
                firebase = new FirebaseClient(
                fbProductionURL,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(fbProductionAppSecret)
                });
            }
            return firebase;
        }

        // GET rollup/v1
        [HttpGet]
        public async Task<ReadSummary> Get()
        {
            FirebaseClient firebase = GetFirebase();
            string dbpath = $"reads/{{24657390-e870-11e6-e0c3-15445952f162}}/reads";

            var col = await firebase
                    .Child(dbpath)
                    .OrderByKey()
                    .LimitToLast(10)
                    .OnceAsync<SimpleRead>();
            var reads = new ReadSummary();

            foreach (var entry in col)
            {
                reads.Add(Convert.ToInt64(entry.Key), new SimpleRead(entry.Object.Delta * FAKE_MULTIPLIER));
            }

            return reads;
        }

        // GET rollup/v1/{DeviceID}/{Start}/{End}/{TimeUnit}
        [HttpGet("{DeviceID}/{Start}/{End}/{TimeUnit}")]
        public async Task<ReadSummary> Get(string deviceID, long start, long end, string timeUnit)
        {
            FirebaseClient firebase = GetFirebase();
            ReadSummary reads = new ReadSummary();

            // Ensure the start and end values
            // are in milliseconds.
            start = start.DateTime().EpochMilliseconds();

            if (end > 0)
            {
                end = end.DateTime().EpochMilliseconds();
            }
            else
            {
                // If the specified end date is 0 (or less),
                // then set it to right now.
                end = DateTime.Now.EpochMilliseconds();
            }

            // Figure out the bucket size.
            timeUnit = timeUnit.ToLowerInvariant();
            if (timeUnit == "daily" || timeUnit == "days")
            {
                timeUnit = "days";
            }
            else if (timeUnit == "hourly" || timeUnit == "hours")
            {
                timeUnit = "hours";
            }
            else if (timeUnit == "minutes")
            {
                timeUnit = "minutes";
            }
            else if (timeUnit == "raw")
            {
                timeUnit = "raw";
            }
            else
            {
                return reads;
            }

            string dbpath;

            if (timeUnit == "raw")
            {
                dbpath = $"reads/{{24657390-e870-11e6-e0c3-15445952f162}}/reads";
            }
            else
            {
                dbpath = $"reads/{{24657390-e870-11e6-e0c3-15445952f162}}/read_summaries/{timeUnit}";
            }

            var col = await firebase
                    .Child(dbpath)
                    .OrderByKey()
                    .StartAt($"{start}")
                    .EndAt($"{end}")
                    //.LimitToLast(10)
                    .OnceAsync<SimpleRead>();

            foreach (var entry in col)
            {
                reads.Add(Convert.ToInt64(entry.Key), new SimpleRead(entry.Object.Delta * FAKE_MULTIPLIER));
            }

            return reads;
        }
    }
}