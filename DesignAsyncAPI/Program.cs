﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace Bnaya.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            ExecOldFashion();
            Console.WriteLine("-------------------");
            Task _ = ExecAsync();
            Console.WriteLine("-------------------");
            Task __ = ExecValueAsync();
            Console.ReadKey(true);
        }

        private static async ValueTask<int> ExeAcync() => await new ValueTask<int>(1);

        #region GetContent

        private static string GetContent() => File.ReadAllText("Content.txt");

        #endregion // GetContent

        #region GetContentAsync

        private static async Task<string> GetContentAsync()
        {
            using (var srm = new FileStream("Content.txt",
                                    FileMode.Open, FileAccess.Read, FileShare.Read,
                                    4096, FileOptions.Asynchronous))
            using (var reader = new StreamReader(srm))
            {
                string content = await reader.ReadToEndAsync().ConfigureAwait(false);
                return content;
            }
        }

        #endregion // GetContentAsync

        #region GetValueContentAsync

        // Value Task is Task Like
        private static async ValueTask<string> GetValueContentAsync()
        {
            using (var srm = new FileStream("Content.txt",
                                    FileMode.Open, FileAccess.Read, FileShare.Read,
                                    4096, FileOptions.Asynchronous))
            using (var reader = new StreamReader(srm))
            {
                string content = await reader.ReadToEndAsync().ConfigureAwait(false);
                return content;
            }
        }

        #endregion // GetValueContentAsync

        #region ExecOldFashion

        private static void ExecOldFashion()
        {
            var logic = new Logic();

            logic.OldFashion(GetContent, "OldFashion -> sync");

            // you don't want this kind of problem (cause)
            logic.OldFashion(() => GetContentAsync().Result, "OldFashion -> async.Result");
        }

        #endregion // ExecOldFashion

        #region ExecAsync

        private static async Task ExecAsync()
        {
            var logic = new Logic();

            await logic.TapFashion(() => Task.FromResult(GetContent()), "Sync -> Wrap").ConfigureAwait(false);
            await logic.TapFashion(GetContentAsync, "TAP -> async").ConfigureAwait(false);
        }

        #endregion // ExecAsync

        #region ExecValueAsync

        private static async Task ExecValueAsync()
        {
            var logic = new Logic();

            string syncContent = GetContent();
            await logic.VTapFashion(() => new ValueTask<string>(syncContent), "Sync -> Wrap");
            await logic.VTapFashion(() => syncContent.ToValueTask(), "TAP -> Wrap(sync)");

            Task<string> asyncContent = GetContentAsync();
            await logic.VTapFashion(() => new ValueTask<string>(asyncContent), "TAP -> async");
            await logic.VTapFashion(async () => await asyncContent.ConfigureAwait(false), "TAP -> async");

            await logic.VTapFashion(GetValueContentAsync, "VTAP -> async");
        }

        #endregion // ExecValueAsync

        #region ExecLightAsync

        private static async Task ExecLightAsync()
        {
            var logic = new Logic();

            await logic.LTapFashion(() => GetContent(), "OldFashion -> sync");
            await logic.LTapFashion(() => GetContentAsync(), "TAP -> async");
        }

        #endregion // ExecLightAsync

    }
}