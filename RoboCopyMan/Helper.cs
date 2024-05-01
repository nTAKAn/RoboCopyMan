using System.Diagnostics;

namespace RoboCopyMan
{
    /// <summary>
    /// ヘルパークラス
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// コマンドを実行する
        /// </summary>
        /// <param name="command">コマンド</param>
        /// <param name="stdOutput">標準出力</param>
        /// <param name="stdError">標準エラー出力</param>
        /// <returns>終了コード</returns>
        /// <exception cref="Exception"></exception>
        public static int ExecuteCommand(string command, out string stdOutput, out string stdError)
        {
            ProcessStartInfo psInfo = new()
            {
                FileName = "cmd",
                Arguments = "/c " + command,
                CreateNoWindow = true,  // コンソール開かない。
                UseShellExecute = false,  // シェル機能使用しない。
                RedirectStandardOutput = true,  // 標準出力をリダイレクト。
                RedirectStandardError = true,  // 標準エラー出力をリダイレクト。
            };

            using Process process = Process.Start(psInfo) ?? throw new Exception();

            // 標準出力を全て取得。
            stdOutput = process.StandardOutput.ReadToEnd();
            stdError = process.StandardError.ReadToEnd();

            process.WaitForExit();
            int exitCode = process.ExitCode;
            process.Close();

            Debug.WriteLine($"StdOutput: {stdOutput}");
            Debug.WriteLine($"StdError: {stdError}");
            Debug.WriteLine($"ExitCode: {exitCode}");
            return exitCode;
        }
    }
}
