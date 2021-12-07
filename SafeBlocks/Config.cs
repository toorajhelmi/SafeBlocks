using System;
namespace SafeBlocks
{
    public enum ExecutionMode
    {
        Debug,
        Release
    }

    public enum ExecutionSpeed
    {
        ExtraSlow,
        Slow,
        Normal,
        Fast,
        ExtraFast
    }

    public enum SafetyMode
    {
        Definite,
        Partial
    }

    public class Config
    {
        public static ExecutionMode ExectuionMode { get; set; } = ExecutionMode.Debug;
        public static ExecutionSpeed ExecutionSpeed { get; set; } = ExecutionSpeed.Normal;
        public static SafetyMode SafetyMode { get; set; } = SafetyMode.Definite;
    }
}
